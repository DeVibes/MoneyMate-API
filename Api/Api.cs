namespace AccountyMinAPI.Api;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        // Map /transactions endpoints
        app.MapGet("/", () => "App is running!");
        app.MapGet("/transactions", TransactionsAPI.GetAllTransactions);
        app.MapGet("/transactions/{id}", TransactionsAPI. GetTransaction);
        app.MapPost("/transactions", TransactionsAPI.InsertTransaction);
        app.MapDelete("/transactions/{id}", TransactionsAPI.DeleteTransaction);
        app.MapPut("/transactions/{id}", TransactionsAPI.UpdateTransaction);
        app.MapMethods("/transactions/{id}", new[] { "PATCH" }, TransactionsAPI.PatchTransaction);
    }
}