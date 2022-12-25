using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountyMinAPI.Models
{
    public record TransactionModel
    {
        public ObjectId Id { get; set; }
        public string Description { get; init; } = String.Empty;
        public string Store { get; init; } = String.Empty;
        public double Price { get; init; }
        public DateTime Date { get; init; }
        public PaymentModel Payment { get; init; }
        public CategoryModel Category { get; init; }
        public bool Seen { get; set; }

        //TODO Handle mapping errors from DB
        public static void ToTransactionGetDto(TransactionModel model, out TransactionGetDto dto)
        {
            CategoryModel.ToCategoryGetDto(model.Category, out CategoryGetDto category);
            PaymentModel.ToPaymentGetDto(model.Payment, out PaymentGetDto payment);
            dto = new()
            {
                Id = model.Id.ToString(),
                Category = category,
                Date = model.Date.ToString("o"),
                Description = model.Description,
                PaymentType = payment,
                Price = model.Price,
                Seen = model.Seen,
                Store = model.Store
            };
        }
    }

    public record BalanceModel
    {
        public double Income { get; init; }
        public double Outcome { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
        public static void ToBalanceDto(BalanceModel model, out BalanceDto dto)
        {
            dto = new()
            {
                FromDate = model.FromDate.ToString("o"),
                ToDate = model.ToDate.ToString("o"),
                Income = model.Income,
                Outcome = model.Outcome
            };
        }
    }

    public record TransactionCategoryModel
    {
        public string CategoryName { get; set; }
        public double Total { get; set; }
        public static void ToCategoryMonthDto(TransactionCategoryModel model, out TransactionCategoryDto dto)
        {
            dto = new()
            {
                CategoryName = model.CategoryName,
                Total = model.Total
            };
        }
    }

    public record TransactionMonthModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public double Total { get; set; }
        public static void ToTransactionMonthDto(TransactionMonthModel model, out TransactionMonthDto dto)
        {
            dto = new()
            {
                Year = model.Year,
                Month = model.Month,
                Total = model.Total
            };
        }
    }
}