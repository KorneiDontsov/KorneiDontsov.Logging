// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Core;

	static class LoggerFunctions {
		public static Logger? MayGetFastImpl (this ILogger logger) =>
			logger switch {
				Logger l => l,
				ConfiguredLogger l => l.impl,
				_ => null
			};
	}
}
