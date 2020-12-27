// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Runtime.CompilerServices;

	static class LoggingConfigurationFunctions {
		/// <exception cref = "LoggingConfigurationException" />
		public static String ReadString (this IConfigurationSection conf, String propName) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException NotFound (IConfigurationSection conf, String propName) =>
				new($"Missed '{conf.Path}:{propName}'.");

			return conf[propName] ?? throw NotFound(conf, propName);
		}

		/// <exception cref = "LoggingConfigurationException" />
		public static Boolean ReadBoolean (this IConfigurationSection conf, String propName) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException NotValid (IConfigurationSection conf, String propName, String value) =>
				new($"Expected '{conf.Path}:{propName}' to be boolean, but accepted '{value}'.");

			return conf[propName]?.ToLowerInvariant() switch {
				"true" => true,
				null or "false" => false,
				{ } value => throw NotValid(conf, propName, value)
			};
		}

		/// <exception cref = "LoggingConfigurationException" />
		public static T? ReadEnum<T> (this IConfigurationSection conf, String propName) where T: struct, Enum {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException NotValid (IConfigurationSection conf, String propName, String value) =>
				new($"'{conf.Path}':{propName} has invalid value '{value}'.");

			return conf[propName] switch {
				null => null,
				{ } value when Enum.TryParse(value, ignoreCase: true, out T parsedValue) => parsedValue,
				{ } value => throw NotValid(conf, propName, value)
			};
		}
	}
}
