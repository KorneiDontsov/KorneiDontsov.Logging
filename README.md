# KorneiDontsov.Logging

Tools for logging based on [Serilog](https://github.com/serilog/serilog).

[![NuGet version](https://img.shields.io/nuget/v/KorneiDontsov.Logging.svg?style=for-the-badge)](https://www.nuget.org/packages/KorneiDontsov.Logging/)
[![NuGet status](https://img.shields.io/nuget/dt/KorneiDontsov.Logging?style=for-the-badge)](https://www.nuget.org/packages/KorneiDontsov.Logging/)

- [Configured logger](#configured-logger)
- [Generic logger](#generic-logger)
- [Crash logger](#crash-logger)
- [AOT compatible logger](#aot-compatible-logger)
- [Conclusion](#conclusion)

## Configured logger

`Serilog` stands for configuring by code using fluent API. It's good that library provides functionality to configure
logger without external sources. That's enough in simple cases. But it's not so easy in the cases where you have to use
different configurations for different environments, since there must be code for these environments. Configuring by
external sources is more flexible and useful, easy to change and extend.

`KorneiDontsov.Logging` provides functionality to configure logger through
[Microsoft.Extensions.Configuration.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Abstractions)
. Look at example.

`appsettings.json`

```json
{
    "$schema": "https://raw.githubusercontent.com/KorneiDontsov/KorneiDontsov.Logging/v2.0.0/schema/appsettings.json",
    "logging": {
        "minLevels": {
            "default": "information",
            "overrides": {
                "KorneiDontsov.Logging.Example.ServiceExample": "debug"
            }
        },
        "enrichments": {
            "thread": true
        },
        "profiles": {
            "console": {
                "type": "console",
                "output": [
                    "[{Level:u3}] {Timestamp} [{SourceContext}]",
                    "{Message:lj}",
                    "Properties: {Properties}",
                    "{Exception}"
                ]
            },
            "file": {
                "type": "file",
                "output": [
                    "Timestamp: {Timestamp}",
                    "Level: {Level}",
                    "SourceContext: {SourceContext}",
                    "Message: {Message:lj}",
                    "Properties: {Properties}",
                    "{Exception}"
                ],
                "path": "logs/{app}_{launchUtc}.log",
                "maxSize": 10485760
            },
            "errorFile": {
                "sync": true,
                "minLevel": "error",
                "type": "file",
                "output": [
                    "Timestamp: {Timestamp}",
                    "Message: {Message:lj}",
                    "Properties: {Properties}",
                    "{Exception}"
                ],
                "path": "logs/errors/{app}_{launchUtc}.log",
                "maxSize": 10485760
            }
        }
    }
}
```

**Note:** use JSON Schema to learn all parameters that are available to configure. Schema of logging is described
in [/schema/logging.json](./schema/logging.json).

`Program.cs`

```c#
Host.CreateDefaultBuilder(args)
    .UseConfiguredLogger()
    .ConfigureSomething()
    .Build()
    .Run();
```

`UseConfiguredLogger()` registers `ILogger` to services and configures it. By default you can write to file and console,
enrich thread id & name. But the functionality can be extended. To add your own components you can implement special
interfaces and register them in DI container:

- `ILoggingProfileApplier` applies sinks (like `LoggerConfiguration.WriteTo`);
- `ILoggingEnrichmentApplier` applies enrichers (like `LoggerConfiguration.Enrich`);
- `ILoggingFilterApplier` applies filters (like `LoggerConfiguration.Filter`).

You can still configure some components of logger by code:

```c#
.UseConfiguredLogger(
    conf => conf.Enrich.WithProperty("EnrichedByCode", true))
```

Also you are available to use the other methods of class `ConfiguredLoggingFunctions` to get configured logging on the
platforms where you do not
use [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting).

## Generic logger

You can use `KorneiDontsov.Logging.ILogger<TSource>` to specify the owner of injected logger by a generic argument:

```c#
using Serilog;
using KorneiDontsov.Logging;

class MyService
{
    readonly ILogger logger;

    public MyService (ILogger<MyService> logger) =>
        this.logger = logger;

    ...
}
```

It's the equivalent for:

```c#
using Serilog;

class MyService
{
    readonly ILogger logger;

    public MyService (ILogger logger) =>
        this.logger = logger.ForContext<MyService>();

    ...
}
```

`KorneiDontsov.Logging.ILogger<TSource>` inherits from `Serilog.ILogger`, so it just helps to reduce boilerplate code
and cache instance of logger in DI container.

To make it work you have to invoke `UseGenericLogger()` on your host builder.

## Crash logger

`CrashLogger` handles unhandled exceptions and unobserved task exceptions, and logs them to global `Serilog.Log.Logger`.
To activate `CrashLogger` you have to invoke

```c#
CrashLogger.Activate();
```

It can also log a crash to the console or create a file, always or if global logger doesn't accept crash log. By default
it:

- always creates a file with a crash log at `logs/crashes` in directory of program;
- writes a crash log to the console, if global logger didn't accept it.

The behaviour can be configured by `CrashLoggerOptions`. Put the options to the activate method. For example:

```c#
CrashLogger.Activate(
    new CrashLoggerOptions {
        writeToFile = CrashLogWriteCondition.IfGlobalLoggerMissed,
        crashLogRootPath = Path.Combine(AppContext.BaseDirectory, "var/crash-logs")
    });
```

It's recommended to activate `CrashLogger` before any other code is executed.

## AOT compatible logger

AOT platforms (like Unity IL2CPP) usually does not support virtual generic methods where generic parameters are
structs. `AotLogger` decorates `ILogger` to get around this constraint. For example:

```c#
ILogger logger = GetLogger();

// This invocation doesn't work on AOT platforms.
logger.Information<Int32>("One is {One}.", 1);

var aotLogger = new AotLogger(logger);

// This one works.
aotLogger.Information<Int32>("One is {One}.", 1);
```

You can also use methods of static class `AotLog` which repeat methods of `Serilog.Log`
and uses global `Serilog.Log.Logger`.

## Conclusion

1. If you develop a program, based on generic host, your `Program.cs` should look like:
   ```c#
   CrashLogger.Activate();
   Host.CreateDefaultBuilder(args)
       .UseConfiguredLogger()
       .UseGenericLogger()
       .ConfigureSomething()
       .Build()
       .Run();
   ```

2. If you doesn't use generic host, but use some of its components, you can learn `KorneiDontsov.Logging` API to find
   how to get all functionality you want to get.

3. If you want to use Serilog on AOT platforms, you would rather use `AotLogger` and `AotLog`.
