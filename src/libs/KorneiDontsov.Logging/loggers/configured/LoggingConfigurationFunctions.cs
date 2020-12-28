// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Runtime.CompilerServices;

	static class LoggingConfigurationFunctions {
		[MethodImpl(MethodImplOptions.NoInlining)]
		static LoggingConfigurationException NotFound (IConfigurationSection conf) =>
			new($"Missed '{conf.Path}'.");

		[MethodImpl(MethodImplOptions.NoInlining)]
		static LoggingConfigurationException NotFound (IConfigurationSection conf, String propName) =>
			new($"Missed '{conf.Path}:{propName}'.");

		[MethodImpl(MethodImplOptions.NoInlining)]
		static LoggingConfigurationException NotValid (IConfigurationSection conf, String value) =>
			new($"'{conf.Path}' has invalid value '{value}'.");

		[MethodImpl(MethodImplOptions.NoInlining)]
		static LoggingConfigurationException NotValid (IConfigurationSection conf, String propName, String value) =>
			new($"'{conf.Path}':{propName} has invalid value '{value}'.");

		[MethodImpl(MethodImplOptions.NoInlining)]
		static LoggingConfigurationException NotNumber (IConfigurationSection conf, String propName, String value) =>
			new($"Expected '{conf.Path}:{propName}' to be a number but accepted '{value}'.");

		[MethodImpl(MethodImplOptions.NoInlining)]
		static LoggingConfigurationException OutOfRange (IConfigurationSection conf, String propName, String value) =>
			new($"'{conf.Path}:{propName}' = '{value}' is out of range.");

		static LoggingConfigurationException NotMatchSign
			(IConfigurationSection conf, String propName, String value, NumberSigns signs) =>
			new($"'{conf.Path}:{propName}' = '{value}' is not {signs}.");

		/// <exception cref = "LoggingConfigurationException" />
		public static String ReadString (this IConfigurationSection conf, String propName) =>
			conf[propName] ?? throw NotFound(conf, propName);

		/// <exception cref = "LoggingConfigurationException" />
		public static Boolean ReadBoolean (this IConfigurationSection conf, Boolean? defaultValue = null) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException NotBoolean (IConfigurationSection conf, String value) =>
				new($"Expected '{conf.Path}' to be boolean, but accepted '{value}'.");

			return conf.Value?.ToLowerInvariant() switch {
				"true" => true,
				"false" => false,
				null => defaultValue ?? throw NotFound(conf),
				{ } value => throw NotBoolean(conf, value)
			};
		}

		/// <exception cref = "LoggingConfigurationException" />
		public static Boolean ReadBoolean
			(this IConfigurationSection conf,
			 String propName,
			 Boolean? defaultValue = null) {
			[MethodImpl(MethodImplOptions.NoInlining)]
			static LoggingConfigurationException NotBoolean
				(IConfigurationSection conf, String propName, String value) =>
				new($"Expected '{conf.Path}:{propName}' to be boolean, but accepted '{value}'.");

			return conf.Value?.ToLowerInvariant() switch {
				"true" => true,
				"false" => false,
				null => defaultValue ?? throw NotFound(conf, propName),
				{ } value => throw NotBoolean(conf, propName, value)
			};
		}

		public static Int32? ReadInt32IfExists
			(this IConfigurationSection conf,
			 String propName,
			 NumberSigns signs = NumberSigns.All) {
			if(conf[propName] is not { } value)
				return null;
			else {
				Int32 number;
				try {
					number = Int32.Parse(value);
				}
				catch(FormatException) {
					throw NotNumber(conf, propName, value);
				}
				catch(OverflowException) {
					throw OutOfRange(conf, propName, value);
				}

				return number.MatchSign(signs) switch {
					true => number,
					false => throw NotMatchSign(conf, propName, value, signs)
				};
			}
		}

		public static Int64 ReadInt64
			(this IConfigurationSection conf,
			 String propName,
			 NumberSigns signs = NumberSigns.All,
			 Int64? defaultValue = null) {
			if(conf[propName] is not { } value)
				return defaultValue ?? throw NotFound(conf, propName);
			else {
				Int64 number;
				try {
					number = Int64.Parse(value);
				}
				catch(FormatException) {
					throw NotNumber(conf, propName, value);
				}
				catch(OverflowException) {
					throw OutOfRange(conf, propName, value);
				}

				return number.MatchSign(signs) switch {
					true => number,
					false => throw NotMatchSign(conf, propName, value, signs)
				};
			}
		}

		/// <exception cref = "LoggingConfigurationException" />
		public static T ReadEnum<T> (this IConfigurationSection conf, T? defaultValue = null)
			where T: struct, Enum =>
			conf.Value switch {
				{ } value when Enum.TryParse(value, ignoreCase: true, out T parsedValue) => parsedValue,
				null => defaultValue ?? throw NotFound(conf),
				{ } value => throw NotValid(conf, value)
			};

		/// <exception cref = "LoggingConfigurationException" />
		public static T ReadEnum<T> (this IConfigurationSection conf, String propName, T? defaultValue = null)
			where T: struct, Enum =>
			conf[propName] switch {
				{ } value when Enum.TryParse(value, ignoreCase: true, out T parsedValue) => parsedValue,
				null => defaultValue ?? throw NotFound(conf, propName),
				{ } value => throw NotValid(conf, propName, value)
			};
	}
}
