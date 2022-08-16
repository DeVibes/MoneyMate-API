using AccountyMinAPI.Data;

namespace AccountyMinAPI.Api;

public static class TransactionsAPI
{
    public static async Task<IResult> GetAllTransactions(ITransactionRepository repo)
    {
        try
        {
            var transactions = await repo.GetAllTransactions();
            var transactionsDto = new List<TransactionReadDto>();
            foreach (var transaction in transactions)
            {
                var mappingResult = TransactionModel.ToTransactionReadDto(transaction, out TransactionReadDto dto);
                if (mappingResult.Item1)
                    transactionsDto.Add(dto);
            }
            return Results.Ok(transactionsDto);
        }
        catch (NotFoundException)
        {
            return Results.NotFound("No transactions exists");
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> GetTransaction(int id, ITransactionRepository repo)
    {
        try
        {
            var transaction = await repo.GetTransactionById(id);
            if (transaction is null)
                return Results.NotFound($"ID - {id} was not found");
            var mappingResult = TransactionModel.ToTransactionReadDto(transaction, out TransactionReadDto dto);
            return Results.Ok(dto);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> InsertTransaction(TransactionCreateDto transactionDto, ITransactionRepository repo)
    {
        try
        {
            var mappingResult = TransactionCreateDto.ToTransactionModel(transactionDto, out TransactionModel transaction);
            if (mappingResult.Item1)
            {
                await repo.InsertTransaction(transaction);
                return Results.NoContent();
            }
            else
                return Results.BadRequest(mappingResult.Item2);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> DeleteTransaction(int id, ITransactionRepository repo)
    {
        try
        {
            await repo.DeleteTransactionById(id);
            return Results.NoContent();
        }
        catch (NotFoundException)
        {
            return Results.NotFound($"ID - {id} was not found");
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> UpdateTransaction(int id, TransactionCreateDto transactionDto, ITransactionRepository repo)
    {
        try
        {
            var mappingResult = TransactionCreateDto.ToTransactionModel(transactionDto, out TransactionModel transaction);
            if (mappingResult.Item1)
            {
                await repo.UpdateTransactionById(id, transaction);
                return Results.NoContent();
            }
            else
                return Results.BadRequest(mappingResult.Item2);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> PatchTransaction(int id, TransactionPatchDto transactionDto, ITransactionRepository repo)
    {
        try
        {
            var mappingResult = TransactionPatchDto.ToTransactionModel(transactionDto, out TransactionModel transaction);
            if (mappingResult.Item1)
            {
                await repo.PatchTransaction(id, transaction);
                return Results.NoContent();
            }
            else
                return Results.BadRequest(mappingResult.Item2);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}