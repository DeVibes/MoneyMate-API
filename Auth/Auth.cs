using System.Text;
using Microsoft.Extensions.Primitives;

namespace AccountyMinAPI.Auth;
public static class Auth
{
    internal static string SecretKey = "6ceccd7405ef4b00b2630009be568cfa";
    internal static byte[] GenerateSecretByte() => 
        Encoding.ASCII.GetBytes(SecretKey);
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