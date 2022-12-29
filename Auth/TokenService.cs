using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AccountyMinAPI.Auth;
internal class TokenService
{
    internal string GenerateToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Auth.GenerateSecretByte();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, APIRoles.User)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}