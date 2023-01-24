using AccountyMinAPI.Auth;

namespace AccountyMinAPI.Config;

public static class ConfigureDI
{
    public static IServiceCollection RegisterDI(this IServiceCollection services)
    {
        services.AddSingleton<ITransactionRepository, MongoTransactionRepository>();
        services.AddSingleton<IAccountRepository, MongoAccountRepository>();
        services.AddSingleton<IUsernameRepository, MongoUsernameRepository>();
        services.AddSingleton<TokenService>();
        return services;
    }
}