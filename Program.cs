using AccountyMinAPI.Api;
using AccountyMinAPI.DB;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
// Serilog configuration		
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
// Register Serilog
builder.Logging.AddSerilog(logger);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IPaymentTypeRepository, PaymentTypeRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.ConfigureApi();

app.Run();