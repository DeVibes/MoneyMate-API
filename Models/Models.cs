namespace AccountyMinAPI.Models;

public abstract record AccountBase
{
    public string Id { get; init; }
    public string Name { get; init; }
}

public record Account2
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public IEnumerable<User>? Users { get; init; }
    public IEnumerable<Category>? Categories { get; init; }
    public IEnumerable<PaymentType>? PaymentTypes { get; init; }
}

public record CreateAccount2Request
{
    public string Name { get; init; } = String.Empty;
    public IEnumerable<User> Users { get; init; } = Enumerable.Empty<User>();
    public IEnumerable<Category> Categories { get; init; } = Enumerable.Empty<Category>();
    public IEnumerable<PaymentType> PaymentTypes { get; init; } = Enumerable.Empty<PaymentType>();
}

public record EditAccount2Request
{
    public string? Name { get; init; }
    public IEnumerable<User>? Users { get; init; }
    public IEnumerable<Category>? Categories { get; init; }
    public IEnumerable<PaymentType>? PaymentTypes { get; init; }
}

public record CreateAccount2Response
{
    public string Id { get; init; } = String.Empty;
    public string Name { get; init; } = String.Empty;
    public IEnumerable<User> Users { get; init; } = Enumerable.Empty<User>();
    public IEnumerable<Category> Categories { get; init; } = Enumerable.Empty<Category>();
    public IEnumerable<PaymentType> PaymentTypes { get; init; } = Enumerable.Empty<PaymentType>();
}