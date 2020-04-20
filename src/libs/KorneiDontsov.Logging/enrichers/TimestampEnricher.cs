// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog.Core;
	using Serilog.Events;
	using System;

	class TimestampEnricher: ILogEventEnricher {
		readonly String format;

		public TimestampEnricher (String format) =>
			this.format = format;

		/// <inheritdoc />
		public void Enrich (LogEvent logEvent, ILogEventPropertyFactory propertyFactory) =>
			logEvent.AddPropertyIfAbsent(
				new LogEventProperty("Timestamp", new ScalarValue(logEvent.Timestamp.ToString(format))));
	}
}
