// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.DependencyInjection;
	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using System;
	using System.Collections.Generic;

	public sealed class Logger<TSource>: ILogger<TSource> {
		readonly ILogger impl;
		readonly Logger? fastImpl;
		readonly IServiceProvider? serviceProvider;

		public Logger (ILogger logger, IServiceProvider? serviceProvider) {
			impl = logger.ForContext<TSource>();
			fastImpl = impl.MayGetFastImpl();
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public ILogger ForContext (ILogEventEnricher enricher) =>
			impl.ForContext(enricher);

		/// <inheritdoc />
		public ILogger ForContext (IEnumerable<ILogEventEnricher> enrichers) =>
			impl.ForContext(enrichers);

		/// <inheritdoc />
		public ILogger ForContext (String propertyName, Object? value, Boolean destructureObjects = false) =>
			impl.ForContext(propertyName, value, destructureObjects);

		/// <inheritdoc />
		public ILogger<TOtherSource> ForContext<TOtherSource> () =>
			serviceProvider?.GetService<ILogger<TOtherSource>>()
			?? new Logger<TOtherSource>(impl.ForContext(typeof(TOtherSource)), serviceProvider);

		/// <inheritdoc />
		ILogger ILogger.ForContext<TOtherSource> () =>
			impl.ForContext(typeof(TOtherSource));

		/// <inheritdoc />
		public ILogger ForContext (Type source) =>
			impl.ForContext(source);

		/// <inheritdoc />
		public void Write (LogEvent logEvent) =>
			impl.Write(logEvent);

		/// <inheritdoc />
		public void Write (LogEventLevel level, String messageTemplate) =>
			impl.Write(level, messageTemplate);

		/// <inheritdoc />
		public void Write<T> (LogEventLevel level, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Write(level, messageTemplate, propertyValue);
			else
				impl.Write(level, messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Write<T0, T1> (LogEventLevel level, String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Write(level, messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Write(level, messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Write<T0, T1, T2>
			(LogEventLevel level,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Write (LogEventLevel level, String messageTemplate, params Object?[] propertyValues) =>
			impl.Write(level, messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Write (LogEventLevel level, Exception exception, String messageTemplate) =>
			impl.Write(level, exception, messageTemplate);

		/// <inheritdoc />
		public void Write<T> (LogEventLevel level, Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Write(level, exception, messageTemplate, propertyValue);
			else
				impl.Write(level, exception, messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Write<T0, T1>
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Write<T0, T1, T2>
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Write
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			impl.Write(level, exception, messageTemplate, propertyValues);

		/// <inheritdoc />
		public Boolean IsEnabled (LogEventLevel level) =>
			impl.IsEnabled(level);

		/// <inheritdoc />
		public void Verbose (String messageTemplate) =>
			impl.Verbose(messageTemplate);

		/// <inheritdoc />
		public void Verbose<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Verbose(messageTemplate, propertyValue);
			else
				impl.Verbose(messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Verbose<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Verbose(messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Verbose(messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Verbose<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Verbose (String messageTemplate, params Object?[] propertyValues) =>
			impl.Verbose(messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Verbose (Exception exception, String messageTemplate) =>
			impl.Verbose(exception, messageTemplate);

		/// <inheritdoc />
		public void Verbose<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Verbose(exception, messageTemplate, propertyValue);
			else
				impl.Verbose(exception, messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Verbose<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Verbose<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Verbose (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Verbose(exception, messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Debug (String messageTemplate) =>
			impl.Debug(messageTemplate);

		/// <inheritdoc />
		public void Debug<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Debug(messageTemplate, propertyValue);
			else
				impl.Debug(messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Debug<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Debug(messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Debug(messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Debug<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Debug (String messageTemplate, params Object?[] propertyValues) =>
			impl.Debug(messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Debug (Exception exception, String messageTemplate) =>
			impl.Debug(exception, messageTemplate);

		/// <inheritdoc />
		public void Debug<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Debug(exception, messageTemplate, propertyValue);
			else
				impl.Debug(exception, messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Debug<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Debug<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Debug (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Debug(exception, messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Information (String messageTemplate) =>
			impl.Information(messageTemplate);

		/// <inheritdoc />
		public void Information<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Information(messageTemplate, propertyValue);
			else
				impl.Information(messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Information<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Information(messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Information(messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Information<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Information (String messageTemplate, params Object?[] propertyValues) =>
			impl.Information(messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Information (Exception exception, String messageTemplate) =>
			impl.Information(exception, messageTemplate);

		/// <inheritdoc />
		public void Information<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Information(exception, messageTemplate, propertyValue);
			else
				impl.Information(exception, messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Information<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Information(exception, messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Information(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Information<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Information (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Information(exception, messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Warning (String messageTemplate) =>
			impl.Warning(messageTemplate);

		/// <inheritdoc />
		public void Warning<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Warning(messageTemplate, propertyValue);
			else
				impl.Warning(messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Warning<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Warning(messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Warning(messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Warning<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Warning (String messageTemplate, params Object?[] propertyValues) =>
			impl.Warning(messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Warning (Exception exception, String messageTemplate) =>
			impl.Warning(exception, messageTemplate);

		/// <inheritdoc />
		public void Warning<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Warning(exception, messageTemplate, propertyValue);
			else
				impl.Warning(exception, messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Warning<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Warning(exception, messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Warning(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Warning<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Warning (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Warning(exception, messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Error (String messageTemplate) =>
			impl.Error(messageTemplate);

		/// <inheritdoc />
		public void Error<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Error(messageTemplate, propertyValue);
			else
				impl.Error(messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Error<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Error(messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Error(messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Error<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Error (String messageTemplate, params Object?[] propertyValues) =>
			impl.Error(messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Error (Exception exception, String messageTemplate) =>
			impl.Error(exception, messageTemplate);

		/// <inheritdoc />
		public void Error<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Error(exception, messageTemplate, propertyValue);
			else
				impl.Error(exception, messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Error<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Error(exception, messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Error(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Error<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Error (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Error(exception, messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Fatal (String messageTemplate) =>
			impl.Fatal(messageTemplate);

		/// <inheritdoc />
		public void Fatal<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Fatal(messageTemplate, propertyValue);
			else
				impl.Fatal(messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Fatal<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Fatal(messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Fatal(messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Fatal<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Fatal (String messageTemplate, params Object?[] propertyValues) =>
			impl.Fatal(messageTemplate, propertyValues);

		/// <inheritdoc />
		public void Fatal (Exception exception, String messageTemplate) =>
			impl.Fatal(exception, messageTemplate);

		/// <inheritdoc />
		public void Fatal<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Fatal(exception, messageTemplate, propertyValue);
			else
				impl.Fatal(exception, messageTemplate, propertyValue);
		}

		/// <inheritdoc />
		public void Fatal<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
			else
				impl.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		/// <inheritdoc />
		public void Fatal<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else
				impl.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		/// <inheritdoc />
		public void Fatal (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Fatal(exception, messageTemplate, propertyValues);

		/// <inheritdoc />
		public Boolean BindMessageTemplate
			(String messageTemplate,
			 Object[] propertyValues,
			 out MessageTemplate parsedTemplate,
			 out IEnumerable<LogEventProperty> boundProperties) =>
			impl.BindMessageTemplate(
				messageTemplate,
				propertyValues,
				out parsedTemplate,
				out boundProperties);

		/// <inheritdoc />
		public Boolean BindProperty
			(String propertyName,
			 Object value,
			 Boolean destructureObjects,
			 out LogEventProperty property) =>
			impl.BindProperty(propertyName, value, destructureObjects, out property);
	}
}
