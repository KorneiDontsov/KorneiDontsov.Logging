// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using System;
	using System.IO;
	using System.Reflection;
	using System.Threading.Tasks;
	using static System.String;
	using static System.Threading.Interlocked;

	public static class CrashLogger {
		static volatile String criticalLogRootPath = "logs/critical";

		static volatile Object? previousRaiseTimestamp;

		static void OnCrash (Exception exception, String logTypeName) {
			var raiseTimestamp = DateTimeOffset.Now;
			var previousRaiseTimestamp = Exchange(ref CrashLogger.previousRaiseTimestamp, raiseTimestamp);

			var sharedLogger = Log.Logger;
			Logger? criticalCrashLogger;
			ILogger logger;
			if(sharedLogger.IsEnabled(LogEventLevel.Fatal)) {
				criticalCrashLogger = null;
				logger = sharedLogger;
			}
			else {
				var appName = Assembly.GetEntryAssembly().GetName().Name;
				var logFileName = $"{logTypeName}_{appName}_{raiseTimestamp.UtcDateTime:yyyy-MM-ddThhmmss.fff}.log";
				var logFilePath = Path.Combine(criticalLogRootPath, logFileName);
				logger = criticalCrashLogger =
					new LoggerConfiguration()
						.WriteTo.File(logFilePath)
						.CreateLogger();
			}

			try {
				var log = "Unhandled exception raised at {RaiseTimestamp}.";
				logger.Fatal(exception, log, new Object?[] { raiseTimestamp });

				if(criticalCrashLogger is {} && previousRaiseTimestamp is {}) {
					var prevRaiseLog = "Previous raise was at {PreviousRaiseTimestamp}.";
					criticalCrashLogger.Information(prevRaiseLog, previousRaiseTimestamp);
				}
			}
			finally {
				Log.CloseAndFlush();
				(sharedLogger as IDisposable)?.Dispose();
				criticalCrashLogger?.Dispose();
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
					OnCrash(
						args.ExceptionObject switch {
							Exception exception => exception,
							var exceptionObject => new NonNativeException(exceptionObject)
						},
						"UnhandledException");
			TaskScheduler.UnobservedTaskException +=
				(sender, args) => OnCrash(args.Exception, "UnobservedTaskException");
		}

		public static void Activate () { }

		public static void Activate (String criticalLogRootPath) {
			if(IsNullOrEmpty(criticalLogRootPath))
				throw new ArgumentException("Critical log root path is null or empty.", nameof(criticalLogRootPath));
			else if(criticalLogRootPath.IndexOfAny(Path.GetInvalidPathChars()) is var invalidCharIndex
			        && invalidCharIndex >= 0) {
				var msg =
					$"Critical log root path '{criticalLogRootPath}' contains invalid character "
					+ $"'{criticalLogRootPath[invalidCharIndex]}' at {invalidCharIndex}.";
				throw new ArgumentException(msg, nameof(criticalLogRootPath));
			}
			else
				CrashLogger.criticalLogRootPath = criticalLogRootPath;
		}
	}
}
