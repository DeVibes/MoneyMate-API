// // using AccountyMinAPI.Data;

// namespace AccountyMinAPI.Api;

// public static class PaymentAPI
// {
//     public static async Task<IResult> GetAllPaymentTypes(IPaymentTypeRepository repo)
//     {
//         try
//         {
//             var paymentTypes = await repo.GetAllPaymentTypes();
//             List<GetPaymentDto> paymentsDto = new();
//             foreach (var paymentType in paymentTypes)
//             {
//                 PaymentModel.ToGetPaymentDto(paymentType, out GetPaymentDto dto);
//                 paymentsDto.Add(dto);
//             }
//             return Results.Ok(paymentsDto);
//         }
//         catch (System.Exception ex)
//         {
//             return Results.Problem(ex.Message);
//         }
//     }

//     public static async Task<IResult> InsertPaymentType(CreatePaymentRequest dto, IPaymentTypeRepository repo, IAccountRepository accRepo)
//     {
//         var (isError, errorMsg) = CreatePaymentRequest.ToPaymentModel(dto, out PaymentModel paymentType);
//         if (isError)
//             return Results.BadRequest(errorMsg);
//         RepositporyModel result = await repo.InsertPaymentType(paymentType);
//         if (result.ErrorType is ErrorTypeEnum.PAYMENT_ALEADY_EXISTS)
//             return Results.BadRequest(result.ErrorMessage);
//         if (result.ErrorType is ErrorTypeEnum.ACCOUNT_NOT_FOUND)
//             return Results.BadRequest(result.ErrorMessage);
//         if (result.ErrorType is ErrorTypeEnum.DB_ERROR)
//             return Results.Problem(result.ErrorMessage);
//         var isMappingSuccessfull = PaymentModel.ToPaymentDto(result.Payload, out GetPaymentDto paymentDto);
//         if (isMappingSuccessfull)
//             return Results.Created(paymentDto.Id, paymentDto);
//         return Results.Problem("Something went wrong");
//     }

//     public static async Task<IResult> DeletePaymentType(string id, IPaymentTypeRepository repo)
//     {
//         try
//         {
//             var deleteResult = await repo.DeletePaymentTypeById(id);
//             if (deleteResult)
//                 return Results.Ok($"Payment type deleted - {id}");
//             return Results.BadRequest($"Payment id not found - {id}");
//         }
//         catch (Exception ex)
//         {
//             return Results.Problem(ex.Message);
//         }
//     }
// }