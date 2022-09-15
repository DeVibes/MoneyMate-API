using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountyMinAPI.Models
{
    public record TransactionModel    
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string? Description { get; init; }
        public string? Store { get; init; } 
        public double? Price { get; init; }
        // [BsonDateTimeOptions(Kind = DateTimeKind.Utc, Representation = BsonType.DateTime)]
        public DateTime? Date { get; init; } 
        public int? PaymentTypeId { get; init; } 
        public int? CategoryId { get; init; }
        public bool? Seen { get; init; }
        public static (bool, string) ToTransactionReadDto(TransactionModel model, out TransactionReadDto dto)
        {
            dto = null;
            if (!model.CategoryId.HasValue)
                return (false, "No category");
            if (!model.Date.HasValue)
                return (false, "No date");
            if (!model.PaymentTypeId.HasValue)
                return (false, "No payment type");
            if (!model.Date.HasValue)
                return (false, "No date");
            if (!model.Price.HasValue)
                return (false, "No price");
            if (!model.Seen.HasValue)
                return (false, "No seen data");
            dto = new()
            {
                Id = model.Id.ToString(),
                CategoryId = model.CategoryId.Value,
                Date = model.Date.Value.ToString("o"),
                Description = model.Description ?? String.Empty,
                PaymentTypeId = model.PaymentTypeId.Value,
                Price = model.Price.Value,
                Seen = model.Seen.Value,
                Store = model.Store ?? String.Empty
            };
            return (true, String.Empty);
        }
    }
}