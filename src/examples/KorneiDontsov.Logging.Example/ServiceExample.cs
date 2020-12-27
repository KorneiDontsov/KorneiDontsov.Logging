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
		ILogger logger { get; }

		public ServiceExample (ILogger logger) =>
			this.logger = logger.ForContext<ServiceExample>();

		async void BeginWork () {
			logger.Information("An error will be thrown after 3 seconds.");
			await Task.Delay(1_500);
			try {
				throw new Exception("Exception example.");
			}
			catch(Exception ex) {
				using(LogContext.PushProperty("HasLogContext", true))
					logger.Error(ex, "Exception example was thrown.");
			}
			finally {
				throw new Exception("Crash example.");
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
