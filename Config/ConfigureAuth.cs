using AccountyMinAPI.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AccountyMinAPI.Config;

public static class ConfigureAuth
{
    public static IServiceCollection RegisterAuth(this IServiceCollection services)
    {
        var secretKey = AuthSecret.GenerateSecretByte();
        services
            .AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
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

        services.AddAuthorization(options =>
        {
            options.AddPolicy(APIRoles.Admin, policy => policy.RequireRole(APIRoles.Admin));
            options.AddPolicy(APIRoles.User, policy => policy.RequireRole(APIRoles.User));
        });
        return services;
    }
}