using AccountyMinAPI.Config;
using AccountyMinAPI.Log;
using AccountyMinAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MongoConnectionString");
var appSecret = builder.Configuration["AppSecret"];
var clientUrl = builder.Configuration["ClientURL"];

builder.UseLogger();

builder.Services
    .AddTransient<GlobalExceptionHandlingMiddleware>()
    .RegisterMongoDB(connectionString)
    .AddEndpointsApiExplorer()
    .RegisterDI()
    .RegisterAuth(appSecret);

builder.Services.AddCors(options => options.AddPolicy("MyAllowedOrigins",
    policy =>
    {
        policy.WithOrigins(clientUrl)
            .AllowAnyHeader()
            .AllowAnyMethod();
    })
);

var app = builder.Build();
app.UseCors("MyAllowedOrigins");
app
    .UseRoutes()
    .UseMiddleware<GlobalExceptionHandlingMiddleware>()
    .UseAuthentication()
    .UseAuthorization()
    .UseHttpsRedirection();
app.Run();
