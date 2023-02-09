using AccountyMinAPI.Auth;
using MongoDB.Bson;

public record UserModel
{
    // public ObjectId Id { get; init; }
    public string Username { get; init; } = String.Empty;
    public string Role { get; init; } = APIRoles.User;
}