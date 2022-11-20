namespace AccountyMinAPI.Api;

public static class BalanceAPI
{
    public static async Task<IResult> GetBalance(ITransactionRepository repo, HttpRequest request)
    {
        try
        {
            var filters = TransactionsFilters.ReadFiltersFromQuery(request);
            BalanceModel monthlyBalance = await repo.GetMonthlyBalance(filters);
            return Results.Ok(monthlyBalance);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}