using MongoDB.Driver;
using AccountyMinAPI.Data;
using MongoDB.Bson;
using System.Reflection;

namespace AccountyMinAPI.Repositories;

public class MongoTransactionRepository : ITransactionRepository
{
    private readonly IMongoCollection<TransactionModel> transactionCollection;
    private readonly FilterDefinitionBuilder<TransactionModel> filterBuilder = Builders<TransactionModel>.Filter;
    private readonly ProjectionDefinitionBuilder<TransactionModel> projectionBuilder = Builders<TransactionModel>.Projection;

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

    public async Task<(long, IEnumerable<TransactionModel>)> GetAllTransactions(TransactionsFilters filters)
    {
        var filter = filterBuilder.Empty;
        if (filters.FromDate != null && filters.ToDate != null)
        {
            var dateFilter = filterBuilder.And(filterBuilder.Gte(x => x.Date, filters.FromDate),
            filterBuilder.Lte(x => x.Date, filters.ToDate));
            filter &= dateFilter;
        }
        var allTransactions = transactionCollection.Find(filter)
            .SortByDescending(doc => doc.Date);

        var count = await allTransactions.CountDocumentsAsync();
        var items = await allTransactions.Skip(filters.PageNumber * 10)
            .Limit(10)
            .ToListAsync();

        return (count, items);
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

    public async Task<BalanceModel> GetMonthlyBalance(TransactionsFilters filters)
    {
        var fromDate = filters.FromDate.HasValue ? filters.FromDate.Value : DateTime.MinValue;
        var toDate = filters.ToDate.HasValue ? filters.ToDate.Value : DateTime.MinValue;

        var monthlyNonIncomefilter = filterBuilder.Gte(x => x.Date, fromDate) &
            filterBuilder.Lte(x => x.Date, toDate);

        var categoriesedTransactions = await transactionCollection.Aggregate()
            .Match(monthlyNonIncomefilter)
            .Group(
                x => x.Category.Name == "Income" ? "Income" : "MonthlySpent",
                group => new
                {
                    Category = group.Key,
                    Total = group.Sum(x => x.Price)
                })
            .ToListAsync();

        var income = categoriesedTransactions.Where(x => x.Category == "Income");
        var outcomes = categoriesedTransactions.Where(x => x.Category == "MonthlySpent");
        
        return new BalanceModel()
        {
            Income = income.Count() == 0 ? 0 : income.First().Total,
            Outcomes = outcomes.Count() == 0 ? 0 : outcomes.First().Total,
            FromDate = fromDate,
            ToDate = toDate
        };
    }

    public async Task<IEnumerable<TransactionCategory>> GetMonthlyByCategory(TransactionsFilters filters)
    {
        var fromDate = filters.FromDate.HasValue ? filters.FromDate.Value : DateTime.MinValue;
        var toDate = filters.ToDate.HasValue ? filters.ToDate.Value : DateTime.MinValue;

        var monthlyNonIncomefilter = filterBuilder.Gte(x => x.Date, fromDate) &
            filterBuilder.Lte(x => x.Date, toDate) & 
            filterBuilder.Ne(x => x.Category.Name, "Income");

        var categoriesedTransactions = await transactionCollection.Aggregate()
            .Match(monthlyNonIncomefilter)
            .Group(
                x => x.Category.Name,
                group => new TransactionCategory
                {
                    CategoryName = group.Key,
                    Total = group.Sum(x => Math.Abs(x.Price))
                })
            .ToListAsync();
        return categoriesedTransactions;
    }
    
    public async Task<IEnumerable<TransactionMonth>> GetYearlySumByMonth(TransactionsFilters filters)
    {
        var fromDate = filters.FromDate.HasValue ? filters.FromDate.Value : DateTime.MinValue;
        var toDate = filters.ToDate.HasValue ? filters.ToDate.Value : DateTime.MinValue;

        var yearlyNonIncomefilter = filterBuilder.Gte(x => x.Date, fromDate) &
            filterBuilder.Lte(x => x.Date, toDate) & 
            filterBuilder.Ne(x => x.Category.Name, "Income");

        var yearlyTransactions = await transactionCollection.Aggregate()
            .Match(yearlyNonIncomefilter)
            .Group(
                x => new DateTime(x.Date.Year, x.Date.Month, 1),
                group => new 
                {
                    Date = group.Key,
                    Total = group.Sum(x => x.Price)
                })
            .Project(g => new TransactionMonth
            {
                Total = g.Total,
                Month = g.Date.Month,
                Year = g.Date.Year
            })
            .ToListAsync();
        return yearlyTransactions;
    }
}