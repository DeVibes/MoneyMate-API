using AccountyMinAPI;
using AccountyMinAPI.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.ConfigureApi();

app.Run();