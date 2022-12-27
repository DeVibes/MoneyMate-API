using AccountyMinAPI.Auth;

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

        app.MapGet("/auth/{username}", async (string username, TokenService service, IUsernameRepository repo) => 
        {
            var isUsernameAllowed = await repo.IsUsernameAllowed(username);
            if (!isUsernameAllowed)
                return Results.Unauthorized();
            var token = service.GenerateToken(username);
            return Results.Ok(new { token = token });
        });
    }
}
