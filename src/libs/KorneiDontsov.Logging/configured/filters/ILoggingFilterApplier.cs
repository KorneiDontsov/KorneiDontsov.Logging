// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Serilog.Configuration;
	using System;

	public interface ILoggingFilterApplier {
		String filterName { get; }

		/// <exception cref = "LoggingConfigurationException" />
		void Apply (LoggerFilterConfiguration filter, IConfigurationSection conf);
	}
}
