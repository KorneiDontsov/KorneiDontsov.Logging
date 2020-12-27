// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging.Example {
	using Microsoft.Extensions.Hosting;
	using Serilog;
	using Serilog.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	class ServiceExample: IHostedService {
		readonly ILogger logger;

		public ServiceExample (ILogger<ServiceExample> logger) =>
			this.logger = logger;

		async void BeginWork () {
			logger.Information("An error will be thrown after 3 seconds.");
			await Task.Delay(1_500);
			try {
				throw new("Exception example.");
			}
			catch(Exception ex) {
				using(LogContext.PushProperty("HasLogContext", true))
					logger.Error(ex, "Exception example was thrown.");
			}
			finally {
				throw new("Crash example.");
			}
		}

		/// <inheritdoc />
		public async Task StartAsync (CancellationToken cancellationToken) {
			logger.Information("Application will start after 2 seconds.");
			await Task.Delay(1_500, cancellationToken);
			BeginWork();
		}

		/// <inheritdoc />
		public Task StopAsync (CancellationToken cancellationToken) =>
			Task.CompletedTask;
	}
}
