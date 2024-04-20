### About

The project is a simple library meant to be an extension method leveraged with Serilog for a straightforward, repeatable logging approach throughout an application.  Once you implement the library,  an agent can allow the code to be telemetered. You must instantiate the Serilog library through your startup independant of these class files to leverage the extension methods.

### Installation

You can install it in multiple ways depending on which platform you're using. If you use your default terminal window with the dotnet command line interface.

```powershell
dotnet add package cws.application-logger
```

If your inside Visual Studio Code or Visual Studio you can leverage their terminal with:

```powershell
Install-Package cws.application-logger
```

If you would like to use the Visual Studio graphical user interface you can do the following:

1. &#x20;Select **Project** in upper left hand corner.
2. &#x20;Choose **Manage Nuget Packages**
3. &#x20;Select browse, then search for **cws.application-logger**

### Code Example

```csharp
public static class IServiceCollection
{
    public static void Register(this IServiceCollection services)
    {
        services.AddLogging(option => option.AddSerilog());
        services.AddSingleton(Log.Logger);
    }
}
```

```csharp
public class Startup
{
    public static async Task Main(string[] arguments) => await Host.CreateDefaultBuilder(arguments)
        .ConfigureWebHostDefaults(builder => { builder.UseStartup<Startup>(); })
        .RunConsoleAsync();

    public Startup()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File($@"{logPath}\log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
     }
    
    public void ConfigureServices(this IServiceCollection) => services.Register();
}
```

After you use the above configuration, which couples dependency injection for this particular example, in theory you could use the single line for the extension method. To actually leverage the library:

```csharp
/* Telemetered Event: */
logger.Capture(LogLevel.Error, "Namespace.Class.Method", exception);

/* Information */
logger.Capture(LogLevel.Information, "Namespace.Class.Method", "A large block", "of text", "that should be iterated", "over multiple lines.");
logger.Capture(LogLevel.Information, "Namespace.Class.Method", "Populate param with a single entry.");
```

### Dependencies:

The project uses the following libraries:

| Name                  | Version |
| --------------------- | ------- |
| Serilog               | 3.1.1   |
| Serilog.AspNetCore    | 8.0.1   |
| Serilog.Sinks.Console | 5.0.1   |
| Serilog.Sinks.File    | 5.0.0   |
| Datadog.Trace.Bundle  | 2.49.0  |

<br>
