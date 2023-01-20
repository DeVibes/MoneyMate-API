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
            AccountName = model.AccountName,
            AccountUsers = model.AccountUsers,
            AccountCategories = model.AccountCategories,
            AccountPaymentTypes = model.AccountPaymentTypes,
        };
    }

    public static AccountUsersResponse ToAccountUserResponse(AccountModel model)
    {
        if (model is null)
            throw new ServerException($"Error in mapping {nameof(model)} to {nameof(AccountUsersResponse)}");
        return new AccountUsersResponse()
        {
            AccountId = model.Id.ToString(),
            AccountName = model.AccountName,
            AccountUsers = model.AccountUsers
        };
    }
    public static AccountPaymentTypesResponse ToAccountPaymentTypeResponse(AccountModel model)
    {
        if (model is null)
            throw new ServerException($"Error in mapping {nameof(model)} to {nameof(AccountPaymentTypesResponse)}");
        return new AccountPaymentTypesResponse()
        {
            AccountId = model.Id.ToString(),
            AccountName = model.AccountName,
            AccountPaymentTypes = model.AccountPaymentTypes
        };
    }

    public static AccountCategoriesResponse ToAccountCategoryResponse(AccountModel model)
    {
        if (model is null)
            throw new ServerException($"Error in mapping {nameof(model)} to {nameof(AccountCategoriesResponse)}");
        return new AccountCategoriesResponse()
        {
            AccountId = model.Id.ToString(),
            AccountName = model.AccountName,
            AccountCategories = model.AccountCategories
        };
    }
}

public record CreateAccountRequest
{
    public string AccountName { get; init; } = String.Empty;
    public IEnumerable<string> AccountUsers { get; init; } = Enumerable.Empty<string>();
    public IEnumerable<string> AccountCategories { get; init; } = Enumerable.Empty<string>();
    public IEnumerable<string> AccountPaymentTypes { get; init; } = Enumerable.Empty<string>();
    public static AccountModel ToAccountModel(CreateAccountRequest request)
    {
        if (String.IsNullOrEmpty(request.AccountName))
            throw new RequestException("Missing account name");
        return new()
        {
            AccountName = request.AccountName,
            AccountUsers = request.AccountUsers,
            AccountCategories = request.AccountCategories,
            AccountPaymentTypes = request.AccountPaymentTypes
        };
    }
}

public record PatchAccountRequest
{
    public string? AccountName { get; init; }
    public static AccountModel ToAccountModel(PatchAccountRequest request)
    {
        if (String.IsNullOrEmpty(request.AccountName))
            throw new RequestException("Missing account name");
        return new()
        {
            AccountName = request.AccountName
        };
    }
}

public record PatchAccountUsersRequest
{
    public string UserId { get; set; } = String.Empty;
}

public record PatchAccountPaymentTypeRequest
{
    public string PaymentTypeId { get; set; } = String.Empty;
}

public record AccountResponse
{
    public string Id { get; init; } = String.Empty;
    public string AccountName { get; init; } = String.Empty;
    public IEnumerable<string> AccountUsers { get; init; } = Enumerable.Empty<string>();
    public IEnumerable<string> AccountCategories { get; init; } = Enumerable.Empty<string>();
    public IEnumerable<string> AccountPaymentTypes { get; init; } = Enumerable.Empty<string>();
}

public record AccountUsersResponse
{
    public string AccountId { get; set; } = String.Empty;
    public string AccountName { get; set; } = String.Empty;
    public IEnumerable<string> AccountUsers { get; init; } = Enumerable.Empty<string>();
}

public record AccountPaymentTypesResponse
{
    public string AccountId { get; set; } = String.Empty;
    public string AccountName { get; set; } = String.Empty;
    public IEnumerable<string> AccountPaymentTypes { get; init; } = Enumerable.Empty<string>();
}

public record AccountCategoryRequest
{
    public string CategoryName { get; init; } = String.Empty;
    // public string CategoryIconRef { get; init; } = "unknown";
    public static CategoryModel ToCategoryModel(AccountCategoryRequest request)
    {
        if (String.IsNullOrEmpty(request.CategoryName))
            throw new RequestException($"Missing category name");
        return new CategoryModel()
        {
            CategoryName = request.CategoryName
            // CategoryIconRef = request.CategoryIconRef
        };
    }
}

public record AccountCategoriesResponse
{
    public string AccountId { get; set; } = String.Empty;
    public string AccountName { get; set; } = String.Empty;
    public IEnumerable<string> AccountCategories { get; init; } = Enumerable.Empty<string>();
}