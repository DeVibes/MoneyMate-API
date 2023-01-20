using AccountyMinAPI.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountyMinAPI.Models;
public record User
{
    public string GId { get; init; }
    public string Name { get; init; }
}

public record Category
{
    public string Name { get; init; }
    public string Value { get; init; }
    public string IconRef { get; init; }
}

public record PaymentType
{
    public string Name { get; init; }
    public string Value { get; init; }
}