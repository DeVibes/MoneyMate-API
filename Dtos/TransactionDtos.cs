namespace AccountyMinAPI.Dtos;

public record TransactionPostDto    
{
    public string Description { get; init; } = String.Empty;
    public string Store { get; init; } = String.Empty; 
    public double Price { get; init; }
    public string Date { get; init; } = String.Empty;
    public string CategoryId { get; init; }
    public string PaymentTypeId { get; init; }
    public bool Seen { get; init; }
    public static MappingResponse ToTransactionModel(TransactionPostDto dto, 
        IEnumerable<CategoryModel> existingCategories, IEnumerable<PaymentModel> existingPaymentTypes,
        out TransactionModel model)
    {
        model = null;
        if (dto.Price == 0)
            return MappingResponse.MISSING_PRICE;
        var isDateCorrect = DateTime.TryParse(dto.Date, out DateTime date);
        if (String.IsNullOrEmpty(dto.Date))
            return MappingResponse.MISSING_DATE;
        if (!isDateCorrect)
            return MappingResponse.INVALID_DATE;
        if (String.IsNullOrEmpty(dto.CategoryId))
            return MappingResponse.MISSING_CATEGORY;
        var categoryData = existingCategories.ToList().Find(cat => 
            cat.Id.ToString() == dto.CategoryId);
        if (categoryData == null)
            return MappingResponse.CATEGORY_NOT_EXISTS;
        if (String.IsNullOrEmpty(dto.PaymentTypeId))
            return MappingResponse.MISSING_PAYMENT;
        var paymentTypeData = existingPaymentTypes.ToList().Find(type => 
            type.Id.ToString() == dto.PaymentTypeId);
        if (paymentTypeData == null)
            return MappingResponse.PAYMENT_NOT_EXISTS;
        model = new()
        {
           Category = categoryData,
           Date = date,
           Payment = paymentTypeData,
           Description = dto.Description,
           Price = dto.Price,
           Seen = dto.Seen,
           Store = dto.Store 
        };
        return MappingResponse.OK;
    }
}

public record TransactionPatchDto    
{
    public string? Description { get; init; }
    public string? Store { get; init; }
    public double? Price { get; init; }
    public string? Date { get; init; }
    public string PaymentTypeName { get; init; }
    public string CategoryName { get; init; }
    public bool? Seen { get; init; }
    public static (bool, string) ToTransactionModel(TransactionPatchDto dto, out TransactionModel model)
    {
        var isDateCorrect = DateTime.TryParse(dto.Date, out DateTime date);
        model = null;
        // model = new()
        // {
        //     CategoryId = dto.CategoryId,
        //     Date = isDateCorrect ? date : null,
        //     Description = dto.Description,
        //     PaymentTypeId = dto.PaymentTypeId,
        //     Price = dto.Price,
        //     Seen = dto.Seen,
        //     Store = dto.Store
        // };
        return (true, String.Empty);
    }
}

public record TransactionGetDto    
{
    public string Id { get; init; }
    public string Description { get; init; } = String.Empty;
    public string Store { get; init; } = String.Empty;
    public double Price { get; init; }
    public string Date { get; set; } = String.Empty;
    public string PaymentTypeId { get; init; } 
    public string CategoryId { get; init; }
    public bool Seen { get; init; }
}