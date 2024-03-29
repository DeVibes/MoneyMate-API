using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace AccountyMinAPI.Log;

public static class ConfigureLogger
{
    public static WebApplicationBuilder UseLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        // Serilog configuration		
        builder.Host.UseSerilog((hostContext, services, configuration) =>
        {
            configuration
                .WriteTo.File("Log/serilog-file.txt")
                .WriteTo.File("Log/serilog-errors.txt", LogEventLevel.Error)
                .WriteTo.File("Log/serilog-info.txt", LogEventLevel.Debug)
                .WriteTo.Console(new CompactJsonFormatter());
        });
        return builder;
    }
}