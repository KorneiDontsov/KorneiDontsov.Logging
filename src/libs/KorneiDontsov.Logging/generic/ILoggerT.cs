// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;

	// ReSharper disable once UnusedTypeParameter
	public interface ILogger<TSource>: ILogger {
		new ILogger<TOtherSource> ForContext<TOtherSource> ();
	}
}
