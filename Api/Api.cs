using System.Net;
using System.Text.Json;
using AccountyMinAPI.Auth;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;

namespace AccountyMinAPI.Api;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        // Map /transactions endpoints
        app.MapGet("/", () => "App is running!");
        app.MapGet("/transactions", TransactionsAPI.GetAllTransactions).RequireAuthorization(APIRoles.User);
        app.MapGet("/transactions/{id}", TransactionsAPI.GetTransaction).RequireAuthorization(APIRoles.User);
        app.MapGet("/transactions/category", TransactionsAPI.GetMonthlyByCategory).RequireAuthorization(APIRoles.User);
        app.MapGet("/transactions/yearly", TransactionsAPI.GetYearlySumByMonth).RequireAuthorization(APIRoles.User);
        app.MapGet("/transactions/balance", TransactionsAPI.GetBalance).RequireAuthorization(APIRoles.User);
        app.MapPost("/transactions", TransactionsAPI.InsertTransaction).RequireAuthorization(APIRoles.User);
        app.MapDelete("/transactions/{id}", TransactionsAPI.DeleteTransaction).RequireAuthorization(APIRoles.User);
        app.MapPut("/transactions/{id}", TransactionsAPI.UpdateTransaction).RequireAuthorization(APIRoles.User);
        app.MapMethods("/transactions/{id}", new[] { "PATCH" }, TransactionsAPI.PatchTransaction).RequireAuthorization(APIRoles.User);

        // Map /categories endpoints
        app.MapGet("/categories", CategoryAPI.GetAllCategories).RequireAuthorization(APIRoles.User);
        app.MapPost("/categories", CategoryAPI.InsertCategory).RequireAuthorization(APIRoles.User);
        app.MapDelete("/categories/{id}", CategoryAPI.DeleteCategory).RequireAuthorization(APIRoles.User);

        // Map /payments endpoints
        app.MapGet("/payments", PaymentAPI.GetAllPaymentTypes).RequireAuthorization(APIRoles.User);
        app.MapPost("/payments", PaymentAPI.InsertPaymentType).RequireAuthorization(APIRoles.User);
        app.MapDelete("/payments/{id}", PaymentAPI.DeletePaymentType).RequireAuthorization(APIRoles.User);

        app.MapGet("/auth", AuthUser);
    }

    private static async Task<IResult> AuthUser(HttpRequest request, TokenService service, IUsernameRepository repo)
    {
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
        var isUsernameAllowed = await repo.IsUsernameAllowed(username);
        if (String.IsNullOrEmpty(username) || !isUsernameAllowed)
            return Results.Unauthorized();
        var token = service.GenerateToken(username);
        return Results.Ok(new { token = token });
    }
}