// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Primitives;
	using Serilog.Events;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public sealed class LoggingProfileConfiguration: IConfigurationSection {
		IConfigurationSection profileConf { get; }
		public LogEventLevel minLevel { get; }
		public String profileTypeName { get; }

		/// <exception cref = "LoggingConfigurationException" />
		public LoggingProfileConfiguration (IConfigurationSection profileConf) {
			this.profileConf = profileConf;

			if(profileConf["minLevel"] is {} minLevelStr) {
				if(Enum.TryParse(minLevelStr, ignoreCase: true, out LogEventLevel parsedMinLevel))
					minLevel = parsedMinLevel;
				else {
					var msg = $"'{profileConf.Path}:minLevel' has invalid value '{minLevelStr}'.";
					throw new LoggingConfigurationException(msg);
				}
			}
			else
				minLevel = LogEventLevel.Verbose;

			profileTypeName =
				profileConf["type"]?.ToLowerInvariant()
				?? throw new LoggingConfigurationException($"Missed '{profileConf.Path}:type'.");
		}

		/// <inheritdoc />
		public String Key =>
			profileConf.Key;

		/// <inheritdoc />
		public String Path =>
			profileConf.Path;

		/// <inheritdoc />
		public String Value {
			get => profileConf.Value;
			set => profileConf.Value = value;
		}

		/// <inheritdoc />
		public String this [String key] {
			get => profileConf[key];
			set => profileConf[key] = value;
		}

		/// <inheritdoc />
		public IEnumerable<IConfigurationSection> GetChildren () =>
			profileConf.GetChildren();

		/// <inheritdoc />
		public IConfigurationSection GetSection (String key) =>
			profileConf.GetSection(key);

		/// <inheritdoc />
		public IChangeToken GetReloadToken () =>
			profileConf.GetReloadToken();

		public Boolean GetSyncValue () =>
			profileConf["sync"]?.ToLowerInvariant() switch {
				"true" => true,
				"false" => false,
				null => false,

				{} value =>
				throw
					new LoggingConfigurationException(
						$"Expected '{profileConf.Path}:sync' to be boolean, but accepted '{value}'.")
			};

		public String GetOutputTemplate () {
			var outputConf = profileConf.GetSection("output");
			if(! outputConf.Exists())
				throw new LoggingConfigurationException($"Missed '{profileConf}.output'.");
			else if(outputConf.GetChildren().ToList() is var outputArrItems && outputArrItems.Count is 0)
				throw new LoggingConfigurationException($"'{profileConf}:output' is not a string array.");
			else {
				var sb = new StringBuilder(256);
				foreach(var outputArrItem in outputArrItems)
					sb.Append(outputArrItem.Value).Append("{NewLine}");
				return sb.ToString();
			}
		}
	}
}
