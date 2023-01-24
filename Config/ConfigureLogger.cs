using Serilog;
using Serilog.Events;

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
                .WriteTo.Console();
        });
        return builder;
    }
}