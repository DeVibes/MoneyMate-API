using AccountyMinAPI.Data;

namespace AccountyMinAPI.Api;

public static class PaymentAPI
{
    public static async Task<IResult> GetAllPaymentTypes(IPaymentTypeRepository repo)
    {
        try
        {
            var paymentTypes = await repo.GetAllPaymentTypes();
            List<PaymentGetDto> paymentsDto = new();
            foreach (var paymentType in paymentTypes)
            {
                PaymentModel.ToPaymentGetDto(paymentType, out PaymentGetDto dto);
                paymentsDto.Add(dto);
            }
            return Results.Ok(paymentsDto);
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> InsertPaymentType(PaymentPostDto dto, IPaymentTypeRepository repo)
    {
        try
        {
            var mappingResponse = PaymentPostDto.ToPaymentModel(dto, out PaymentModel paymentType);
            switch (mappingResponse)
            {
                case MappingResponse.OK:
                    await repo.InsertPaymentType(paymentType);
                    return Results.Ok($"Payment type added - {paymentType.Name}");
                case MappingResponse.MISSING_PAYMENT:
                    return Results.BadRequest("Missing payment");
                default:
                    return Results.Problem("Something went wrong");
            }
        }
        catch (PaymentAlreadyExistsException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> DeletePaymentType(string id, IPaymentTypeRepository repo)
    {
        try
        {
            var deleteResult = await repo.DeletePaymentTypeById(id);
            if (deleteResult)
                return Results.Ok($"Payment type deleted - {id}");
            return Results.BadRequest($"Payment id not found - {id}");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}