namespace AccountyMinAPI.Dtos;

public record TransactionCreateDto    
{
    public string Description { get; init; } = String.Empty;
    public string Store { get; init; } = String.Empty; 
    public double Price { get; init; }
    public string Date { get; init; } = String.Empty;
    public int PaymentTypeId { get; init; }
    public string PaymentTypeName { get; init; } = String.Empty;
    public int CategoryId { get; init; }
    public string CategoryName { get; set; } = String.Empty;
    public bool Seen { get; init; }
    public static (bool, string) ToTransactionModel(TransactionCreateDto dto,  out TransactionModel model)
    {
        model = null;
        var isDateCorrect = DateTime.TryParse(dto.Date, out DateTime date);
        if (dto.Date == String.Empty || !isDateCorrect)
            return (false, $"Missing / wrong date property");
        if (dto.PaymentTypeId == 0)
            return (false, $"Missing Payment Type property");
        if (dto.CategoryId == 0)
            return (false, $"Missing Category property");
        if (dto.Price == 0)
            return (false, $"Missing Price property");
        model = new()
        {
           CategoryId = dto.CategoryId,
           Date = date,
           PaymentTypeId = dto.PaymentTypeId,
           Description = dto.Description,
           Price = dto.Price,
           Seen = dto.Seen,
           Store = dto.Store 
        };
        return (true, String.Empty);
    }
}

public record TransactionPatchDto    
{
    public string? Description { get; init; }
    public string? Store { get; init; }
    public double? Price { get; init; }
    public string? Date { get; init; }
    public int? PaymentTypeId { get; init; }
    public int? CategoryId { get; init; }
    public bool? Seen { get; init; }
    public static (bool, string) ToTransactionModel(TransactionPatchDto dto, out TransactionModel model)
    {
        var isDateCorrect = DateTime.TryParse(dto.Date, out DateTime date);
        model = new()
        {
            CategoryId = dto.CategoryId,
            Date = isDateCorrect ? date : null,
            Description = dto.Description,
            PaymentTypeId = dto.PaymentTypeId,
            Price = dto.Price,
            Seen = dto.Seen,
            Store = dto.Store
        };
        return (true, String.Empty);
    }
}

public record TransactionReadDto    
{
    public int Id { get; init; }
    public string Description { get; init; } = String.Empty;
    public string Store { get; init; } = String.Empty;
    public double Price { get; init; }
    public string? Date { get; set; } = String.Empty;
    public int PaymentTypeId { get; init; } 
    public string PaymentTypeName { get; set; } = String.Empty;
    public int CategoryId { get; init; }
    public string CategoryTypeName { get; set; } = String.Empty;
    public bool Seen { get; init; }
}