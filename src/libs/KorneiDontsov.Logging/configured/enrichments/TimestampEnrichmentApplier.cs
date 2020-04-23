// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Serilog.Configuration;
	using System;

	[Obsolete("Not supported. It cannot work since 'Timestamp' cannot be overriden.")]
	public sealed class TimestampEnrichmentApplier: ILoggingEnrichmentApplier {
		/// <inheritdoc />
		public String enrichmentName =>
			"timestamp";

		/// <inheritdoc />
		public void Apply (LoggerEnrichmentConfiguration enrich, IConfigurationSection conf) { }
	}
}
