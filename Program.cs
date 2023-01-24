using AccountyMinAPI.Config;
using AccountyMinAPI.Log;
using AccountyMinAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MongoConnectionString");
var clientUrl = builder.Configuration["ClientURL"];

builder.UseLogger();

builder.Services
    .RegisterMongoDB(connectionString)
    .AddEndpointsApiExplorer()
    .RegisterDI()
    .RegisterAuth();

// builder.Services.AddCors(options => options.AddPolicy("MyAllowedOrigins",
//     policy =>
//     {
//         policy.WithOrigins(clientUrl)
//             .AllowAnyHeader()
//             .AllowAnyMethod();
//     })
// );

var app = builder.Build();
// app.UseCors("MyAllowedOrigins");
app
    .UseRoutes()
    // .UseMiddleware<GlobalExceptionHandlingMiddleware>()
    .UseAuthentication()
    .UseAuthorization()
    .UseHttpsRedirection();

// app.Use(async (context, next) => 
// {
//     try
//     {
//         await next(context);
//     }
//     catch (Exception ex)
//     {
//         context.Response.StatusCode = 500;
//     }
// });

app.Run();
