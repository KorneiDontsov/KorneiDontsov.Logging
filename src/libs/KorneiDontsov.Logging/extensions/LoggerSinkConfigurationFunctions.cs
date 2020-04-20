// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Configuration;
	using System;

	public static class LoggerSinkConfigurationFunctions {
		public static void SyncOrAsync
			(this LoggerSinkConfiguration sinkConfiguration,
			 Boolean sync,
			 Action<LoggerSinkConfiguration> configure) {
			if(sync)
				configure(sinkConfiguration);
			else
				sinkConfiguration.Async(configure);
		}
	}
}
