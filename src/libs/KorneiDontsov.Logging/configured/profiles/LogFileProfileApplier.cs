// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Configuration;
	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using static System.String;

	public sealed class LogFileProfileApplier: ILoggingProfileApplier {
		public LoggingAppEnvironment environment { get; }

		public LogFileProfileApplier (LoggingAppEnvironment environment) =>
			this.environment = environment;

		/// <inheritdoc />
		public String profileTypeName =>
			"file";

		/// <inheritdoc />
		public void Apply (LoggerSinkConfiguration writeTo, LoggingProfileConfiguration conf) {
			var isSync = conf.GetSyncValue();
			var outputTemplate = conf.GetOutputTemplate();

			var filePathTemplate =
				conf["path"] switch {
					null => throw new LoggingConfigurationException($"Missed '{conf.Path}:path'."),

					{ } value when IsNullOrWhiteSpace(value) =>
						throw new LoggingConfigurationException($"'{conf.Path}:path' is empty."),

					{ } value => value
				};
			var filePath =
				Regex.Replace(
					filePathTemplate,
					@"{(app|launchutc)}",
					match =>
						match.Groups[1].Value.ToLowerInvariant() switch {
							"app" => environment.appName,
							"launchutc" => environment.launchTimestamp.UtcDateTime.ToString("yyyy-MM-ddTHHmmss.fff")
						},
					RegexOptions.IgnoreCase);
			if(filePath.IndexOfAny(Path.GetInvalidPathChars()) is var invalidCharIndex
			   && invalidCharIndex >= 0) {
				var msg =
					$"File path '{filePath}' based on template '{filePathTemplate}' "
					+ $"from '{conf.Path}:path' contains invalid character "
					+ $"'{filePath[invalidCharIndex]}' at {invalidCharIndex}";
				throw new LoggingConfigurationException(msg);
			}
			else {
				var fileFullPath =
					Path.IsPathRooted(filePath)
						? filePath
						: Path.Combine(environment.contentRootPath, filePath);

				Int64 maxFileSize;
				if(conf["maxSize"] is { } maxFileSizeStr)
					try {
						maxFileSize = Int64.Parse(maxFileSizeStr);
						if(maxFileSize <= 0) {
							var msg = $"'{conf.Path}:maxFileSize' = '{maxFileSize}' is not positive.";
							throw new LoggingConfigurationException(msg);
						}
					}
					catch(FormatException ex) {
						var msg = $"'{conf.Path}:maxSize' = '{maxFileSizeStr}' is not a number.";
						throw new LoggingConfigurationException(msg, ex);
					}
					catch(OverflowException ex) {
						var msg = $"'{conf.Path}:maxSize' = '{maxFileSizeStr}' is out of range of 64-bit number.";
						throw new LoggingConfigurationException(msg, ex);
					}
				else
					maxFileSize = 1L * 1024 * 1024 * 1024;

				Int32? retainedFileCountLimit;
				if(conf["retainedFileCountLimit"] is { } retainedFileCountLimitStr)
					try {
						retainedFileCountLimit = Int32.Parse(retainedFileCountLimitStr);
						if(retainedFileCountLimit <= 0) {
							var msg =
								$"'{conf.Path}:retainedFileCountLimit' = '{retainedFileCountLimit}' is not positive.";
							throw new LoggingConfigurationException(msg);
						}
					}
					catch(FormatException ex) {
						var msg =
							$"'{conf.Path}:retainedFileCountLimit' = '{retainedFileCountLimitStr}' is not a number.";
						throw new LoggingConfigurationException(msg, ex);
					}
					catch(OverflowException ex) {
						var msg =
							$"'{conf.Path}:retainedFileCountLimit' = '{retainedFileCountLimitStr}'"
							+ " is out of range of 32-bit number.";
						throw new LoggingConfigurationException(msg, ex);
					}
				else
					retainedFileCountLimit = null;

				writeTo.SyncOrAsync(
					isSync,
					sink =>
						sink.File(
							fileFullPath,
							conf.minLevel,
							outputTemplate,
							fileSizeLimitBytes: maxFileSize,
							rollOnFileSizeLimit: true,
							retainedFileCountLimit: retainedFileCountLimit));
			}
		}
	}
}
