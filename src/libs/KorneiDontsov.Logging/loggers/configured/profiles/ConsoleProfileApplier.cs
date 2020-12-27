// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Configuration;
	using System;

	public sealed class ConsoleProfileApplier: ILoggingProfileApplier {
		/// <inheritdoc />
		public String profileTypeName => "console";

		/// <inheritdoc />
		public void Apply (LoggerSinkConfiguration writeTo, LoggingProfileConfiguration conf) {
			var isSync = conf.GetSyncValue();
			var outputTemplate = conf.GetOutputTemplate();
			writeTo.SyncOrAsync(isSync, sink => sink.Console(conf.minLevel, outputTemplate));
		}
	}
}
