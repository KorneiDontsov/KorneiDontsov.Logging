// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Serilog.Configuration;
	using System;

	public sealed class ThreadEnrichmentApplier: ILoggingEnrichmentApplier {
		/// <inheritdoc />
		public String enrichmentName =>
			"thread";

		/// <inheritdoc />
		public void Apply (LoggerEnrichmentConfiguration enrich, IConfigurationSection conf) {
			switch(conf.Value?.ToLowerInvariant()) {
				case "true":
					enrich.With<ThreadIdEnricher>();
					enrich.With<ThreadNameEnricher>();
					break;

				case "false":
				case null:
					break;

				default:
					var msg = $"Expected value '{conf.Path}' to be true or false, but accepted '{conf.Value}'.";
					throw new LoggingConfigurationException(msg);
			}
		}
	}
}
