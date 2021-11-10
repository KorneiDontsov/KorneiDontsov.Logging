// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Destructurama;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Microsoft.Extensions.Hosting;
	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;

	public static class ConfiguredLoggingFunctions {
		static void ConfigureMinLevels (LoggerConfiguration loggerConf, IConfiguration conf) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException SourceIsNotSpecified (IConfigurationSection overrideConf) =>
				new($"'{overrideConf.Path}' -- source to be overriden is not specified.");

			var minLevelsConf = conf.GetSection("minLevels");
			var minLevel = minLevelsConf.ReadEnum<LogEventLevel>("default", defaultValue: LogEventLevel.Information);
			loggerConf.MinimumLevel.Is(minLevel);

			foreach(var overrideConf in minLevelsConf.GetSection("overrides").GetChildren()) {
				var source = overrideConf.Key;
				if(String.IsNullOrWhiteSpace(source)) throw SourceIsNotSpecified(overrideConf);

				var sourceMinLevel = overrideConf.ReadEnum<LogEventLevel>();
				loggerConf.MinimumLevel.Override(source, sourceMinLevel);
			}
		}

		static void ConfigureProfiles
			(LoggerConfiguration loggerConf,
			 IConfiguration conf,
			 IEnumerable<ILoggingProfileApplier> profileAppliers) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static ArgumentException Conflict
				(String profileTypeName,
				 ILoggingProfileApplier applier,
				 ILoggingProfileApplier otherApplier) {
				var msg =
					$"{applier.GetType()} cannot handle profile type '{profileTypeName}' because it's already handled "
					+ $"by {otherApplier.GetType()}.";
				return new(msg, nameof(profileAppliers));
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException ProfileTypeIsNotKnown (LoggingProfileConfiguration profileConf) =>
				new($"Profile type '{profileConf.profileTypeName}' of profile '{profileConf.Path}' is not known.");

			var profileAppliersMap = new Dictionary<String, ILoggingProfileApplier>();
			foreach(var profileApplier in profileAppliers) {
				var profileTypeName = profileApplier.profileTypeName.ToLowerInvariant();
				if(! profileAppliersMap.TryGetValue(profileTypeName, out var otherApplier))
					profileAppliersMap.Add(profileTypeName, profileApplier);
				else
					throw Conflict(profileTypeName, profileApplier, otherApplier);
			}

			var profileConfs = conf.GetSection("profiles").GetChildren();
			foreach(var profileConfRaw in profileConfs) {
				var profileConf = new LoggingProfileConfiguration(profileConfRaw);
				if(profileAppliersMap.TryGetValue(profileConf.profileTypeName, out var profileApplier))
					profileApplier.Apply(loggerConf.WriteTo, profileConf);
				else
					throw ProfileTypeIsNotKnown(profileConf);
			}
		}

		static void ConfigureEnrichments
			(LoggerConfiguration loggerConf,
			 IConfiguration conf,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static ArgumentException Conflict
				(String enrichmentName,
				 ILoggingEnrichmentApplier applier,
				 ILoggingEnrichmentApplier otherApplier) {
				var msg =
					$"{applier.GetType()} cannot handle enrichment '{enrichmentName}' because it's already handled "
					+ $"by {otherApplier.GetType()}.";
				return new(msg, nameof(enrichmentAppliers));
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException EnrichmentIsNotKnown
				(IConfigurationSection enrichmentConf, String enrichmentName) =>
				new($"Name '{enrichmentName}' of enrichment '{enrichmentConf.Path}' is not known.");

			var enrichmentAppliersMap = new Dictionary<String, ILoggingEnrichmentApplier>();
			foreach(var enrichmentApplier in enrichmentAppliers) {
				var enrichmentName = enrichmentApplier.enrichmentName.ToLowerInvariant();
				if(! enrichmentAppliersMap.TryGetValue(enrichmentName, out var otherApplier))
					enrichmentAppliersMap.Add(enrichmentName, enrichmentApplier);
				else
					throw Conflict(enrichmentName, enrichmentApplier, otherApplier);
			}

			var enrichmentConfs = conf.GetSection("enrichments").GetChildren();
			foreach(var enrichmentConf in enrichmentConfs) {
				var enrichmentName = enrichmentConf.Key.ToLowerInvariant();
				if(enrichmentAppliersMap.TryGetValue(enrichmentName, out var enrichmentApplier))
					enrichmentApplier.Apply(loggerConf.Enrich, enrichmentConf);
				else
					throw EnrichmentIsNotKnown(enrichmentConf, enrichmentName);
			}
		}

		static void ConfigureFilters
			(LoggerConfiguration loggerConf,
			 IConfiguration conf,
			 IEnumerable<ILoggingFilterApplier> filterAppliers) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static ArgumentException Conflict
				(String filterName,
				 ILoggingFilterApplier applier,
				 ILoggingFilterApplier otherApplier) {
				var msg =
					$"{applier.GetType()} cannot handle filter '{filterName}' because it's already handled by "
					+ $"{otherApplier.GetType()}.";
				return new(msg, nameof(filterAppliers));
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException FilterIsNotKnown
				(IConfigurationSection filterConf, String filterName) =>
				new($"Name '{filterName}' of filter '{filterConf.Path}' is not known.");

			var filterAppliersMap = new Dictionary<String, ILoggingFilterApplier>();
			foreach(var filterApplier in filterAppliers) {
				var filterName = filterApplier.filterName.ToLowerInvariant();
				if(! filterAppliersMap.TryGetValue(filterName, out var otherApplier))
					filterAppliersMap.Add(filterName, filterApplier);
				else
					throw Conflict(filterName, filterApplier, otherApplier);
			}

			var filterConfs = conf.GetSection("filters").GetChildren();
			foreach(var filterConf in filterConfs) {
				var filterName = filterConf.Key.ToLowerInvariant();
				if(filterAppliersMap.TryGetValue(filterName, out var filterApplier))
					filterApplier.Apply(loggerConf.Filter, filterConf);
				else
					throw FilterIsNotKnown(filterConf, filterName);
			}
		}

		/// <exception cref = "LoggingConfigurationException" />
		public static Logger CreateConfiguredLogger
			(IConfiguration conf,
			 IEnumerable<ILoggingProfileApplier> profileAppliers,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers,
			 IEnumerable<ILoggingFilterApplier> filterAppliers,
			 OnLoggerConfigurationLoaded? onLoggerConfigurationLoaded = null) {
			var loggerConf =
				new LoggerConfiguration()
					.Destructure.UsingAttributes()
					.Enrich.FromLogContext();

			ConfigureMinLevels(loggerConf, conf);
			ConfigureProfiles(loggerConf, conf, profileAppliers);
			ConfigureEnrichments(loggerConf, conf, enrichmentAppliers);
			ConfigureFilters(loggerConf, conf, filterAppliers);
			onLoggerConfigurationLoaded?.Invoke(loggerConf);

			return loggerConf.CreateLogger();
		}

		/// <exception cref = "LoggingConfigurationException" />
		public static Logger CreateConfiguredLogger
			(IConfiguration conf,
			 IEnumerable<ILoggingProfileApplier> profileAppliers,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers) =>
			CreateConfiguredLogger(
				conf,
				profileAppliers,
				enrichmentAppliers,
				Enumerable.Empty<ILoggingFilterApplier>());

		/// <exception cref = "LoggingConfigurationException" />
		public static Logger CreateSharedConfiguredLogger
			(IConfiguration conf,
			 IEnumerable<ILoggingProfileApplier> profileAppliers,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers) {
			var logger = CreateConfiguredLogger(conf, profileAppliers, enrichmentAppliers);
			Log.Logger = logger;
			return logger;
		}

		public static IServiceCollection AddConfiguredLoggerCore (this IServiceCollection services) {
			services.TryAddSingleton<LoggingAppEnvironment>();
			return services.AddSingleton<ILogger, ConfiguredLogger>()
				.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory?, MicrosoftLoggerFactory>()
				.AddAotLogger();
		}

		public static IServiceCollection AddConfiguredLogger (this IServiceCollection services) =>
			services.AddConfiguredLoggerCore()
				.AddTransient<ILoggingProfileApplier, ConsoleProfileApplier>()
				.AddTransient<ILoggingProfileApplier, LogFileProfileApplier>()
				.AddTransient<ILoggingEnrichmentApplier, ThreadEnrichmentApplier>();

		public static IHostBuilder UseConfiguredLogger (this IHostBuilder hostBuilder) =>
			hostBuilder.ConfigureServices((context, services) => services.AddConfiguredLogger());

		public static IHostBuilder UseConfiguredLogger
			(this IHostBuilder hostBuilder, OnLoggerConfigurationLoaded onConfigurationLoaded) =>
			hostBuilder.ConfigureServices(
				(context, services) =>
					services.AddConfiguredLogger()
						.AddSingleton(onConfigurationLoaded));
	}
}
