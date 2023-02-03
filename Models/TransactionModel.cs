
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
    public DateTime? Date { get; set; }
    public string? PaymentType { get; init; }
    public string? Category { get; init; }
    public bool? Seen { get; set; }

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
            PaymentType = model.PaymentType,
            Price = model.Price.Value,
            Seen = model.Seen.Value,
            Store = model.Store
        };
    }
}

public record TransactionRequest    
{
    public string LinkedUserId { get; init; } = String.Empty;
    public string? Description { get; init; }
    public string? Store { get; init; }
    public string? Price { get; init; }
    public string? Date { get; init; }
    public string? Category { get; init; }
    public string? PaymentType { get; init; }
    public bool? Seen { get; init; }
    public static TransactionModel ToTransactionModelPatch(TransactionRequest request)
    {
        bool isDateValid = DateTime.TryParse(request.Date, out DateTime date);
        if (request.Date is not null && !isDateValid)
            throw new RequestException("Invalid date");
        bool isPriceValid = Double.TryParse(request.Price, out double price);
        if (request.Price is not null && !isPriceValid)
            throw new RequestException("Invalid price");
        // bool isSeenValid = Boolean.TryParse(request.Seen, out bool seen);
        // if (request.Seen is not null && !isSeenValid)
        //     throw new RequestException("Invalid seen");

        if (!String.IsNullOrEmpty(request.Category))
            price = request.Category.Equals("Income") ? price : price * -1;

        return new()
        {
           Category = request.Category,
           PaymentType = request.PaymentType,
           Description = request.Description,
           Store = request.Store,
           Date = isDateValid ? date : null,
           Price = isPriceValid ? price : null,
           Seen = request.Seen 
        };
    }
    
    public static TransactionModel ToTransactionModelCreate(TransactionRequest request)
    {
        if (String.IsNullOrEmpty(request.LinkedUserId))
            throw new RequestException("Missing user id");
        var isDateValid = DateTime.TryParse(request.Date, out DateTime date);
        if (String.IsNullOrEmpty(request.Date))
            throw new RequestException("Missing date");
        if (!isDateValid)
            throw new RequestException("Invalid date");
        var isPriceValid = Double.TryParse(request.Price, out Double price);
        if (String.IsNullOrEmpty(request.Price))
            throw new RequestException("Missing price");
        if (!isPriceValid)
            throw new RequestException("Invalid price");
        // bool isSeenValid = Boolean.TryParse(request.Seen, out bool seen);
        // if (request.Seen is not null && !isSeenValid)
        //     throw new RequestException("Invalid seen");
        if (String.IsNullOrEmpty(request.Category))
            throw new RequestException("Missing category");
        if (String.IsNullOrEmpty(request.PaymentType))
            throw new RequestException("Missing payment type");

        price = request.Category.Equals("Income") ? price : price * -1;

        return new()
        {
           LinkedUserId = request.LinkedUserId,
           Category = request.Category ?? String.Empty,
           PaymentType = request.PaymentType ?? String.Empty,
           Description = request.Description ?? String.Empty,
           Store = request.Store ?? String.Empty,
           Date = date,
           Price = price,
           Seen = request.Seen ?? false
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