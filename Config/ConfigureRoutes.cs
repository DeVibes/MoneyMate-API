using AccountyMinAPI.Api;
using AccountyMinAPI.Auth;

namespace AccountyMinAPI.Config;

public static class ConfigureRoutes
{
    public static WebApplication UseRoutes(this WebApplication app)
    {
        app.MapGet("/", () => "App is running!");

        // Map /transactions endpoints
        app.MapGet("/transactions/{accountId}/all", TransactionsAPI.GetAllTransactions).RequireAuthorization(APIRoles.User);
        app.MapGet("/transactions/{transactionId}", TransactionsAPI.GetTransactionById).RequireAuthorization(APIRoles.User);
        app.MapGet("/transactions/{accountId}/category", TransactionsAPI.GetMonthlySummary).RequireAuthorization(APIRoles.User);
        app.MapGet("/transactions/{accountId}/yearly", TransactionsAPI.GetYearlySumByMonth).RequireAuthorization(APIRoles.User);
        app.MapGet("/transactions/{accountId}/balance", TransactionsAPI.GetAccountBalance).RequireAuthorization(APIRoles.User);
        app.MapPost("/transactions/{accountId}", TransactionsAPI.CreateTransaction).RequireAuthorization(APIRoles.User);
        app.MapDelete("/transactions/{transactionId}", TransactionsAPI.DeleteTransaction).RequireAuthorization(APIRoles.User);
        app.MapMethods("/transactions/{accountId}/{transactionId}", new[] { "PATCH" }, TransactionsAPI.PatchTransaction).RequireAuthorization(APIRoles.User);

        // Map /accounts endpoints
        app.MapGet("/accounts", AccountAPI.GetAccounts).RequireAuthorization(APIRoles.User);
        app.MapGet("/accounts/{accountId}", AccountAPI.GetAccountById).RequireAuthorization(APIRoles.User);
        app.MapPost("/accounts", AccountAPI.CreateAccount).RequireAuthorization(APIRoles.Admin);
        app.MapPost("/accounts/{accountId}/users", AccountAPI.AssignUserToAccount).RequireAuthorization(APIRoles.Admin);
        app.MapPost("/accounts/{accountId}/payments", AccountAPI.AssignPaymentTypeToAccount).RequireAuthorization(APIRoles.Admin);
        app.MapPost("/accounts/{accountId}/categories", AccountAPI.AddCategoryToAccount).RequireAuthorization(APIRoles.User);
        app.MapMethods("/accounts/{accountId}", new[] { "PATCH" }, AccountAPI.PatchAccountById).RequireAuthorization(APIRoles.User);
        app.MapDelete("/accounts/{accountId}/users", AccountAPI.DeassignUserFromAccount).RequireAuthorization(APIRoles.User);
        app.MapDelete("/accounts/{accountId}/payments", AccountAPI.DeassignPaymentTypeFromAccount).RequireAuthorization(APIRoles.User);
        app.MapDelete("/accounts/{accountId}", AccountAPI.DeleteAccountById).RequireAuthorization(APIRoles.User);

        app.MapGet("/auth", AuthAPI.AuthUser);
        
        return app;
    }
}