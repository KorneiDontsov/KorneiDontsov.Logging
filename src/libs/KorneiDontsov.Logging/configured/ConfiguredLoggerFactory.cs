// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;
	using Serilog;
	using Serilog.Core;
	using Serilog.Debugging;
	using Serilog.Extensions.Logging;
	using System;
	using System.Collections.Generic;
	using static ConfiguredLoggingFunctions;

	class ConfiguredLoggerFactory: ILoggerFactory {
		public Logger logger { get; }
		Serilog.ILogger overridenLogger { get; }
		SerilogLoggerProvider provider { get; }

		public ConfiguredLoggerFactory
			(IConfiguration configuration,
			 IEnumerable<ILoggingProfileApplier> profileAppliers,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers,
			 IEnumerable<ILoggingFilterApplier> filterAppliers) {
			logger =
				CreateConfiguredLogger(
					configuration.GetSection("logging"),
					profileAppliers,
					enrichmentAppliers,
					filterAppliers);
			(overridenLogger, Log.Logger) = (Log.Logger, logger);
			provider = new(logger, dispose: false);
		}

		void IDisposable.Dispose () {
			Log.Logger = overridenLogger;
			logger.Dispose();
		}

		/// <inheritdoc />
		public Microsoft.Extensions.Logging.ILogger CreateLogger (String categoryName) =>
			provider.CreateLogger(categoryName);

		/// <inheritdoc />
		public void AddProvider (ILoggerProvider provider) =>
			SelfLog.WriteLine("Ignoring added logger provider {0}", provider);
	}
}
