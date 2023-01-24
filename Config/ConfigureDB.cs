using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace AccountyMinAPI.Config;

public static class ConfigureDB
{
    public static IServiceCollection RegisterMongoDB(this IServiceCollection services, string connectionString)
    {
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            return new MongoClient(settings);
        });
        return services;
    }
}