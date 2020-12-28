// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog.Core;
	using Serilog.Events;
	using System;

	public sealed class ThreadIdEnricher: ILogEventEnricher {
		/// <inheritdoc />
		public void Enrich (LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
			var threadId = Environment.CurrentManagedThreadId;
			logEvent.AddPropertyIfAbsent(new("ThreadId", new ScalarValue(threadId)));
		}
	}
}
