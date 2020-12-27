// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	public static class GenericLoggerFunctions {
		public static IServiceCollection AddGenericLogger (this IServiceCollection services) =>
			services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

		public static IHostBuilder UseGenericLogger (this IHostBuilder host) =>
			host.ConfigureServices((context, services) => services.AddGenericLogger());
	}
}
