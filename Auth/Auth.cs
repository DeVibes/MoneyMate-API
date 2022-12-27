using System.Text;

namespace AccountyMinAPI.Auth;
internal static class Auth
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