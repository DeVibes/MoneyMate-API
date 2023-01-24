using AccountyMinAPI.Api;
namespace AccountyMinAPI.Config;

public static class ConfigureRoutes
{
    public static WebApplication UseRoutes(this WebApplication app)
    {
        app.MapGet("/", () => "App is running!");

        // Map /transactions endpoints
        app.MapGet("/transactions/{accountId}/all", TransactionsAPI.GetAllTransactions);
        app.MapGet("/transactions/{transactionId}", TransactionsAPI.GetTransactionById);
        app.MapGet("/transactions/{accountId}/category", TransactionsAPI.GetMonthlySummary);
        app.MapGet("/transactions/{accountId}/yearly", TransactionsAPI.GetYearlySumByMonth);
        app.MapGet("/transactions/{accountId}/balance", TransactionsAPI.GetAccountBalance);
        app.MapPost("/transactions/{accountId}", TransactionsAPI.CreateTransaction);
        app.MapDelete("/transactions/{transactionId}", TransactionsAPI.DeleteTransaction);
        app.MapMethods("/transactions/{accountId}/{transactionId}", new[] { "PATCH" }, TransactionsAPI.PatchTransaction);

        // Map /accounts endpoints
        app.MapGet("/accounts", AccountAPI.GetAccounts);
        app.MapGet("/accounts/{accountId}", AccountAPI.GetAccountById);
        app.MapPost("/accounts", AccountAPI.CreateAccount);
        app.MapPost("/accounts/{accountId}/users", AccountAPI.AssignUserToAccount);
        app.MapPost("/accounts/{accountId}/payments", AccountAPI.AssignPaymentTypeToAccount);
        app.MapPost("/accounts/{accountId}/categories", AccountAPI.AddCategoryToAccount);
        app.MapMethods("/accounts/{accountId}", new[] { "PATCH" }, AccountAPI.PatchAccountById);
        app.MapDelete("/accounts/{accountId}/users", AccountAPI.DeassignUserFromAccount);
        app.MapDelete("/accounts/{accountId}/payments", AccountAPI.DeassignPaymentTypeFromAccount);
        app.MapDelete("/accounts/{accountId}", AccountAPI.DeleteAccountById);

        app.MapGet("/auth", AuthAPI.AuthUser);
        
        return app;
    }
}