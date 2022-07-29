namespace AccountyMinAPI;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        // Map all API endpoints
        app.MapGet("/transactions", GetAllTransactions);
        app.MapPost("/transactions", InsertTransaction);
    }

    private static async Task<IResult> GetAllTransactions(ITransactionRepository repo)
    {
        try
        {
            return Results.Ok(await repo.GetAllTransactions());
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertTransaction(TransactionModel transaction, ITransactionRepository repo)
    {
        try
        {
            await repo.InsertTransaction(transaction);
            return Results.Ok();
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}