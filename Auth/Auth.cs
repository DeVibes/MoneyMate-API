using System.Text;
using Microsoft.Extensions.Primitives;

namespace AccountyMinAPI.Auth;
public static class AuthSecret
{
    public static byte[] GenerateSecretByte(string secretKey) => 
        Encoding.ASCII.GetBytes(secretKey);
}

public struct APIRoles
{
    public static readonly string Admin = "admin";
    public static readonly string User = "user";
}

public static class RequestHelper
{
    public static string GetBearerToken(HttpRequest request)
    {
        if (request.Headers.TryGetValue("Authorization", out StringValues values))
        {
            string authorizationHeader = values.FirstOrDefault();
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return authorizationHeader.Substring("Bearer ".Length).Trim();
        }
        return String.Empty;
    }
}