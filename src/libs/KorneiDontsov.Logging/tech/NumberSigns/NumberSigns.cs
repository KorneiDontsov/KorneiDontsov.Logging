// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using System;

	[Flags]
	enum NumberSigns {
		Zero = 1,
		Positive = 1 << 1,
		Negative = 1 << 2,

		All = Zero | Positive | Negative
	}
}
