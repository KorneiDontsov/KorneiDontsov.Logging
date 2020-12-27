// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;

	/// <summary>
	///     Specifies when to write crash log. If crash log is not written, log is still written to global
	///     <see cref = "Log.Logger" />.
	/// </summary>
	public enum CrashLogWriteCondition {
		Never,
		Always,

		/// <summary>
		///     Write crash log only if global <see cref = "Log.Logger" /> doesn't accept the log.
		///     <para />
		/// </summary>
		IfGlobalLoggerMissed
	}
}
