// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using System;

	static class NumberSignsFunctions {
		public static Boolean MatchSign (this Int32 number, NumberSigns signs) =>
			number switch {
				< 0 => (signs & NumberSigns.Negative) is not 0,
				0 => (signs & NumberSigns.Zero) is not 0,
				> 0 => (signs & NumberSigns.Positive) is not 0
			};

		public static Boolean MatchSign (this Int64 number, NumberSigns signs) =>
			number switch {
				< 0 => (signs & NumberSigns.Negative) is not 0,
				0 => (signs & NumberSigns.Zero) is not 0,
				> 0 => (signs & NumberSigns.Positive) is not 0
			};
	}
}
