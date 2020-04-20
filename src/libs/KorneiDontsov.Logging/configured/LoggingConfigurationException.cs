// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using System;

	public sealed class LoggingConfigurationException: Exception {
		public LoggingConfigurationException (String message, Exception? innerException = null):
			base(message, innerException) { }
	}
}
