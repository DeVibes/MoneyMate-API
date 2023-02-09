using MongoDB.Driver;

namespace AccountyMinAPI.Repositories;
public class MongoUsernameRepository : IUsernameRepository
{
    private readonly IMongoCollection<UserModel> allowedUserCollection;
    private readonly FilterDefinitionBuilder<UserModel> filterBuilder = 
        Builders<UserModel>.Filter;
    public MongoUsernameRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        var database = mongoClient.GetDatabase(configuration["Database:current"]);
        allowedUserCollection = database.GetCollection<UserModel>("allowedUsers");
    }
    public async Task<bool> IsUsernameAllowed(string username)
    {
        var filter = filterBuilder.Eq("username", username);
        return await allowedUserCollection.CountDocumentsAsync(filter) > 0;
    }
}
