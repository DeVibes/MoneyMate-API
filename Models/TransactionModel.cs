namespace AccountyMinAPI.Models
{
    public record TransactionModel    
    {
        public int? Id { get; init; }
        public string? Description { get; init; }
        public string? Store { get; init; } 
        public double? Price { get; init; }
        public DateTime? Date { get; init; } 
        public int? PaymentTypeId { get; init; } 
        public string? PaymentTypeName { get; set; }
        public int? CategoryId { get; init; }
        public string? CategoryTypeName { get; set; }
        public bool? Seen { get; init; }
        public static (bool, string) ToTransactionReadDto(TransactionModel model, out TransactionReadDto dto)
        {
            dto = null;
            if (!model.Id.HasValue)
                return (false, "No id");
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
                Id = model.Id.Value,
                CategoryId = model.CategoryId.Value,
                CategoryTypeName = model.CategoryTypeName ?? String.Empty,
                Date = model.Date.Value.ToString("o"),
                Description = model.Description ?? String.Empty,
                PaymentTypeId = model.PaymentTypeId.Value,
                PaymentTypeName = model.PaymentTypeName ?? String.Empty,
                Price = model.Price.Value,
                Seen = model.Seen.Value,
                Store = model.Store ?? String.Empty
            };
            return (true, String.Empty);
        }
    }
}