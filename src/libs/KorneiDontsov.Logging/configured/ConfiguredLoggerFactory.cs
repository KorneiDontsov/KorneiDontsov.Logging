// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Serilog;
	using Serilog.Core;
	using Serilog.Extensions.Logging;
	using System;
	using System.Collections.Generic;
	using static ConfiguredLoggingFunctions;

	class ConfiguredLoggerFactory: SerilogLoggerFactory, IDisposable {
		public Logger logger { get; }

		ConfiguredLoggerFactory (Logger logger):
			base(logger) =>
			this.logger = logger;

		public ConfiguredLoggerFactory
			(IConfiguration configuration,
			 IEnumerable<ILoggingProfileApplier> profileAppliers,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers):
			this(
				CreateSharedConfiguredLogger(
					configuration.GetSection("logging"),
					profileAppliers,
					enrichmentAppliers)) { }

		void IDisposable.Dispose () {
			base.Dispose();
			Log.CloseAndFlush();
			logger.Dispose();
		}
	}
}
