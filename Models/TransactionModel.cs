
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AccountyMinAPI.Models;
public record TransactionModel
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string Id { get; set; }
    public string? LinkedUserId { get; init; }
    public string? Description { get; init; } 
    public string? Store { get; init; } 
    public double? Price { get; init; }
    public DateTime? Date { get; init; }
    public string? Payment { get; init; }
    public string? Category { get; init; }
    public bool Seen { get; set; }

    public static TransactionResponse ToTransactionResponse(TransactionModel model)
    {
        if (model is null)
            throw new ServerException($"Error in mapping {nameof(model)} to {nameof(TransactionResponse)}");
        return new() 
        {
            Id = model.Id,
            LinkedUserId = model.LinkedUserId,
            Category = model.Category,
            Date = model.Date.Value.ToString("o"),
            Description = model.Description,
            PaymentType = model.Payment,
            Price = model.Price.Value,
            Seen = model.Seen,
            Store = model.Store
        };
    }

    // public static TransactionModel ToTransactionGetDto(TransactionModel model, out TransactionGetDto dto)
    // {
    //     CategoryModel.ToCategoryGetDto(model.Category, out CategoryGetDto category);
    //     // PaymentModel.ToGetPaymentDto(model.Payment, out GetPaymentDto payment);
    //     dto = new()
    //     {
    //         Id = model.Id.ToString(),
    //         Category = category,
    //         Date = model.Date.ToString("o"),
    //         Description = model.Description,
    //         // PaymentType = payment,
    //         Price = model.Price,
    //         Seen = model.Seen,
    //         Store = model.Store
    //     };
    // }
}
    
public record CreateTransactionRequest    
{
    public string LinkedUserId { get; init; } = String.Empty;
    public string Description { get; init; } = String.Empty;
    public string Store { get; init; } = String.Empty; 
    public double Price { get; init; }
    public string? Date { get; init; } = String.Empty;
    public string Category { get; init; } = String.Empty;
    public string PaymentType { get; init; } = String.Empty;
    public bool Seen { get; init; }
    public static TransactionModel ToTransactionModel(CreateTransactionRequest request)
    {
        if (String.IsNullOrEmpty(request.LinkedUserId))
            throw new RequestException("Missing user id");
        if (request.Price == 0)
            throw new RequestException("Missing price");
        var isDateCorrect = DateTime.TryParse(request.Date, out DateTime date);
        if (String.IsNullOrEmpty(request.Date))
            throw new RequestException("Missing date");
        if (!isDateCorrect)
            throw new RequestException("invalid date");
        if (String.IsNullOrEmpty(request.Category))
            throw new RequestException("Missing category");
        if (String.IsNullOrEmpty(request.PaymentType))
            throw new RequestException("Missing payment type");
        return new()
        {
           LinkedUserId = request.LinkedUserId,
           Category = request.Category,
           Date = date,
           Payment = request.PaymentType,
           Description = request.Description,
           Price = request.Price,
           Seen = request.Seen,
           Store = request.Store 
        };
    }
}

public record TransactionResponse
{
    public string Id { get; init; } = String.Empty;
    public string LinkedUserId { get; init; } = String.Empty;
    public string Description { get; init; } = String.Empty;
    public string Store { get; init; } = String.Empty; 
    public double Price { get; init; }
    public string Date { get; init; } = String.Empty;
    public string Category { get; init; } = String.Empty;
    public string PaymentType { get; init; } = String.Empty;
    public bool Seen { get; init; }
}
//     public record BalanceModel
//     {
//         public double Income { get; init; }
//         public double Outcome { get; init; }
//         public DateTime FromDate { get; init; }
//         public DateTime ToDate { get; init; }
//         public static void ToBalanceDto(BalanceModel model, out BalanceDto dto)
//         {
//             dto = new()
//             {
//                 FromDate = model.FromDate.ToString("o"),
//                 ToDate = model.ToDate.ToString("o"),
//                 Income = model.Income,
//                 Outcome = model.Outcome
//             };
//         }
//     }

//     public record TransactionCategoryModel
//     {
//         public string CategoryName { get; set; }
//         public double Total { get; set; }
//         public static void ToCategoryMonthDto(TransactionCategoryModel model, out TransactionCategoryDto dto)
//         {
//             dto = new()
//             {
//                 CategoryName = model.CategoryName,
//                 Total = model.Total
//             };
//         }
//     }

//     public record TransactionMonthModel
//     {
//         public int Year { get; set; }
//         public int Month { get; set; }
//         public double Total { get; set; }
//         public static void ToTransactionMonthDto(TransactionMonthModel model, out TransactionMonthDto dto)
//         {
//             dto = new()
//             {
//                 Year = model.Year,
//                 Month = model.Month,
//                 Total = model.Total
//             };
//         }
//     }