// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using System;
	using System.IO;

	public record CrashLoggerOptions {
		/// <summary>
		///     When to write crash log to console.
		/// </summary>
		public CrashLogWriteCondition writeToConsole { get; set; }

		/// <summary>
		/// </summary>
		public CrashLogWriteCondition writeToFile { get; set; }

		/// <summary>
		///     Path to directory where the crash log files should be written.
		///     <para />
		///     Must exists if <see cref = "writeToFile" /> is not
		///     <see cref = "CrashLogWriteCondition.Never" />.
		/// </summary>
		public String? crashLogRootPath { get; set; }

		/// <summary>
		///     Actual default options of <see cref = "CrashLogger" />.
		/// </summary>
		public static CrashLoggerOptions Version2 () =>
			new() {
				writeToConsole = CrashLogWriteCondition.IfGlobalLoggerMissed,
				writeToFile = CrashLogWriteCondition.Always,
				crashLogRootPath = Path.Combine(AppContext.BaseDirectory, "logs/crashes")
			};

		/// <summary>
		///     This options was default options of <see cref = "CrashLogger" /> in KorneiDontsov.Logging v1.*.
		/// </summary>
		public static CrashLoggerOptions Version1 () =>
			new() {
				writeToFile = CrashLogWriteCondition.IfGlobalLoggerMissed,
				crashLogRootPath = "logs/critical"
			};
	}
}
