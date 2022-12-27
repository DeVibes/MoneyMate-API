using MongoDB.Driver;

namespace AccountyMinAPI.Repositories;
public class MongoUsernameRepository : IUsernameRepository
{
    private readonly IMongoCollection<UsernameModel> allowedUserCollection;
    private readonly FilterDefinitionBuilder<UsernameModel> filterBuilder = 
        Builders<UsernameModel>.Filter;
    public MongoUsernameRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        var database = mongoClient.GetDatabase(configuration["Database:current"]);
        allowedUserCollection = database.GetCollection<UsernameModel>("allowedUsers");
    }
    public async Task<bool> IsUsernameAllowed(string username)
    {
        var filter = filterBuilder.Eq("username", username);
        return await allowedUserCollection.CountDocumentsAsync(filter) > 0;
    }
}
