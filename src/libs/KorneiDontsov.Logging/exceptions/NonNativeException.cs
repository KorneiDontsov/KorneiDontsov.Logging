// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using System;

	sealed class NonNativeException: Exception {
		readonly Object exceptionObject;

		public NonNativeException (Object exceptionObject) =>
			this.exceptionObject = exceptionObject;

		/// <inheritdoc />
		public override String ToString () => exceptionObject.ToString();
	}
}
