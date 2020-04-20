// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog.Configuration;
	using System;

	public interface ILoggingProfileApplier {
		String profileTypeName { get; }

		void Apply (LoggerSinkConfiguration writeTo, LoggingProfileConfiguration conf);
	}
}
