// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging.Example {
	using Microsoft.Extensions.Hosting;
	using Serilog;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	class ServiceExample: IHostedService {
		IHostApplicationLifetime appLifetime { get; }
		ILogger logger { get; }

		public ServiceExample (IHostApplicationLifetime appLifetime, ILogger logger) {
			this.appLifetime = appLifetime;
			this.logger = logger.ForContext<ServiceExample>();
		}

		async void BeginWork () {
			logger.Information("An error will be thrown after 3 seconds.");
			await Task.Delay(3_000);
			try {
				throw new Exception("Exception example.");
			}
			catch(Exception ex) {
				logger.Error(ex, "Exception example was thrown.");
			}
			finally { appLifetime.StopApplication(); }
		}

		/// <inheritdoc />
		public async Task StartAsync (CancellationToken cancellationToken) {
			logger.Information("Application will start after 2 seconds.");
			await Task.Delay(2_000, cancellationToken);
			BeginWork();
		}

		/// <inheritdoc />
		public Task StopAsync (CancellationToken cancellationToken) =>
			Task.CompletedTask;
	}
}
