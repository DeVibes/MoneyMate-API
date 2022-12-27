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
            var (count, transactions) = await repo.GetAllTransactions(filters);
            var transactionsDto = new List<TransactionGetDto>();
            foreach (var transaction in transactions)
            {
                TransactionModel.ToTransactionGetDto(transaction, out TransactionGetDto dto);
                transactionsDto.Add(dto);
            }
            return Results.Ok(new
            {
                itemsCount = count,
                page = filters.PageNumber,
                transactions = transactionsDto
            });
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

    public static async Task<IResult> GetTransaction(string id, ITransactionRepository repo)
    {
        try
        {
            var transaction = await repo.GetTransactionById(id);
            if (transaction is null)
                return Results.NotFound($"ID - {id} was not found");
            TransactionModel.ToTransactionGetDto(transaction, out TransactionGetDto dto);
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

    public static async Task<IResult> InsertTransaction(TransactionPostDto transactionDto,
        ITransactionRepository repo, ICategoryRepository catRepo, IPaymentTypeRepository paymentRepo)
    {
        try
        {
            var existingCategories = await catRepo.GetAllCategories();
            var existingPaymentTypes = await paymentRepo.GetAllPaymentTypes();
            var mappingResponse = TransactionPostDto.ToTransactionModel(transactionDto,
                existingCategories, existingPaymentTypes, out TransactionModel transaction);
            switch (mappingResponse)
            {
                case MappingResponse.OK:
                    await repo.InsertTransaction(transaction);
                    return Results.NoContent();
                case MappingResponse.MISSING_PRICE:
                    return Results.BadRequest("Missing price");
                case MappingResponse.MISSING_DATE:
                    return Results.BadRequest("Missing date");
                case MappingResponse.MISSING_CATEGORY:
                    return Results.BadRequest("Missing category");
                case MappingResponse.MISSING_PAYMENT:
                    return Results.BadRequest("Missing payment");
                case MappingResponse.CATEGORY_NOT_EXISTS:
                    return Results.BadRequest(new
                    {
                        Message = "Invalid category, existing categories:",
                        Types = existingCategories.Select(cat => cat.Name)
                    });
                case MappingResponse.PAYMENT_NOT_EXISTS:
                    return Results.BadRequest(new
                    {
                        Message = "Invalid payment type, existing types:",
                        Types = existingPaymentTypes.Select(type => type.Name)
                    });
                case MappingResponse.INVALID_DATE:
                    return Results.BadRequest("Invalid date");
                default:
                    return Results.BadRequest("Something went wrong");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> DeleteTransaction(string id, ITransactionRepository repo)
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

    public static async Task<IResult> UpdateTransaction(string id, TransactionPostDto transactionDto, ITransactionRepository repo,
        ICategoryRepository catRepo, IPaymentTypeRepository paymentRepo
    )
    {
        // try
        // {
        //     var existingCategories = await catRepo.GetAllCategories();
        //     var existingPaymentTypes = await paymentRepo.GetAllPaymentTypes();
        //     var mappingResponse = TransactionPostDto.ToTransactionModel(transactionDto, 
        //         existingCategories, existingPaymentTypes, out TransactionModel transaction);
        //     switch (mappingResponse)
        //     {
        //         case MappingResponse.OK:
        //             await repo.UpdateTransactionById(id, transaction);
        //             return Results.NoContent();
        //         case MappingResponse.MISSING_PRICE:
        //             return Results.BadRequest("Missing price");
        //         case MappingResponse.MISSING_DATE:
        //             return Results.BadRequest("Missing date");
        //         case MappingResponse.MISSING_CATEGORY:
        //             return Results.BadRequest("Missing category");
        //         case MappingResponse.MISSING_PAYMENT:
        //             return Results.BadRequest("Missing payment");
        //         case MappingResponse.CATEGORY_NOT_EXISTS:
        //             return Results.BadRequest(new 
        //             {
        //                 Message = "Invalid category, existing categories:",
        //                 Types = existingCategories.Select(cat => cat.Name)
        //             });
        //         case MappingResponse.PAYMENT_NOT_EXISTS:
        //             return Results.BadRequest(new 
        //             {
        //                 Message = "Invalid payment type, existing types:",
        //                 Types = existingPaymentTypes.Select(type => type.Name)
        //             });
        //         case MappingResponse.INVALID_DATE:
        //             return Results.BadRequest("Invalid date");
        //         default:
        //             return Results.BadRequest("Something went wrong");
        //     }
        // }
        // catch (Exception ex)
        // {
        //     return Results.Problem(ex.Message);
        // }
        return Results.Problem("Not supporeted yet");
    }

    public static async Task<IResult> PatchTransaction(string id, bool seen, ITransactionRepository transactionRepository)
    {
        //TODO implement rest of the PATCH
        try
        {
            await transactionRepository.PatchSeenStatus(id, seen);
            return Results.Ok();
        }
        catch (NotFoundException)
        {
            return Results.NotFound($"ID - {id} was not found");
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
        //     try
        //     {
        //         var (mappingResult, mappingErrorIfExists) = TransactionPatchDto.ToTransactionModel(transactionDto, out TransactionModel transaction);
        //         if (mappingResult)
        //         {
        //             var categoryId = transaction.CategoryId;
        //             if (categoryId is not null)
        //             {
        //                 var existingCategoriesId = await catRepo.GetAllCategories();
        //                 // if (!existingCategoriesId.ToList().Exists(cat => cat.Item1 == categoryId ))
        //                 //     return MissingCategoryBadRequest;
        //             }
        //             var paymentTypeId = transaction.PaymentTypeId;
        //             if (paymentTypeId is not null)
        //             {
        //                 var existingPaymentsTypeId = await payRepo.GetAllPaymentTypes();
        //                 // if (!existingPaymentsTypeId.ToList().Exists(paymentType => paymentType.Item1 == paymentTypeId ))
        //                 //     return MissingPaymentTypeBadRequest;
        //             }
        //             await repo.PatchTransaction(id, transaction);
        //             return Results.NoContent();
        //         }
        //         else
        //             return Results.BadRequest(mappingErrorIfExists);
        //     }
        //     catch (NotFoundException)
        //     {
        //         return Results.NotFound($"ID - {id} was not found");
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return Results.Problem(ex.Message);
        //     }
    }
    
    public static async Task<IResult> GetMonthlyByCategory(ITransactionRepository repo, HttpRequest request)
    {
        try
        {
            var filters = TransactionsFilters.ReadFiltersFromQuery(request);
            var result = await repo.GetMonthlyByCategory(filters);
            return Results.Ok(result);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> GetYearlySumByMonth(ITransactionRepository repo, HttpRequest request)
    {
        try
        {
            var filters = TransactionsFilters.ReadFiltersFromQuery(request);
            var result = await repo.GetYearlySumByMonth(filters);
            return Results.Ok(result);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    
    public static async Task<IResult> GetBalance(ITransactionRepository repo, HttpRequest request)
    {
        try
        {
            var filters = TransactionsFilters.ReadFiltersFromQuery(request);
            BalanceModel monthlyBalance = await repo.GetMonthlyBalance(filters);
            BalanceModel.ToBalanceDto(monthlyBalance, out BalanceDto balanceDto);
            return Results.Ok(balanceDto);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}