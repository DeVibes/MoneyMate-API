namespace AccountyMinAPI.Dtos;

public record PaymentGetDto
{
    public string Id { get; init; }
    public string Name { get; init; } = String.Empty;
}

public record PaymentPostDto
{
    public string Name { get; init; } = String.Empty;
    public static MappingResponse ToPaymentModel(PaymentPostDto dto, out PaymentModel model)
    {
        model = null;
        if (String.IsNullOrEmpty(dto.Name))
            return MappingResponse.MISSING_PAYMENT;
        model = new() { Name = dto.Name };
        return MappingResponse.OK;
    }
}