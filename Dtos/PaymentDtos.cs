namespace AccountyMinAPI.Dtos;

public record GetPaymentDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string LinkedAccountId { get; init; }
}

public record CreatePaymentRequest
{
    public string Name { get; init; } = String.Empty;
    public string LinkedAccountId { get; init; } = String.Empty;
    public static (bool, string) ToPaymentModel(CreatePaymentRequest dto, out PaymentModel model)
    {
        model = null;
        if (String.IsNullOrEmpty(dto.Name))
            return (true, "Missing payment name");
        if (String.IsNullOrEmpty(dto.LinkedAccountId))
            return (true, "Missing assosiated account id");
        model = new() 
        { 
            Name = dto.Name,
            LinkedAccountId = dto.LinkedAccountId       
        };
        return (false, String.Empty);

    }
}