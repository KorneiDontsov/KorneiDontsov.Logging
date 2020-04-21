// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Events;
	using System;
	using System.Reflection;
	using System.Threading.Tasks;

	public static class CrashLogger {
		static void OnUnhandledException (Exception exception) {
			var timestamp = DateTimeOffset.Now;
			var sharedLogger = Log.Logger;
			var logger =
				sharedLogger.IsEnabled(LogEventLevel.Fatal)
					? sharedLogger
					: new LoggerConfiguration()
						.WriteTo.File(
							$"CRITICAL/CRASH_{Assembly.GetEntryAssembly().GetName().Name}_{timestamp.UtcDateTime:yyyy-MM-ddThhmmss.fff}.log")
						.CreateLogger();
			try {
				logger.Fatal(exception, "Unhandled exception raised at {RaiseTimestamp}.", new Object?[] { timestamp });
			}
			finally {
				Log.CloseAndFlush();
				(logger as IDisposable)?.Dispose();
				(sharedLogger as IDisposable)?.Dispose();
			}
		}

		class NonNativeException: Exception {
			Object exceptionObject { get; }

			public NonNativeException (Object exceptionObject) =>
				this.exceptionObject = exceptionObject;

			/// <inheritdoc />
			public override String Message => exceptionObject.ToString();
		}

		static CrashLogger () {
			AppDomain.CurrentDomain.UnhandledException +=
				(sender, args) =>
					OnUnhandledException(
						args.ExceptionObject switch {
							Exception exception => exception,
							var exceptionObject => new NonNativeException(exceptionObject)
						});
			TaskScheduler.UnobservedTaskException +=
				(sender, args) => OnUnhandledException(args.Exception);
		}

		public static void Activate () { }
	}
}
