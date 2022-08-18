using System.Data.SqlClient;
using AccountyMinAPI.Data;
using AccountyMinAPI.DB;

namespace AccountyMinAPI.Api;

public static class TransactionsAPI
{
    private static IResult MissingCategoryBadRequest => Results.BadRequest("Wrong category");
    private static IResult MissingPaymentTypeBadRequest => Results.BadRequest("Wrong payment type");
    public static async Task<IResult> GetAllTransactions(ITransactionRepository repo, HttpRequest request)
    {
        try
        {
            var filters = TransactionsFilters.ReadFiltersFromQuery(request);
            var transactions = await repo.GetAllTransactions(filters);
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
        catch (NotFoundException)
        {
            return Results.NotFound($"ID - {id} was not found");
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> InsertTransaction(TransactionCreateDto transactionDto, 
        ITransactionRepository repo, ICategoryRepository catRepo, IPaymentTypeRepository payRepo
    )
    {
        try
        {
            var (isMappingSuccessful ,mappingErrorIfExists) = TransactionCreateDto.ToTransactionModel(transactionDto, out TransactionModel transaction);
            if (isMappingSuccessful)
            {
                var categoryId = transaction.CategoryId;
                var existingCategoriesId = await catRepo.GetAllCategories();
                if (!existingCategoriesId.ToList().Exists(cat => cat.Item1 == categoryId ))
                    return MissingCategoryBadRequest;
                var paymentTypeId = transaction.PaymentTypeId;
                var existingPaymentsTypeId = await payRepo.GetAllPaymentTypes();
                if (!existingPaymentsTypeId.ToList().Exists(paymentType => paymentType.Item1 == paymentTypeId ))
                    return MissingPaymentTypeBadRequest;
                await repo.InsertTransaction(transaction);
                return Results.NoContent();
            }
            else
                return Results.BadRequest(mappingErrorIfExists);
        }
        catch (Exception ex)
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

    public static async Task<IResult> UpdateTransaction(int id, TransactionCreateDto transactionDto, ITransactionRepository repo,
        ICategoryRepository catRepo, IPaymentTypeRepository payRepo
    )
    {
        try
        {
            var (isMappingSuccessful, mappingErrorIfExists) = TransactionCreateDto.ToTransactionModel(transactionDto, out TransactionModel transaction);
            if (isMappingSuccessful)
            {
                var categoryId = transaction.CategoryId;
                var existingCategoriesId = await catRepo.GetAllCategories();
                if (!existingCategoriesId.ToList().Exists(cat => cat.Item1 == categoryId ))
                    return MissingCategoryBadRequest;
                var paymentTypeId = transaction.PaymentTypeId;
                var existingPaymentsTypeId = await payRepo.GetAllPaymentTypes();
                if (!existingPaymentsTypeId.ToList().Exists(paymentType => paymentType.Item1 == paymentTypeId ))
                    return MissingPaymentTypeBadRequest;
                await repo.UpdateTransactionById(id, transaction);
                return Results.NoContent();
            }
            else
                return Results.BadRequest(mappingErrorIfExists);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> PatchTransaction(int id, TransactionPatchDto transactionDto, ITransactionRepository repo, 
        ICategoryRepository catRepo, IPaymentTypeRepository payRepo
    )
    {
        try
        {
            var (mappingResult, mappingErrorIfExists) = TransactionPatchDto.ToTransactionModel(transactionDto, out TransactionModel transaction);
            if (mappingResult)
            {
                var categoryId = transaction.CategoryId;
                if (categoryId is not null)
                {
                    var existingCategoriesId = await catRepo.GetAllCategories();
                    if (!existingCategoriesId.ToList().Exists(cat => cat.Item1 == categoryId ))
                        return MissingCategoryBadRequest;
                }
                var paymentTypeId = transaction.PaymentTypeId;
                if (paymentTypeId is not null)
                {
                    var existingPaymentsTypeId = await payRepo.GetAllPaymentTypes();
                    if (!existingPaymentsTypeId.ToList().Exists(paymentType => paymentType.Item1 == paymentTypeId ))
                        return MissingPaymentTypeBadRequest;
                }
                await repo.PatchTransaction(id, transaction);
                return Results.NoContent();
            }
            else
                return Results.BadRequest(mappingErrorIfExists);
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
}