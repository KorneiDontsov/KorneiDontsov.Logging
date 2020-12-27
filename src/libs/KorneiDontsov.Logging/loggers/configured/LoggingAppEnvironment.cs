// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using System;

	public sealed class LoggingAppEnvironment {
		public String appName { get; }
		public String contentRootPath { get; }
		public DateTimeOffset launchTimestamp { get; }

		public LoggingAppEnvironment (String appName, String contentRootPath, DateTimeOffset launchTimestamp) {
			this.appName = appName;
			this.contentRootPath = contentRootPath;
			this.launchTimestamp = launchTimestamp;
		}

		[ActivatorUtilitiesConstructor]
		public LoggingAppEnvironment (IHostEnvironment environment):
			this(environment.ApplicationName, environment.ContentRootPath, DateTimeOffset.Now) { }
	}
}
