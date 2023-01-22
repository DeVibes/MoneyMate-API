using AccountyMinAPI.Api;
using AccountyMinAPI.Auth;
using AccountyMinAPI.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var allowedUsernames = builder.Configuration.GetSection("AllowedUsernames").Get<List<string>>();

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
        policy.WithOrigins(client)
            .AllowAnyHeader()
            .AllowAnyMethod();
    })
);
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient>(serviceProvider => 
{
    var connectionString = builder.Configuration.GetConnectionString("MongoConnectionString");
    var settings = MongoClientSettings.FromConnectionString(connectionString);
    return new MongoClient(settings);
});
builder.Services.AddSingleton<ITransactionRepository, MongoTransactionRepository>();
builder.Services.AddSingleton<IAccountRepository, MongoAccountRepository>();
builder.Services.AddSingleton<IUsernameRepository, MongoUsernameRepository>();
builder.Services.AddSingleton<TokenService>();

var secretKey = Auth.GenerateSecretByte();
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(APIRoles.Admin, policy => policy.RequireRole(APIRoles.Admin));
    options.AddPolicy(APIRoles.User, policy => policy.RequireRole(APIRoles.User));
});

BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

var app = builder.Build();
app.UseCors("MyAllowedOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.ConfigureApi();

app.Run();