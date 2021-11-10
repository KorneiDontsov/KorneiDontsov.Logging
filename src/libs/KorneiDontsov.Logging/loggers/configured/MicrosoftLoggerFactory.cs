// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Logging;
	using Serilog.Debugging;
	using Serilog.Extensions.Logging;
	using System;

	class MicrosoftLoggerFactory: ILoggerFactory {
		SerilogLoggerProvider provider { get; }

		public MicrosoftLoggerFactory (Serilog.ILogger logger) =>
			provider = new(logger, dispose: false);

		void IDisposable.Dispose () { }

		public ILogger CreateLogger (String categoryName) =>
			provider.CreateLogger(categoryName);

		public void AddProvider (ILoggerProvider provider) =>
			SelfLog.WriteLine("Ignoring added logger provider {0}", provider);
	}
}
