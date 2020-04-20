// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using System;
	using System.Collections.Generic;
	using static System.Threading.Interlocked;

	public class AotLog {
		static volatile AotLogger logger =
			AotLogger.None;

		/// <summary>
		///     The globally-shared AOT compatible logger that decorates <see cref = "Serilog.Core.Logger" />.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static AotLogger Logger {
			get {
				var oldValue = logger;
				var loggerImpl = Log.Logger;
				if(ReferenceEquals(oldValue.Impl, loggerImpl))
					return oldValue;
				else
					do {
						var newValue = new AotLogger(loggerImpl);
						var exchangedValue = CompareExchange(ref logger, newValue, oldValue);
						if(ReferenceEquals(exchangedValue, oldValue))
							return newValue;
						else {
							oldValue = logger;
							loggerImpl = Log.Logger;
							if(ReferenceEquals(oldValue.Impl, loggerImpl)) return oldValue;
						}
					} while(true);
			}
		}

		/// <inheritdoc cref = "Log.CloseAndFlush()" />
		public static void CloseAndFlush () {
			Log.CloseAndFlush();
			_ = Logger;
		}

		/// <inheritdoc cref = "Log.ForContext(ILogEventEnricher)" />
		public static AotLogger ForContext (ILogEventEnricher enricher) =>
			Logger.ForContext(enricher);

		/// <inheritdoc cref = "Log.ForContext(ILogEventEnricher[])" />
		public static AotLogger ForContext (ILogEventEnricher[] enrichers) =>
			Logger.ForContext(enrichers);

		/// <inheritdoc cref = "Log.ForContext(string, object?, bool)" />
		public static AotLogger ForContext (String propertyName, Object? value, Boolean destructiveObjects = false) =>
			Logger.ForContext(propertyName, value, destructiveObjects);

		/// <inheritdoc cref = "Log.ForContext{TSource}()" />
		public static AotLogger ForContext<TSource> () =>
			Logger.ForContext<TSource>();

		/// <inheritdoc cref = "Log.ForContext(Type)" />
		public static AotLogger ForContext (Type source) =>
			Logger.ForContext(source);

		/// <inheritdoc cref = "Log.Write(LogEvent)" />
		public static void Write (LogEvent logEvent) =>
			Logger.Write(logEvent);

		/// <inheritdoc cref = "Log.Write(LogEventLevel, Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write (LogEventLevel level, String messageTemplate) =>
			Logger.Write(level, messageTemplate);

		/// <inheritdoc cref = "Log.Write{T}(LogEventLevel, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write<T> (LogEventLevel level, String messageTemplate, T propertyValue) =>
			Logger.Write(level, messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Write{T0, T1}(LogEventLevel, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write<T0, T1>
			(LogEventLevel level,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) =>
			Logger.Write(level, messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Write{T0, T1, T2}(LogEventLevel, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write<T0, T1, T2>
			(LogEventLevel level,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Write(LogEventLevel, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write (LogEventLevel level, String messageTemplate, params Object?[] propertyValues) =>
			Logger.Write(level, messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Write(LogEventLevel, Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write (LogEventLevel level, Exception exception, String messageTemplate) =>
			Logger.Write(level, exception, messageTemplate);

		/// <inheritdoc cref = "Log.Write{T}(LogEventLevel, Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write<T>
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 T propertyValue) =>
			Logger.Write(level, exception, messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Write{T0, T1}(LogEventLevel, Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write<T0, T1>
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) =>
			Logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Write{T0, T1, T2}(LogEventLevel, Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write<T0, T1, T2>
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Write(LogEventLevel, Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Write
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			Logger.Write(level, exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Verbose(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose (String messageTemplate) =>
			Logger.Verbose(messageTemplate);

		/// <inheritdoc cref = "Log.Verbose{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose<T> (String messageTemplate, T propertyValue) =>
			Logger.Verbose(messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Verbose{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			Logger.Verbose(messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Verbose{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Verbose(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose (String messageTemplate, params Object?[] propertyValues) =>
			Logger.Verbose(messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Verbose(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose (Exception exception, String messageTemplate) =>
			Logger.Verbose(exception, messageTemplate);

		/// <inheritdoc cref = "Log.Verbose{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose<T>
			(Exception exception,
			 String messageTemplate,
			 T propertyValue) =>
			Logger.Verbose(exception, messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Verbose{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) =>
			Logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Verbose{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Verbose(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Verbose
			(Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			Logger.Verbose(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Debug(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug (String messageTemplate) =>
			Logger.Debug(messageTemplate);

		/// <inheritdoc cref = "Log.Debug{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug<T> (String messageTemplate, T propertyValue) =>
			Logger.Debug(messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Debug{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			Logger.Debug(messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Debug{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Debug(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug (String messageTemplate, params Object?[] propertyValues) =>
			Logger.Debug(messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Debug(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug (Exception exception, String messageTemplate) =>
			Logger.Debug(exception, messageTemplate);

		/// <inheritdoc cref = "Log.Debug{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug<T>
			(Exception exception,
			 String messageTemplate,
			 T propertyValue) =>
			Logger.Debug(exception, messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Debug{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) =>
			Logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Debug{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Debug(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Debug
			(Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			Logger.Debug(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Information(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information (String messageTemplate) =>
			Logger.Information(messageTemplate);

		/// <inheritdoc cref = "Log.Information{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information<T> (String messageTemplate, T propertyValue) =>
			Logger.Information(messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Information{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			Logger.Information(messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Information{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Information(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information (String messageTemplate, params Object?[] propertyValues) =>
			Logger.Information(messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Information(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information (Exception exception, String messageTemplate) =>
			Logger.Information(exception, messageTemplate);

		/// <inheritdoc cref = "Log.Information{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information<T>
			(Exception exception,
			 String messageTemplate,
			 T propertyValue) =>
			Logger.Information(exception, messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Information{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) =>
			Logger.Information(exception, messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Information{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Information(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Information
			(Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			Logger.Information(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Warning(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning (String messageTemplate) =>
			Logger.Warning(messageTemplate);

		/// <inheritdoc cref = "Log.Warning{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning<T> (String messageTemplate, T propertyValue) =>
			Logger.Warning(messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Warning{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			Logger.Warning(messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Warning{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Warning(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning (String messageTemplate, params Object?[] propertyValues) =>
			Logger.Warning(messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Warning(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning (Exception exception, String messageTemplate) =>
			Logger.Warning(exception, messageTemplate);

		/// <inheritdoc cref = "Log.Warning{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning<T>
			(Exception exception,
			 String messageTemplate,
			 T propertyValue) =>
			Logger.Warning(exception, messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Warning{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) =>
			Logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Warning{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Warning(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Warning
			(Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			Logger.Warning(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Error(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error (String messageTemplate) =>
			Logger.Error(messageTemplate);

		/// <inheritdoc cref = "Log.Error{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error<T> (String messageTemplate, T propertyValue) =>
			Logger.Error(messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Error{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			Logger.Error(messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Error{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Error(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error (String messageTemplate, params Object?[] propertyValues) =>
			Logger.Error(messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Error(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error (Exception exception, String messageTemplate) =>
			Logger.Error(exception, messageTemplate);

		/// <inheritdoc cref = "Log.Error{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error<T>
			(Exception exception,
			 String messageTemplate,
			 T propertyValue) =>
			Logger.Error(exception, messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Error{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) =>
			Logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Error{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Error(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Error
			(Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			Logger.Error(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Fatal(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal (String messageTemplate) =>
			Logger.Fatal(messageTemplate);

		/// <inheritdoc cref = "Log.Fatal{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal<T> (String messageTemplate, T propertyValue) =>
			Logger.Fatal(messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Fatal{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			Logger.Fatal(messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Fatal{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Fatal(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal (String messageTemplate, params Object?[] propertyValues) =>
			Logger.Fatal(messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.Fatal(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal (Exception exception, String messageTemplate) =>
			Logger.Fatal(exception, messageTemplate);

		/// <inheritdoc cref = "Log.Fatal{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal<T>
			(Exception exception,
			 String messageTemplate,
			 T propertyValue) =>
			Logger.Fatal(exception, messageTemplate, propertyValue);

		/// <inheritdoc cref = "Log.Fatal{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) =>
			Logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);

		/// <inheritdoc cref = "Log.Fatal{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) =>
			Logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		/// <inheritdoc cref = "Log.Fatal(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static void Fatal
			(Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			Logger.Fatal(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "Log.BindMessageTemplate" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public static Boolean BindMessageTemplate
			(String messageTemplate,
			 Object[] propertyValues,
			 out MessageTemplate parsedTemplate,
			 out IEnumerable<LogEventProperty> boundProperties) =>
			Logger.BindMessageTemplate(messageTemplate, propertyValues, out parsedTemplate, out boundProperties);

		/// <inheritdoc cref = "Log.BindProperty" />
		public static Boolean BindProperty
			(String propertyName,
			 Object value,
			 Boolean destructureObjects,
			 out LogEventProperty property) =>
			Logger.BindProperty(propertyName, value, destructureObjects, out property);
	}
}
