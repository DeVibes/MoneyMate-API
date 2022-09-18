using AccountyMinAPI.Api;
using AccountyMinAPI.DB;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
// Serilog configuration		
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
// Register Serilog
builder.Logging.AddSerilog(logger);
var client = builder.Configuration["ClientURL"];
builder.Services.AddCors(options => options.AddPolicy("MyAllowedOrigins", 
    policy => 
    {
        policy.WithOrigins(client).AllowAnyHeader()
            .AllowAnyMethod();
    })
);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IMongoClient>(serviceProvider => 
{
    var connectionString = builder.Configuration.GetConnectionString("MongoConnectionString");
    var settings = MongoClientSettings.FromConnectionString(connectionString);
    return new MongoClient(settings);
});
builder.Services.AddSingleton<ITransactionRepository, MongoTransactionRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IPaymentTypeRepository, PaymentTypeRepository>();

BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

var app = builder.Build();
app.UseCors("MyAllowedOrigins");

app.UseHttpsRedirection();

app.ConfigureApi();

app.Run();