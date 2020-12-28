// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Serilog;
	using Serilog.Configuration;
	using System;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text.RegularExpressions;

	public sealed class LogFileProfileApplier: ILoggingProfileApplier {
		public LoggingAppEnvironment environment { get; }

		public LogFileProfileApplier (LoggingAppEnvironment environment) =>
			this.environment = environment;

		/// <inheritdoc />
		public String profileTypeName => "file";

		/// <inheritdoc />
		public void Apply (LoggerSinkConfiguration writeTo, LoggingProfileConfiguration conf) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static void ThrowFilePathTemplateIsEmpty (IConfigurationSection conf) =>
				throw new LoggingConfigurationException($"'{conf.Path}:path' is empty.");

			[MethodImpl(MethodImplOptions.NoInlining)]
			static void ThrowFilePathIsNotValid
				(IConfigurationSection conf, String filePathTemplate, String filePath, Int32 invalidCharIndex) {
				var msg =
					$"File path '{filePath}' based on template '{filePathTemplate}' from '{conf.Path}:path' contains "
					+ $"invalid character '{filePath[invalidCharIndex]}' at {invalidCharIndex}";
				throw new LoggingConfigurationException(msg);
			}

			var isSync = conf.GetSyncValue();
			var outputTemplate = conf.GetOutputTemplate();

			var filePathTemplate = conf.ReadString("path");
			if(String.IsNullOrWhiteSpace(filePathTemplate))
				ThrowFilePathTemplateIsEmpty(conf);

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
			if(filePath.IndexOfAny(Path.GetInvalidPathChars()) is >= 0 and var invalidCharIndex)
				ThrowFilePathIsNotValid(conf, filePathTemplate, filePath, invalidCharIndex);

			var fileFullPath =
				Path.IsPathRooted(filePath) switch {
					true => filePath,
					false => Path.Combine(environment.contentRootPath, filePath)
				};

			var maxFileSize = conf.ReadInt64("maxSize", NumberSigns.Positive, defaultValue: 1L * 1024 * 1024 * 1024);
			var retainedFileCountLimit = conf.ReadInt32IfExists("retainedFileCountLimit", NumberSigns.Positive);

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
