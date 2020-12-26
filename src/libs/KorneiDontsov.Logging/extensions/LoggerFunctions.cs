// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;

	public static class LoggerFunctions {
		public static AotLogger Aot (this ILogger logger) => new(logger);
	}
}
