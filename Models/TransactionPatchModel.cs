
namespace AccountyMinAPI.Models
{
    public record TransactionPatchModel    
    {
        public string? Description { get; init; }
        public string? Store { get; init; }
        public double? Price { get; init; }
        public DateTime? Date { get; init; } 
        public PaymentModel? Payment { get; init; } 
        public CategoryModel? Category { get; init; }
        public bool? Seen { get; init; }
        
        // TODO Handle mapping errors from DB
        // public static void ToTransactionGetDto(TransactionModel model, out TransactionGetDto dto)
        // {
        //     CategoryModel.ToCategoryGetDto(model.Category, out CategoryGetDto category);
        //     PaymentModel.ToPaymentGetDto(model.Payment, out PaymentGetDto payment);
        //     dto = new()
        //     {
        //         Id = model.Id.ToString(),
        //         Category = category,
        //         Date = model.Date.ToString("o"),
        //         Description = model.Description,
        //         PaymentType = payment,
        //         Price = model.Price,
        //         Seen = model.Seen,
        //         Store = model.Store
        //     };
        // }
    }
}