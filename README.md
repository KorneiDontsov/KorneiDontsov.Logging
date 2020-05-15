# KorneiDontsov.Logging

Tools for logging based on [Serilog](https://github.com/serilog/serilog).

[![NuGet version](https://img.shields.io/nuget/v/KorneiDontsov.Logging.svg?style=for-the-badge)](https://www.nuget.org/packages/KorneiDontsov.Logging/)
[![NuGet status](https://img.shields.io/nuget/dt/KorneiDontsov.Logging?style=for-the-badge)](https://www.nuget.org/packages/KorneiDontsov.Logging/)

## Configured logging

`Serilog` stands for configuring by code using fluent API. It's good that
library provides functionality to configure logger without external sources.
That's enough in simple cases. But it's not so easy in the cases where you have
to use different configurations for different environments, since there must be
code for these environments. Configuring by external sources is more flexible and
useful, easy to change and extend.

`KorneiDontsov.Logging` provides functionality to configure logger through
[Microsoft.Extensions.Configuration.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Abstractions). Look at example.

`appsettings.json`

```json
{
    "$schema": "https://raw.githubusercontent.com/KorneiDontsov/KorneiDontsov.Logging/v1.4.0/src/schemas/appsettings.json",
    "logging": {
        "enrichments": {
            "thread": true
        },
        "profiles": {
            "defaultConsole": {
                "minLevel": "information",
                "sync": true,
                "type": "console",
                "output": [
                    "{Timestamp} [{Level:u3}] {SourceContext}",
                    "{Message:lj}{NewLine}",
                    "{Exception}"
                ]
            },
            "defaultFile": {
                "minLevel": "information",
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

`Program.cs`

```c#
class Program {
    public static void Main (String[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseConfiguredLogger()
            .ConfigureSomething()
            .Build()
            .Run();
}
```

`UseConfiguredLogger()` registers `ILogger` to services and configures it.
By default you can write to file and console, enrich thread id, thread name and
timestamp. But you can add your own profile types and enrichements implementing
and registering `ILoggingProfileApplier` and `ILoggingEnrichmentApplier`.

You can also use other methods of class `ConfiguredLoggingFunctions` to use
configured logging on platforms where you do not use
[Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting).

## CrashLogger

To activate `CrashLogger` you have to invoke

```c#
CrashLogger.Activate();
```

`CrashLogger` handles unhandled exceptions and unobserved task exceptions, and logs them
to shared `Serilog.Log.Logger` before the process is terminated. If shared logger does
not handle `Fatal` errors then `CrashLogger` logs to file with name like
`logs/critical/UnhandledException_{app}_{timestamp}.log` in application directory.

Also you can customize root path of critical logs:

```c#
var customPath = "var/critical-logs";
CrashLogger.Activate(customPath);
```

It's recommended to activate `CrashLogger` before any other code is executed.

## AOT compatible logging

AOT platforms (like Unity IL2CPP) does not support virtual generic methods where generic
parameters are structs. `AotLogger` decorates `ILogger` to get around this constraint.
For example:

```c#
ILogger logger = GetLogger();

// This invocation is not worked on AOT.
logger.Information<Int32>("One is {One}.", 1);

var aotLogger = new AotLogger(logger);

// It's worked;
aotLogger.Information<Int32>("One is {One}.", 1);
```

You can also use methods of statis class `AotLog` which repeat methods of `Serilog.Log`
and uses shared `Serilog.Log.Logger`.
