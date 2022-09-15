using MongoDB.Bson;
using MongoDB.Driver;
using AccountyMinAPI.Data;

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
        var filter = filterBuilder.Eq(transaction => transaction.Id, id);
        var deleteResult = await transactionCollection.DeleteOneAsync(filter);
        if (deleteResult.DeletedCount == 0)
            throw new NotFoundException();
    }

    public async Task<IEnumerable<TransactionModel>> GetAllTransactions(TransactionsFilters filters)
    {
        return await transactionCollection.Find(_ => true).ToListAsync();
    }

    public async Task<TransactionModel> GetTransactionById(string id)
    {
        var filter = filterBuilder.Eq(transaction => transaction.Id, id);
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

    public async Task UpdateTransactionById(string id, TransactionModel transaction)
    {
        var filter = filterBuilder.Eq(existingTr => existingTr.Id, transaction.Id);
        await transactionCollection.ReplaceOneAsync(filter, transaction);
        
    }
}
