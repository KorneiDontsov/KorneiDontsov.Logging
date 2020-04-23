// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog.Core;
	using Serilog.Events;
	using System;

	[Obsolete("Not supported. It cannot work since 'Timestamp' cannot be overriden.")]
	public sealed class TimestampEnricher: ILogEventEnricher {
		public TimestampEnricher (String format) { }

		/// <inheritdoc />
		public void Enrich (LogEvent logEvent, ILogEventPropertyFactory propertyFactory) { }
	}
}
