using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountyMinAPI.Models
{
    public record TransactionModel    
    {
        public ObjectId Id { get; init; }
        public string Description { get; init; } = String.Empty;
        public string Store { get; init; } = String.Empty;
        public double Price { get; init; }
        public DateTime Date { get; init; } 
        public PaymentModel Payment { get; init; } 
        public CategoryModel Category { get; init; }
        public bool Seen { get; init; }
        
        //TODO Handle mapping errors from DB
        public static void ToTransactionGetDto(TransactionModel model, out TransactionGetDto dto)
        {
            dto = new()
            {
                Id = model.Id.ToString(),
                CategoryName = model.Category.Name,
                Date = model.Date.ToString("o"),
                Description = model.Description,
                PaymentTypeName = model.Payment.Name,
                Price = model.Price,
                Seen = model.Seen,
                Store = model.Store
            };
        }
    }
}