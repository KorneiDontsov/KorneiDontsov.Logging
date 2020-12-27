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
			minLevel = profileConf.ReadEnum<LogEventLevel>("minLevel") ?? LogEventLevel.Verbose;
			profileTypeName = profileConf.ReadString("type").ToLowerInvariant();
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
			profileConf.ReadBoolean("sync");

		public String GetOutputTemplate () {
			if(profileConf.GetSection("output").GetChildren().ToList() is not { Count: > 0 } outputArrItems)
				throw new LoggingConfigurationException($"'Missed {profileConf}:output'.");
			else {
				var sb = new StringBuilder(256);
				foreach(var outputArrItem in outputArrItems)
					sb.Append(outputArrItem.Value).Append("{NewLine}");
				return sb.ToString();
			}
		}
	}
}
