// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Serilog.Configuration;
	using System;

	public interface ILoggingEnrichmentApplier {
		String enrichmentName { get; }

		/// <exception cref = "LoggingConfigurationException" />
		void Apply (LoggerEnrichmentConfiguration enrich, IConfigurationSection conf);
	}
}
