namespace AccountyMinAPI.Api;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        // Map /transactions endpoints
        app.MapGet("/", () => "App is running!");
        app.MapGet("/transactions", TransactionsAPI.GetAllTransactions);
        app.MapGet("/transactions/{id}", TransactionsAPI.GetTransaction);
        app.MapGet("/transactions/category", TransactionsAPI.GetMonthlyByCategory);
        app.MapGet("/transactions/yearly", TransactionsAPI.GetYearlySumByMonth);
        app.MapGet("/transactions/balance", TransactionsAPI.GetBalance);
        app.MapPost("/transactions", TransactionsAPI.InsertTransaction);
        app.MapDelete("/transactions/{id}", TransactionsAPI.DeleteTransaction);
        app.MapPut("/transactions/{id}", TransactionsAPI.UpdateTransaction);
        app.MapMethods("/transactions/{id}", new[] { "PATCH" }, TransactionsAPI.PatchTransaction);

        // Map /categories endpoints
        app.MapGet("/categories", CategoryAPI.GetAllCategories);
        app.MapPost("/categories", CategoryAPI.InsertCategory);
        app.MapDelete("/categories/{id}", CategoryAPI.DeleteCategory);

        // Map /payments endpoints
        app.MapGet("/payments", PaymentAPI.GetAllPaymentTypes);
        app.MapPost("/payments", PaymentAPI.InsertPaymentType);
        app.MapDelete("/payments/{id}", PaymentAPI.DeletePaymentType);
    }
}
