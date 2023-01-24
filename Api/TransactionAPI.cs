using Microsoft.AspNetCore.Mvc;

namespace AccountyMinAPI.Api;

public static class TransactionsAPI
{
    public static async Task<IResult> GetAllTransactions(
        string accountId,
        ITransactionRepository transactionRepository, 
        IAccountRepository accountRepository, 
        HttpRequest request)
    {
        try
        {
            if (String.IsNullOrEmpty(accountId))
                throw new RequestException("Account id missing");
            IEnumerable<string> accountUsers = await accountRepository.GetAccountUsers(accountId);
            TransactionsFilters filters = TransactionsFilters.ReadFiltersFromQuery(request);
            filters.Users = accountUsers;
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
        catch (RequestException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> GetTransactionById(
        string transactionId, 
        ITransactionRepository transactionRepository)
    {
        try
        {
            TransactionModel transaction = await transactionRepository.GetTransactionById(transactionId);
            TransactionResponse payload = TransactionModel.ToTransactionResponse(transaction);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> CreateTransaction(
        string accountId, 
        [FromBody]TransactionRequest requestBody, 
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        try
        {
            TransactionModel model = TransactionRequest.ToTransactionModelCreate(requestBody);
            await accountRepository.ValidateTransactionData(accountId, model);
            TransactionModel createdTransaction = await transactionRepository.InsertTransaction(model);
            TransactionResponse payload = TransactionModel.ToTransactionResponse(createdTransaction);
            return Results.Created(payload.Id, payload);
        }
        catch (UserException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> DeleteTransaction(
        string transactionId, 
        ITransactionRepository transactionRepository)
    {
        try
        {
            await transactionRepository.DeleteTransactionById(transactionId);
            return Results.NoContent();
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> PatchTransaction(
        string accountId, 
        string transactionId, 
        TransactionRequest request,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository)
    {
        try
        {
            TransactionModel model = TransactionRequest.ToTransactionModelPatch(request);
            await accountRepository.ValidateTransactionData(accountId, model);
            TransactionModel updatedTransaction = await transactionRepository.EditTransaction(transactionId, model);
            TransactionResponse payload = TransactionModel.ToTransactionResponse(updatedTransaction);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }
    
    public static async Task<IResult> GetMonthlySummary(
        string accountId,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository, 
        HttpRequest request)
    {
        try
        {
            if (String.IsNullOrEmpty(accountId))
                throw new RequestException("Account id missing");
            IEnumerable<string> accountUsers = await accountRepository.GetAccountUsers(accountId);
            TransactionsFilters filters = TransactionsFilters.ReadFiltersFromQuery(request);
            filters.Users = accountUsers;
            IEnumerable<MonthlyCategorySummaryModel> monthSummary = await transactionRepository.GetMonthlySummary(filters);
            IEnumerable<MonthlyCategorySummaryResponse> payload = monthSummary.Select(MonthlyCategorySummaryModel.ToResponse);
            return Results.Ok(payload);
        }
        catch (RequestException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> GetYearlySumByMonth(
        string accountId,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository, 
        HttpRequest request)
    {
        try
        {
            if (String.IsNullOrEmpty(accountId))
                throw new RequestException("Account id missing");
            IEnumerable<string> accountUsers = await accountRepository.GetAccountUsers(accountId);
            TransactionsFilters filters = TransactionsFilters.ReadFiltersFromQuery(request);
            filters.Users = accountUsers;
            IEnumerable<YearlySummaryModel> yearlySummary = await transactionRepository.GetYearlySumByMonth(filters);
            IEnumerable<YearlySummaryResponse> payload = yearlySummary.Select(YearlySummaryModel.ToResponse);
            return Results.Ok(payload);
        }
        catch (RequestException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }
    
    public static async Task<IResult> GetAccountBalance(
        string accountId,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository, 
        HttpRequest request)
    {
        try
        {
            if (String.IsNullOrEmpty(accountId))
                throw new RequestException("Account id missing");
            IEnumerable<string> accountUsers = await accountRepository.GetAccountUsers(accountId);
            TransactionsFilters filters = TransactionsFilters.ReadFiltersFromQuery(request);
            filters.Users = accountUsers;
            BalanceModel monthlyBalance = await transactionRepository.GetMonthlyBalance(filters);
            BalanceResponse payload = BalanceModel.ToResponse(monthlyBalance);
            return Results.Ok(payload);
        }
        catch (RequestException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }
}