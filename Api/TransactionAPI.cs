using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccountyMinAPI.Api;

public static class TransactionsAPI
{
    public static async Task<IResult> GetAllTransactions(
        string accountId,
        ITransactionRepository transactionRepository, 
        IAccountRepository accountRepository, 
        HttpRequest request)
    {
        if (String.IsNullOrEmpty(accountId))
            throw new RequestException("Account id missing");
        IEnumerable<UserModel> accountUsers = await accountRepository.GetAccountUsers(accountId);
        TransactionsFilters filters = TransactionsFilters.ReadFiltersFromQuery(request);
        filters.Users = accountUsers.Select(a => a.Username);
        var (count, transactions) = await transactionRepository.GetAllTransactions(filters);
        List<TransactionResponse> transactionsPayload = transactions
            .Select(TransactionModel.ToTransactionResponse)
            .ToList();
        return Results.Ok(new
        {
            itemsCount = count,
            page = filters.PageNumber,
            transactions = transactionsPayload
        });
    }
    public static async Task<IResult> GetTransactionById(
        string transactionId, 
        ITransactionRepository transactionRepository)
    {
        TransactionModel transaction = await transactionRepository.GetTransactionById(transactionId);
        TransactionResponse payload = TransactionModel.ToTransactionResponse(transaction);
        return Results.Ok(payload);
    }

    public static async Task<IResult> CreateTransaction(
        string accountId, 
        [FromBody]TransactionRequest requestBody, 
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        TransactionModel model = TransactionRequest.ToTransactionModelCreate(requestBody);
        await accountRepository.ValidateTransactionData(accountId, model);
        TransactionModel createdTransaction = await transactionRepository.InsertTransaction(model);
        TransactionResponse payload = TransactionModel.ToTransactionResponse(createdTransaction);
        return Results.Created(payload.Id, payload);
    }

    public static async Task<IResult> DeleteTransaction(
        string transactionId, 
        ITransactionRepository transactionRepository)
    {
        await transactionRepository.DeleteTransactionById(transactionId);
        return Results.NoContent();
    }

    public static async Task<IResult> PatchTransaction(
        string accountId, 
        string transactionId, 
        TransactionRequest request,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository)
    {
        TransactionModel model = TransactionRequest.ToTransactionModelPatch(request);
        await accountRepository.ValidateTransactionData(accountId, model);
        TransactionModel updatedTransaction = await transactionRepository.EditTransaction(transactionId, model);
        TransactionResponse payload = TransactionModel.ToTransactionResponse(updatedTransaction);
        return Results.Ok(payload);
    }
    
    public static async Task<IResult> GetMonthlySummary(
        string accountId,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository, 
        HttpRequest request)
    {
        if (String.IsNullOrEmpty(accountId))
            throw new RequestException("Account id missing");
        IEnumerable<UserModel> accountUsers = await accountRepository.GetAccountUsers(accountId);
        TransactionsFilters filters = TransactionsFilters.ReadFiltersFromQuery(request);
        filters.Users = accountUsers.Select(a => a.Username);
        IEnumerable<MonthlyCategorySummaryModel> monthSummary = await transactionRepository.GetMonthlySummary(filters);
        var result = monthSummary.ToLookup(categoryDetail => categoryDetail.Category == "Income");
        BalanceResponse payload = new()
        {
            CategoriesDetails = monthSummary.Where(x => x.Category != "Income").ToList(),
            Income = result[true].Sum(x => x.Total),
            Outcome = result[false].Sum(x => x.Total)
        };
        string payloadJson = JsonConvert.SerializeObject(payload);
        return Results.Ok(payloadJson);
    }

    public static async Task<IResult> GetYearlySumByMonth(
        string accountId,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository, 
        HttpRequest request)
    {
        if (String.IsNullOrEmpty(accountId))
            throw new RequestException("Account id missing");
        IEnumerable<UserModel> accountUsers = await accountRepository.GetAccountUsers(accountId);
        TransactionsFilters filters = TransactionsFilters.ReadFiltersFromQuery(request);
        filters.Users = accountUsers.Select(a => a.Username);
        IEnumerable<YearlySummaryModel> yearlySummary = await transactionRepository.GetYearlySumByMonth(filters);
        IEnumerable<YearlySummaryResponse> payload = yearlySummary.Select(YearlySummaryModel.ToResponse);
        return Results.Ok(payload);
    }
}