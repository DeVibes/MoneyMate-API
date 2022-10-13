using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountyMinAPI.Models;
public record PaymentModel
{
    public ObjectId Id { get; init; }
    public string Name { get; init; } = String.Empty;

    //TODO Handle mapping errors from DB
    public static (bool, string) ToPaymentGetDto(PaymentModel model, out PaymentGetDto dto)
    {
        dto = new()
        {
            Id = model.Id.ToString(),
            Name = model.Name
        };
        return (true, String.Empty);
    }
}