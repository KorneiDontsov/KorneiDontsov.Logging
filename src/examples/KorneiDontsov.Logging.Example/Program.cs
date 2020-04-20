// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging.Example {
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using System;

	class Program {
		static void Main (String[] args) {
			CrashLogger.Activate();
			Host.CreateDefaultBuilder(args)
				.UseConfiguredLogger()
				.ConfigureServices(services => services.AddHostedService<ServiceExample>())
				.Build()
				.Run();
		}
	}
}
