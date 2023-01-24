using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AccountyMinAPI.Models;

public record CategoryModel
{
    // [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    // public string Id { get; set; }
    public string CategoryName { get; set; }
    public string CategoryIconRef { get; set; }
}

public record AccountModel
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string Id { get; set; }
    public string? AccountName { get; init; }
    public IEnumerable<string>? AccountUsers { get; init; }
    public IEnumerable<string>? AccountCategories { get; init; }
    public IEnumerable<string>? AccountPaymentTypes { get; init; }
    public static AccountResponse ToAccountResponse(AccountModel model)
    {
        if (model is null)
            throw new ServerException($"Error in mapping {nameof(model)} to {nameof(AccountResponse)}");
        return new AccountResponse()
        {
            Id = model.Id.ToString(),
            AccountName = model.AccountName ?? String.Empty,
            AccountUsers = model.AccountUsers ?? Enumerable.Empty<string>(),
            AccountCategories = model.AccountCategories ?? Enumerable.Empty<string>(),
            AccountPaymentTypes = model.AccountPaymentTypes ?? Enumerable.Empty<string>()
        };
    }
}

public record AccountRequest
{
    public string? AccountName { get; set; }
    public IEnumerable<string>? AccountUsers { get; set; }
    public IEnumerable<string>? AccountCategories { get; set; }
    public IEnumerable<string>? AccountPaymentTypes { get; set; }
    public static AccountModel ToAccountModelCreate(AccountRequest request)
    {
        if (String.IsNullOrEmpty(request.AccountName))
            throw new RequestException("Missing account name");
        return new()
        {
            AccountName = request.AccountName,
            AccountUsers = request.AccountUsers ?? Enumerable.Empty<string>(),
            AccountCategories = request.AccountCategories ?? Enumerable.Empty<string>(),
            AccountPaymentTypes = request.AccountPaymentTypes ?? Enumerable.Empty<string>()
        };
    }
    public static AccountModel ToAccountModelPatch(AccountRequest request)
    {
        return new()
        {
            AccountName = request.AccountName,
            AccountUsers = request.AccountUsers,
            AccountCategories = request.AccountCategories,
            AccountPaymentTypes = request.AccountPaymentTypes
        };
    }
}

public record AccountResponse
{
    public string Id { get; init; } = String.Empty;
    public string AccountName { get; init; } = String.Empty;
    public IEnumerable<string> AccountUsers { get; init; } = Enumerable.Empty<string>();
    public IEnumerable<string> AccountCategories { get; init; } = Enumerable.Empty<string>();
    public IEnumerable<string> AccountPaymentTypes { get; init; } = Enumerable.Empty<string>();
}

public record AccountUser
{
    public string Id { get; init; } = String.Empty;
}

public record AccountCategory
{
    public string Name { get; init; } = String.Empty;
    // public string? Value { get; init; }
    // public string? IconRef { get; init; }
}

public record AccountPaymentType
{
    public string Name { get; init; } = String.Empty;
    // public string? Value { get; init; }
}