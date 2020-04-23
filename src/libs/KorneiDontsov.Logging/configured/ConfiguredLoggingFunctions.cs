// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Serilog;
	using Serilog.Core;
	using System;
	using System.Collections.Generic;

	public static class ConfiguredLoggingFunctions {
		/// <exception cref = "LoggingConfigurationException" />
		public static Logger CreateConfiguredLogger
			(IConfiguration conf,
			 IEnumerable<ILoggingProfileApplier> profileAppliers,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers) {
			var profileAppliersMap = new Dictionary<String, ILoggingProfileApplier>();
			foreach(var profileApplier in profileAppliers) {
				var profileTypeName = profileApplier.profileTypeName.ToLowerInvariant();
				if(profileAppliersMap.TryGetValue(profileTypeName, out var otherApplier)) {
					var msg =
						$"{profileApplier.GetType()} cannot handle profile type '{profileTypeName}' "
						+ $"because it's already handled by {otherApplier.GetType()}.";
					throw new ArgumentException(msg, nameof(profileAppliers));
				}
				else
					profileAppliersMap.Add(profileTypeName, profileApplier);
			}

			var enrichmentAppliersMap = new Dictionary<String, ILoggingEnrichmentApplier>();
			foreach(var enrichmentApplier in enrichmentAppliers) {
				var enrichmentName = enrichmentApplier.enrichmentName.ToLowerInvariant();
				if(enrichmentAppliersMap.TryGetValue(enrichmentName, out var otherApplier)) {
					var msg =
						$"{enrichmentApplier.GetType()} cannot handle enrichment '{enrichmentName}' "
						+ $"because it's already handled by {otherApplier.GetType()}.";
					throw new ArgumentException(msg, nameof(enrichmentAppliers));
				}
				else
					enrichmentAppliersMap.Add(enrichmentName, enrichmentApplier);
			}

			var loggerConf = new LoggerConfiguration().Enrich.FromLogContext();

			var profileConfs = conf.GetSection("profiles").GetChildren();
			foreach(var profileConfRaw in profileConfs) {
				var profileConf = new LoggingProfileConfiguration(profileConfRaw);
				if(profileAppliersMap.TryGetValue(profileConf.profileTypeName, out var profileApplier))
					profileApplier.Apply(loggerConf.WriteTo, profileConf);
				else {
					var msg = $"'{profileConf.Path}:type': profile type '{profileConf.profileTypeName}' is not known.";
					throw new LoggingConfigurationException(msg);
				}
			}

			var enrichmentsConf = conf.GetSection("enrichments").GetChildren();
			foreach(var enrichmentConf in enrichmentsConf) {
				var enrichmentName = enrichmentConf.Key.ToLowerInvariant();
				if(enrichmentAppliersMap.TryGetValue(enrichmentName, out var enrichmentApplier))
					enrichmentApplier.Apply(loggerConf.Enrich, enrichmentConf);
				else {
					var msg = $"'{enrichmentConf.Path}:type': enrichment '{enrichmentName}' is not known.";
					throw new LoggingConfigurationException(msg);
				}
			}

			return loggerConf.CreateLogger();
		}

		/// <exception cref = "LoggingConfigurationException" />
		public static Logger CreateSharedConfiguredLogger
			(IConfiguration conf,
			 IEnumerable<ILoggingProfileApplier> profileAppliers,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers) {
			var logger = CreateConfiguredLogger(conf, profileAppliers, enrichmentAppliers);
			Log.Logger = logger;
			return logger;
		}

		public static IHostBuilder UseConfiguredLogger (this IHostBuilder hostBuilder) =>
			hostBuilder.ConfigureServices(
				(context, services) =>
					services
						.AddSingleton<LoggingAppEnvironment>()
						.AddTransient<ILoggingProfileApplier, ConsoleProfileApplier>()
						.AddTransient<ILoggingProfileApplier, LogFileProfileApplier>()
						.AddTransient<ILoggingEnrichmentApplier, ThreadEnrichmentApplier>()
						.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, ConfiguredLoggerFactory>()
						.AddSingleton<ILogger?>(
							provider => {
								var loggerFactory = provider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
								return (loggerFactory as ConfiguredLoggerFactory)?.logger;
							})
						.AddSingleton(
							provider => {
								var logger = provider.GetService<ILogger?>();
								return logger is {} ? new AotLogger(logger) : null;
							}));
	}
}
