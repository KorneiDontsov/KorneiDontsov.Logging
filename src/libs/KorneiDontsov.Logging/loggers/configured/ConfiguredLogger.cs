// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Microsoft.Extensions.Configuration;
	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using static ConfiguredLoggingFunctions;

	class ConfiguredLogger: ILogger, IDisposable {
		public Logger impl { get; }
		ILogger overriden { get; }

		public ConfiguredLogger
			(IConfiguration configuration,
			 IEnumerable<ILoggingProfileApplier> profileAppliers,
			 IEnumerable<ILoggingEnrichmentApplier> enrichmentAppliers,
			 IEnumerable<ILoggingFilterApplier> filterAppliers,
			 OnLoggerConfigurationLoaded? onLoggerConfigurationLoaded = null) {
			impl =
				CreateConfiguredLogger(
					configuration.GetSection("logging"),
					profileAppliers,
					enrichmentAppliers,
					filterAppliers,
					onLoggerConfigurationLoaded);
			(overriden, Log.Logger) = (Log.Logger, impl);
		}

		Int32 isDisposedFlag;

		public void Dispose () {
			if(Interlocked.CompareExchange(ref isDisposedFlag, 1, 0) is 0) {
				Log.Logger = overriden;
				impl.Dispose();
			}
		}

		public ILogger ForContext (ILogEventEnricher enricher) => impl.ForContext(enricher);

		public ILogger ForContext (IEnumerable<ILogEventEnricher> enrichers) => impl.ForContext(enrichers);

		public ILogger ForContext
			(String propertyName, Object value, Boolean destructureObjects = false) =>
			impl.ForContext(propertyName, value, destructureObjects);

		public ILogger ForContext<TSource> () => impl.ForContext<TSource>();

		public ILogger ForContext (Type source) => impl.ForContext(source);

		public void Write (LogEvent logEvent) => impl.Write(logEvent);

		public void Write (LogEventLevel level, String messageTemplate) => impl.Write(level, messageTemplate);

		public void Write<T>
			(LogEventLevel level, String messageTemplate, T propertyValue) =>
			impl.Write(level, messageTemplate, propertyValue);

		public void Write<T0, T1>
			(LogEventLevel level, String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Write(
			level, messageTemplate, propertyValue0, propertyValue1);

		public void Write<T0, T1, T2>
			(LogEventLevel level, String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			impl.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Write
			(LogEventLevel level, String messageTemplate, params Object[] propertyValues) =>
			impl.Write(level, messageTemplate, propertyValues);

		public void Write
			(LogEventLevel level, Exception exception, String messageTemplate) =>
			impl.Write(level, exception, messageTemplate);

		public void Write<T>
			(LogEventLevel level, Exception exception, String messageTemplate, T propertyValue) =>
			impl.Write(level, exception, messageTemplate, propertyValue);

		public void Write<T0, T1>
			(LogEventLevel level, Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			impl.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);

		public void Write<T0, T1, T2>
			(LogEventLevel level, Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1,
			 T2 propertyValue2) =>
			impl.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Write
			(LogEventLevel level, Exception exception, String messageTemplate, params Object[] propertyValues) =>
			impl.Write(level, exception, messageTemplate, propertyValues);

		public Boolean IsEnabled (LogEventLevel level) => impl.IsEnabled(level);

		public void Verbose (String messageTemplate) => impl.Verbose(messageTemplate);

		public void Verbose<T>
			(String messageTemplate, T propertyValue) => impl.Verbose(messageTemplate, propertyValue);

		public void Verbose<T0, T1>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Verbose(
			messageTemplate, propertyValue0, propertyValue1);

		public void Verbose<T0, T1, T2>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => impl.Verbose(
			messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Verbose
			(String messageTemplate, params Object[] propertyValues) => impl.Verbose(messageTemplate, propertyValues);

		public void Verbose (Exception exception, String messageTemplate) => impl.Verbose(exception, messageTemplate);

		public void Verbose<T>
			(Exception exception, String messageTemplate, T propertyValue) =>
			impl.Verbose(exception, messageTemplate, propertyValue);

		public void Verbose<T0, T1>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Verbose(
			exception, messageTemplate, propertyValue0, propertyValue1);

		public void Verbose<T0, T1, T2>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			impl.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Verbose
			(Exception exception, String messageTemplate, params Object[] propertyValues) =>
			impl.Verbose(exception, messageTemplate, propertyValues);

		public void Debug (String messageTemplate) => impl.Debug(messageTemplate);

		public void Debug<T> (String messageTemplate, T propertyValue) => impl.Debug(messageTemplate, propertyValue);

		public void Debug<T0, T1>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Debug(
			messageTemplate, propertyValue0, propertyValue1);

		public void Debug<T0, T1, T2>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => impl.Debug(
			messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Debug
			(String messageTemplate, params Object[] propertyValues) => impl.Debug(messageTemplate, propertyValues);

		public void Debug (Exception exception, String messageTemplate) => impl.Debug(exception, messageTemplate);

		public void Debug<T>
			(Exception exception, String messageTemplate, T propertyValue) =>
			impl.Debug(exception, messageTemplate, propertyValue);

		public void Debug<T0, T1>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Debug(
			exception, messageTemplate, propertyValue0, propertyValue1);

		public void Debug<T0, T1, T2>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			impl.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Debug
			(Exception exception, String messageTemplate, params Object[] propertyValues) =>
			impl.Debug(exception, messageTemplate, propertyValues);

		public void Information (String messageTemplate) => impl.Information(messageTemplate);

		public void Information<T>
			(String messageTemplate, T propertyValue) => impl.Information(messageTemplate, propertyValue);

		public void Information<T0, T1>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Information(
			messageTemplate, propertyValue0, propertyValue1);

		public void Information<T0, T1, T2>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => impl.Information(
			messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Information
			(String messageTemplate, params Object[] propertyValues) =>
			impl.Information(messageTemplate, propertyValues);

		public void Information
			(Exception exception, String messageTemplate) => impl.Information(exception, messageTemplate);

		public void Information<T>
			(Exception exception, String messageTemplate, T propertyValue) =>
			impl.Information(exception, messageTemplate, propertyValue);

		public void Information<T0, T1>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Information(
			exception, messageTemplate, propertyValue0, propertyValue1);

		public void Information<T0, T1, T2>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			impl.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Information
			(Exception exception, String messageTemplate, params Object[] propertyValues) =>
			impl.Information(exception, messageTemplate, propertyValues);

		public void Warning (String messageTemplate) => impl.Warning(messageTemplate);

		public void Warning<T>
			(String messageTemplate, T propertyValue) => impl.Warning(messageTemplate, propertyValue);

		public void Warning<T0, T1>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Warning(
			messageTemplate, propertyValue0, propertyValue1);

		public void Warning<T0, T1, T2>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => impl.Warning(
			messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Warning
			(String messageTemplate, params Object[] propertyValues) => impl.Warning(messageTemplate, propertyValues);

		public void Warning (Exception exception, String messageTemplate) => impl.Warning(exception, messageTemplate);

		public void Warning<T>
			(Exception exception, String messageTemplate, T propertyValue) =>
			impl.Warning(exception, messageTemplate, propertyValue);

		public void Warning<T0, T1>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Warning(
			exception, messageTemplate, propertyValue0, propertyValue1);

		public void Warning<T0, T1, T2>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			impl.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Warning
			(Exception exception, String messageTemplate, params Object[] propertyValues) =>
			impl.Warning(exception, messageTemplate, propertyValues);

		public void Error (String messageTemplate) => impl.Error(messageTemplate);

		public void Error<T> (String messageTemplate, T propertyValue) => impl.Error(messageTemplate, propertyValue);

		public void Error<T0, T1>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Error(
			messageTemplate, propertyValue0, propertyValue1);

		public void Error<T0, T1, T2>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => impl.Error(
			messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Error
			(String messageTemplate, params Object[] propertyValues) => impl.Error(messageTemplate, propertyValues);

		public void Error (Exception exception, String messageTemplate) => impl.Error(exception, messageTemplate);

		public void Error<T>
			(Exception exception, String messageTemplate, T propertyValue) =>
			impl.Error(exception, messageTemplate, propertyValue);

		public void Error<T0, T1>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Error(
			exception, messageTemplate, propertyValue0, propertyValue1);

		public void Error<T0, T1, T2>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			impl.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Error
			(Exception exception, String messageTemplate, params Object[] propertyValues) =>
			impl.Error(exception, messageTemplate, propertyValues);

		public void Fatal (String messageTemplate) => impl.Fatal(messageTemplate);

		public void Fatal<T> (String messageTemplate, T propertyValue) => impl.Fatal(messageTemplate, propertyValue);

		public void Fatal<T0, T1>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Fatal(
			messageTemplate, propertyValue0, propertyValue1);

		public void Fatal<T0, T1, T2>
			(String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => impl.Fatal(
			messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Fatal
			(String messageTemplate, params Object[] propertyValues) => impl.Fatal(messageTemplate, propertyValues);

		public void Fatal (Exception exception, String messageTemplate) => impl.Fatal(exception, messageTemplate);

		public void Fatal<T>
			(Exception exception, String messageTemplate, T propertyValue) =>
			impl.Fatal(exception, messageTemplate, propertyValue);

		public void Fatal<T0, T1>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1) => impl.Fatal(
			exception, messageTemplate, propertyValue0, propertyValue1);

		public void Fatal<T0, T1, T2>
			(Exception exception, String messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			impl.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Fatal
			(Exception exception, String messageTemplate, params Object[] propertyValues) =>
			impl.Fatal(exception, messageTemplate, propertyValues);

		public Boolean BindMessageTemplate
			(String messageTemplate, Object[] propertyValues, out MessageTemplate parsedTemplate,
			 out IEnumerable<LogEventProperty> boundProperties) =>
			impl.BindMessageTemplate(messageTemplate, propertyValues, out parsedTemplate, out boundProperties);

		public Boolean BindProperty
			(String propertyName, Object value, Boolean destructureObjects, out LogEventProperty property) =>
			impl.BindProperty(propertyName, value, destructureObjects, out property);
	}
}
