using MongoDB.Bson;

public record UsernameModel
{
    public ObjectId Id { get; init; }
    public string Username { get; init; } = String.Empty;
}