// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.DependencyInjection;
	using Serilog;

	public static class AotLoggerFunctions {
		public static AotLogger Aot (this ILogger logger) => new(logger);

		public static IServiceCollection AddAotLogger (this IServiceCollection services) =>
			services.AddSingleton<AotLogger?>(
				provider => provider.GetService<ILogger?>() switch {
					null => null,
					{ } logger => new(logger, disposable: false)
				});
	}
}
