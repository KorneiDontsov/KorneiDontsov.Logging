// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

using KorneiDontsov.Logging;
using KorneiDontsov.Logging.Example;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CrashLogger.Activate();
Host.CreateDefaultBuilder(args)
	.UseConfiguredLogger(
		conf => conf.Enrich.WithProperty("EnrichedByCode", true))
	.UseGenericLogger()
	.ConfigureServices(services => services.AddHostedService<ServiceExample>())
	.Build()
	.Run();
