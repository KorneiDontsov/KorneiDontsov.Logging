// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT.
// See LICENSE in the project root for license information.

namespace KorneiDontsov.Logging {
	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using System;
	using System.Collections.Generic;

	/// <inheritdoc />
	/// <summary>
	///     AOT compatible logger that decorates <see cref = "T:Serilog.ILogger" />.
	/// </summary>
	public sealed class AotLogger: IDisposable {
		readonly Logger? fastImpl;

		readonly ILogger impl;

		// ReSharper disable once InconsistentNaming
		public ILogger Impl => impl;

		public AotLogger (ILogger impl) {
			fastImpl = impl as Logger;
			this.impl = impl;
		}

		// ReSharper disable once InconsistentNaming
		public static AotLogger None { get; } = new(Logger.None);

		/// <inheritdoc />
		public void Dispose () {
			if(fastImpl is { }) fastImpl.Dispose();
			else if(impl is IDisposable disposableImpl) disposableImpl.Dispose();
		}

		/// <inheritdoc cref = "ILogger.ForContext(ILogEventEnricher)" />
		public AotLogger ForContext (ILogEventEnricher enricher) =>
			new(impl.ForContext(enricher));

		/// <inheritdoc cref = "ILogger.ForContext(IEnumerable{ILogEventEnricher})" />
		public AotLogger ForContext (IEnumerable<ILogEventEnricher> enrichers) =>
			new(impl.ForContext(enrichers));

		/// <inheritdoc cref = "ILogger.ForContext(string, object, bool)" />
		public AotLogger ForContext (String propertyName, Object? value, Boolean destructureObjects = false) =>
			new(impl.ForContext(propertyName, value, destructureObjects));

		/// <inheritdoc cref = "ILogger.ForContext{TSource}" />
		public AotLogger ForContext<TSource> () =>
			new(impl.ForContext(typeof(TSource)));

		/// <inheritdoc cref = "ILogger.ForContext(Type)" />
		public AotLogger ForContext (Type source) =>
			new(impl.ForContext(source));

		/// <inheritdoc cref = "ILogger.Write(LogEvent)" />
		public void Write (LogEvent logEvent) =>
			impl.Write(logEvent);

		/// <inheritdoc cref = "ILogger.Write(LogEventLevel, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write (LogEventLevel level, String messageTemplate) =>
			impl.Write(level, messageTemplate);

		/// <inheritdoc cref = "ILogger.Write{T}(LogEventLevel, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write<T> (LogEventLevel level, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Write(level, messageTemplate, propertyValue);
			else if(impl.IsEnabled(level))
				impl.Write(level, messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Write{T0, T1}(LogEventLevel, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write<T0, T1> (LogEventLevel level, String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Write(level, messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(level))
				impl.Write(level, messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Write{T0, T1, T2}(LogEventLevel, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write<T0, T1, T2>
			(LogEventLevel level,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(level))
				impl.Write(level, messageTemplate, new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Write(LogEventLevel, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write (LogEventLevel level, String messageTemplate, params Object?[] propertyValues) =>
			impl.Write(level, messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Write(LogEventLevel, Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write (LogEventLevel level, Exception exception, String messageTemplate) =>
			impl.Write(level, exception, messageTemplate);

		/// <inheritdoc cref = "ILogger.Write{T}(LogEventLevel, Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write<T> (LogEventLevel level, Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Write(level, exception, messageTemplate, propertyValue);
			else if(impl.IsEnabled(level))
				impl.Write(level, exception, messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Write{T0, T1}(LogEventLevel, Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write<T0, T1>
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(level))
				impl.Write(level, exception, messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Write{T0, T1, T2}(LogEventLevel, Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write<T0, T1, T2>
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(level))
				impl.Write(
					level,
					exception,
					messageTemplate,
					new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Write(LogEventLevel, Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Write
			(LogEventLevel level,
			 Exception exception,
			 String messageTemplate,
			 params Object?[] propertyValues) =>
			impl.Write(level, exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.IsEnabled(LogEventLevel)" />
		public Boolean IsEnabled (LogEventLevel level) =>
			impl.IsEnabled(level);

		/// <inheritdoc cref = "ILogger.Verbose(string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose (String messageTemplate) =>
			impl.Verbose(messageTemplate);

		/// <inheritdoc cref = "ILogger.Verbose{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Verbose(messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Verbose))
				impl.Verbose(messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Verbose{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Verbose(messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Verbose))
				impl.Verbose(messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Verbose{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Verbose))
				impl.Verbose(messageTemplate, new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Verbose(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose (String messageTemplate, params Object?[] propertyValues) =>
			impl.Verbose(messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Verbose(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose (Exception exception, String messageTemplate) =>
			impl.Verbose(exception, messageTemplate);

		/// <inheritdoc cref = "ILogger.Verbose{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Verbose(exception, messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Verbose))
				impl.Verbose(exception, messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Verbose{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Verbose))
				impl.Verbose(exception, messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Verbose{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Verbose))
				impl.Verbose(
					exception,
					messageTemplate,
					new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Verbose(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Verbose (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Verbose(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Debug(string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug (String messageTemplate) =>
			impl.Debug(messageTemplate);

		/// <inheritdoc cref = "ILogger.Debug{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Debug(messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Debug))
				impl.Debug(messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Debug{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Debug(messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Debug))
				impl.Debug(messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Debug{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Debug))
				impl.Debug(messageTemplate, new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Debug(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug (String messageTemplate, params Object?[] propertyValues) =>
			impl.Debug(messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Debug(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug (Exception exception, String messageTemplate) =>
			impl.Debug(exception, messageTemplate);

		/// <inheritdoc cref = "ILogger.Debug{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Debug(exception, messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Debug))
				impl.Debug(exception, messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Debug{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Debug))
				impl.Debug(exception, messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Debug{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Debug))
				impl.Debug(
					exception,
					messageTemplate,
					new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Debug(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Debug (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Debug(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Information(string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information (String messageTemplate) =>
			impl.Information(messageTemplate);

		/// <inheritdoc cref = "ILogger.Information{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Information(messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Information))
				impl.Information(messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Information{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Information(messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Information))
				impl.Information(messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Information{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Information))
				impl.Information(messageTemplate, new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Information(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information (String messageTemplate, params Object?[] propertyValues) =>
			impl.Information(messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Information(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information (Exception exception, String messageTemplate) =>
			impl.Information(exception, messageTemplate);

		/// <inheritdoc cref = "ILogger.Information{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Information(exception, messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Information))
				impl.Information(exception, messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Information{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Information(exception, messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Information))
				impl.Information(exception, messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Information{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Information))
				impl.Information(
					exception,
					messageTemplate,
					new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Information(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Information (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Information(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Warning(string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning (String messageTemplate) =>
			impl.Warning(messageTemplate);

		/// <inheritdoc cref = "ILogger.Warning{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Warning(messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Warning))
				impl.Warning(messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Warning{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Warning(messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Warning))
				impl.Warning(messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Warning{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Warning))
				impl.Warning(messageTemplate, new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Warning(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning (String messageTemplate, params Object?[] propertyValues) =>
			impl.Warning(messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Warning(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning (Exception exception, String messageTemplate) =>
			impl.Warning(exception, messageTemplate);

		/// <inheritdoc cref = "ILogger.Warning{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Warning(exception, messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Warning))
				impl.Warning(exception, messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Warning{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Warning(exception, messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Warning))
				impl.Warning(exception, messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Warning{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Warning))
				impl.Warning(
					exception,
					messageTemplate,
					new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Warning(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Warning (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Warning(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Error(string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error (String messageTemplate) =>
			impl.Error(messageTemplate);

		/// <inheritdoc cref = "ILogger.Error{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Error(messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Error)) impl.Error(messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Error{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Error(messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Error))
				impl.Error(messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Error{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Error))
				impl.Error(messageTemplate, new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Error(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error (String messageTemplate, params Object?[] propertyValues) =>
			impl.Error(messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Error(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error (Exception exception, String messageTemplate) =>
			impl.Error(exception, messageTemplate);

		/// <inheritdoc cref = "ILogger.Error{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Error(exception, messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Error))
				impl.Error(exception, messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Error{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Error(exception, messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Error))
				impl.Error(exception, messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Error{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Error))
				impl.Error(
					exception,
					messageTemplate,
					new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Error(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Error (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Error(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Fatal(string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal (String messageTemplate) =>
			impl.Fatal(messageTemplate);

		/// <inheritdoc cref = "ILogger.Fatal{T}(string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal<T> (String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Fatal(messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Fatal))
				impl.Fatal(messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Fatal{T0, T1}(string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal<T0, T1> (String messageTemplate, T0 propertyValue0, T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Fatal(messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Fatal))
				impl.Fatal(messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Fatal{T0, T1, T2}(string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal<T0, T1, T2>
			(String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Fatal))
				impl.Fatal(messageTemplate, new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Fatal(string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal (String messageTemplate, params Object?[] propertyValues) =>
			impl.Fatal(messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.Fatal(Exception, string)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal (Exception exception, String messageTemplate) =>
			impl.Fatal(exception, messageTemplate);

		/// <inheritdoc cref = "ILogger.Fatal{T}(Exception, string, T)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal<T> (Exception exception, String messageTemplate, T propertyValue) {
			if(fastImpl is { })
				fastImpl.Fatal(exception, messageTemplate, propertyValue);
			else if(impl.IsEnabled(LogEventLevel.Fatal))
				impl.Fatal(exception, messageTemplate, new Object?[] { propertyValue });
		}

		/// <inheritdoc cref = "ILogger.Fatal{T0, T1}(Exception, string, T0, T1)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal<T0, T1>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1) {
			if(fastImpl is { })
				fastImpl.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
			else if(impl.IsEnabled(LogEventLevel.Fatal))
				impl.Fatal(exception, messageTemplate, new Object?[] { propertyValue0, propertyValue1 });
		}

		/// <inheritdoc cref = "ILogger.Fatal{T0, T1, T2}(Exception, string, T0, T1, T2)" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal<T0, T1, T2>
			(Exception exception,
			 String messageTemplate,
			 T0 propertyValue0,
			 T1 propertyValue1,
			 T2 propertyValue2) {
			if(fastImpl is { })
				fastImpl.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
			else if(impl.IsEnabled(LogEventLevel.Fatal))
				impl.Fatal(
					exception,
					messageTemplate,
					new Object?[] { propertyValue0, propertyValue1, propertyValue2 });
		}

		/// <inheritdoc cref = "ILogger.Fatal(Exception, string, object[])" />
		[MessageTemplateFormatMethod("messageTemplate")]
		public void Fatal (Exception exception, String messageTemplate, params Object?[] propertyValues) =>
			impl.Fatal(exception, messageTemplate, propertyValues);

		/// <inheritdoc cref = "ILogger.BindMessageTemplate" />
		[MessageTemplateFormatMethod("messageTemplate")]
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

		/// <inheritdoc cref = "ILogger.BindProperty" />
		public Boolean BindProperty
			(String propertyName,
			 Object value,
			 Boolean destructureObjects,
			 out LogEventProperty property) =>
			impl.BindProperty(propertyName, value, destructureObjects, out property);
	}
}
