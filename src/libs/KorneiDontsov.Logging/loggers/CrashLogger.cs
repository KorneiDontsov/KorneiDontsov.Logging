// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using Serilog.Sinks.SystemConsole.Themes;
	using System;
	using System.Globalization;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;
	using static System.Threading.Interlocked;

	public static class CrashLogger {
		sealed class CrashLogConsoleTheme: ConsoleTheme {
			public static CrashLogConsoleTheme shared { get; } = new();

			public override Boolean CanBuffer => false;

			protected override Int32 ResetCharCount => 0;

			/// <inheritdoc />
			public override Int32 Set (TextWriter output, ConsoleThemeStyle style) {
				Console.ForegroundColor = ConsoleColor.DarkRed;
				return 0;
			}

			/// <inheritdoc />
			public override void Reset (TextWriter output) =>
				Console.ResetColor();
		}

		static class Worker {
			static readonly AppDomain appDomain;

			public static CrashLoggerOptions options;

			static String? logFileNameTemplate;

			static Object? previousRaiseTimestamp;

			static String EncodeToUseInPath (String name) {
				var invalidChars = Path.GetInvalidPathChars();
				Array.Resize(ref invalidChars, invalidChars.Length + 2);
				invalidChars[invalidChars.Length - 2] = Path.PathSeparator;
				invalidChars[invalidChars.Length - 1] = Path.AltDirectorySeparatorChar;

				var encodedName = name.ToCharArray().AsSpan();
				foreach(ref var currentChar in encodedName)
					foreach(var invalidChar in invalidChars)
						if(currentChar == invalidChar) {
							currentChar = '-';
							break;
						}

				return encodedName.ToString();
			}

			static Logger? CreateCrashLoggerIfRequired
				(String logName, in DateTimeOffset raisedAt, Boolean missedGlobalLogger) {
				var options = Worker.options;
				var writeToConsole =
					options.writeToConsole switch {
						CrashLogWriteCondition.Always => true,
						CrashLogWriteCondition.IfGlobalLoggerMissed => missedGlobalLogger,
						CrashLogWriteCondition.Never => false
					};
				var writeToFile =
					options.writeToFile switch {
						CrashLogWriteCondition.Always => true,
						CrashLogWriteCondition.IfGlobalLoggerMissed => missedGlobalLogger,
						CrashLogWriteCondition.Never => false
					};
				if(! (writeToConsole & writeToFile))
					return null;
				else {
					const String outputTemplate =
						"[[CRASH]] {Message:lj}{NewLine}"
						+ "{Exception}{NewLine}";

					var loggerConf = new LoggerConfiguration();

					if(writeToConsole)
						loggerConf.WriteTo.Console(
							outputTemplate: outputTemplate,
							theme: CrashLogConsoleTheme.shared);

					if(writeToFile) {
						logFileNameTemplate ??= EncodeToUseInPath(appDomain.FriendlyName) + "_{0}_{1}.log";
						var fileName =
							String.Format(
								CultureInfo.InvariantCulture,
								logFileNameTemplate,
								logName,
								raisedAt.UtcDateTime.ToString("yyyy-MM-ddThhmmss.fff"));
						loggerConf.WriteTo.File(
							Path.Combine(options.crashLogRootPath!, fileName),
							outputTemplate: outputTemplate);
					}

					return loggerConf.CreateLogger();
				}
			}

			static void OnCrash (String logName, Exception exception) {
				var raiseTimestamp = DateTimeOffset.Now;
				var raiseTimestampObj = (Object) raiseTimestamp;
				var previousRaiseTimestampObj = Exchange(ref previousRaiseTimestamp, raiseTimestampObj);

				const String firstLog = "Unhandled exception raised at {RaiseTimestamp}.";
				const String nextLog = firstLog + " Previous raise was at {PreviousRaiseTimestamp}.";
				var (log, props) =
					previousRaiseTimestampObj switch {
						null => (firstLog, new[] { raiseTimestampObj }),
						{ } => (nextLog, new[] { raiseTimestampObj, previousRaiseTimestampObj })
					};

				Log.Fatal(exception, log, props);
				var missedGlobalLogger = ! Log.IsEnabled(LogEventLevel.Fatal);
				if(CreateCrashLoggerIfRequired(logName, raiseTimestamp, missedGlobalLogger) is { } crashLogger)
					crashLogger.Fatal(exception, log, props);
			}

			static Worker () {
				appDomain = AppDomain.CurrentDomain;
				options = CrashLoggerOptions.Version2();

				appDomain.UnhandledException +=
					(_, args) =>
						OnCrash(
							nameof(AppDomain.UnhandledException),
							args.ExceptionObject switch {
								Exception exception => exception,
								var exceptionObject => new NonNativeException(exceptionObject)
							});
				TaskScheduler.UnobservedTaskException +=
					(_, args) => OnCrash(nameof(TaskScheduler.UnobservedTaskException), args.Exception);
				appDomain.ProcessExit +=
					(_, _) => Log.CloseAndFlush();
			}

			public static void Awake () { }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void ValidatePath (String? path, String argName) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static void ThrowPathIsNullOrEmpty (String argName) =>
				throw new ArgumentException("Path is null or empty.", argName);

			[MethodImpl(MethodImplOptions.NoInlining)]
			static void ThrowPathContainsInvalidChar (String path, String argName, Int32 invalidCharIndex) =>
				throw new ArgumentException(
					$"Path '{path}' contains invalid character '{path[invalidCharIndex]}' at {invalidCharIndex}.",
					argName);

			if(String.IsNullOrEmpty(path))
				ThrowPathIsNullOrEmpty(nameof(path));
			else if(path!.IndexOfAny(Path.GetInvalidPathChars()) is >= 0 and var invalidCharIndex)
				ThrowPathContainsInvalidChar(path, nameof(path), invalidCharIndex);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void ValidateWriteCondition (CrashLogWriteCondition writeCondition, String argName) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static void ThrowWriteConditionIsNotValid (CrashLogWriteCondition writeCondition, String argName) =>
				throw new ArgumentException($"Crash log write condition {writeCondition} is not valid.", argName);

			var notValid =
				writeCondition switch {
					CrashLogWriteCondition.Never => false,
					CrashLogWriteCondition.Always => false,
					CrashLogWriteCondition.IfGlobalLoggerMissed => false,
					_ => true
				};
			if(notValid) ThrowWriteConditionIsNotValid(writeCondition, argName);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Activate () => Worker.Awake();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Activate (String crashLogRootPath) {
			ValidatePath(crashLogRootPath, nameof(crashLogRootPath));
			Worker.options =
				new CrashLoggerOptions {
					writeToConsole = CrashLogWriteCondition.IfGlobalLoggerMissed,
					writeToFile = CrashLogWriteCondition.Always,
					crashLogRootPath = crashLogRootPath
				};
		}

		public static void Activate (CrashLoggerOptions options) {
			var theOptions = options with { };

			ValidateWriteCondition(theOptions.writeToConsole, nameof(options) + "." + nameof(options.writeToConsole));
			ValidateWriteCondition(theOptions.writeToFile, nameof(options) + "." + nameof(options.writeToFile));
			if(theOptions.writeToFile is not CrashLogWriteCondition.Never)
				ValidatePath(theOptions.crashLogRootPath, nameof(options) + "." + nameof(options.crashLogRootPath));

			Worker.options = theOptions;
		}
	}
}
