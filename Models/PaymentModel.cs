using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountyMinAPI.Models;
public record PaymentModel
{
    public ObjectId Id { get; init; }
    public string Name { get; init; } = String.Empty;
    public string LinkedAccountId { get; init; } = String.Empty;

    public static bool ToPaymentDto(PaymentModel model, out GetPaymentDto dto)
    {
        dto = null;
        if (model is null)
            return false;
        dto = new()
        {
            Id = model.Id.ToString(),
            Name = model.Name,
            LinkedAccountId = model.LinkedAccountId
        };
        return true;
    }
}