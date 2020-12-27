// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog.Core;
	using Serilog.Events;
	using System.Threading;

	public sealed class ThreadNameEnricher: ILogEventEnricher {
		/// <inheritdoc />
		public void Enrich (LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
			if(Thread.CurrentThread.Name is { } threadName)
				logEvent.AddPropertyIfAbsent(new LogEventProperty("ThreadName", new ScalarValue(threadName)));
		}
	}
}
