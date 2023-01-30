
using System.Net;
using AccountyMinAPI.Auth;
using Newtonsoft.Json.Linq;

namespace AccountyMinAPI.Api;

public static class AuthAPI
{
    public static async Task<IResult> AuthUser(
        HttpRequest request, 
        TokenService service, 
        IConfiguration config,
        IUsernameRepository usernameRepository,
        IAccountRepository accountRepository)
    {
        string secret = config["AppSecret"];
        string? gToken = RequestHelper.GetBearerToken(request);
        string requestUrl = $"https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={gToken}";
        string username = String.Empty;
        using (HttpClient client = new())
        {
            HttpResponseMessage response = await client.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseMsg = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(responseMsg);
                username = (string)obj["email"];
            }
        }
        var isUsernameAllowed = await usernameRepository.IsUsernameAllowed(username);
        if (String.IsNullOrEmpty(username) || !isUsernameAllowed)
            return Results.Unauthorized();
        var token = service.GenerateToken(username, secret);
        IEnumerable<string> userAccounts = await accountRepository.GetUserAccounts(username);
        return Results.Ok(new { token = token, accounts = userAccounts });
    }
} 