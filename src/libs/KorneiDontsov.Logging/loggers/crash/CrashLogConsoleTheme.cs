// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog.Sinks.SystemConsole.Themes;
	using System;
	using System.IO;

	sealed class CrashLogConsoleTheme: ConsoleTheme {
		public static CrashLogConsoleTheme shared { get; } = new();

		public override Boolean CanBuffer => false;

		protected override Int32 ResetCharCount => 0;

		public override Int32 Set (TextWriter output, ConsoleThemeStyle style) {
			Console.ForegroundColor = ConsoleColor.DarkRed;
			return 0;
		}

		public override void Reset (TextWriter output) =>
			Console.ResetColor();
	}
}
