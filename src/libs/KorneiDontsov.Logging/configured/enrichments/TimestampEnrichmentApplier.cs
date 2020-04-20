// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Serilog.Configuration;
	using System;

	class TimestampEnrichmentApplier: ILoggingEnrichmentApplier {
		/// <inheritdoc />
		public String enrichmentName =>
			"timestamp";

		/// <inheritdoc />
		public void Apply (LoggerEnrichmentConfiguration enrich, IConfigurationSection conf) {
			if(conf.Value is {} timestampFormat) {
				try {
					_ = DateTimeOffset.Now.ToString(timestampFormat);
				}
				catch(FormatException ex) {
					throw new LoggingConfigurationException($"Value '{conf.Path}' has invalid format.", ex);
				}

				enrich.With(new TimestampEnricher(timestampFormat));
			}
		}
	}
}
