using MongoDB.Driver;
using AccountyMinAPI.Data;
using MongoDB.Bson;
using System.Reflection;

namespace AccountyMinAPI.Repositories;

public class MongoTransactionRepository : ITransactionRepository
{
    private readonly IMongoCollection<TransactionModel> transactionCollection;
    private readonly FilterDefinitionBuilder<TransactionModel> filterBuilder = Builders<TransactionModel>.Filter;

    public MongoTransactionRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        var database = mongoClient.GetDatabase(configuration["Database:Current"]);
        transactionCollection = database.GetCollection<TransactionModel>("transactions");
    }
    public async Task DeleteTransactionById(string id)
    {
        var _id = new ObjectId(id);
        var filter = filterBuilder.Eq(transaction => transaction.Id, _id);
        var deleteResult = await transactionCollection.DeleteOneAsync(filter);
        if (deleteResult.DeletedCount == 0)
            throw new NotFoundException();
    }

    public async Task<IEnumerable<TransactionModel>> GetAllTransactions(TransactionsFilters filters)
    {
        var filter = filterBuilder.Empty;
        if (filters.StartDate != null && filters.EndDate != null)
        {
            var dateFilter = filterBuilder.And(filterBuilder.Gte(x => x.Date, filters.StartDate),
            filterBuilder.Lte(x => x.Date, filters.EndDate));
            filter &= dateFilter;
        }
        return await transactionCollection.Find(filter).ToListAsync();
    }

    public async Task<TransactionModel> GetTransactionById(string id)
    {
        var _id = new ObjectId(id);
        var filter = filterBuilder.Eq(transaction => transaction.Id, _id);
        return await transactionCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task InsertTransaction(TransactionModel transaction)
    {
        await transactionCollection.InsertOneAsync(transaction);
    }

    public Task PatchTransaction(string id, TransactionModel transaction)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTransactionById(string id, TransactionModel transaction)
    {
        throw new NotImplementedException();
    }

    public async Task PatchSeenStatus(string id, bool seen)
    {
        var _id = new ObjectId(id);
        var filter = filterBuilder.Eq(transaction => transaction.Id, _id);
        var update = Builders<TransactionModel>.Update.Set("Seen", seen);
        var result = await transactionCollection.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException();
    } 
}
