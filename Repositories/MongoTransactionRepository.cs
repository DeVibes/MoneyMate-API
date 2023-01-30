using MongoDB.Driver;
using AccountyMinAPI.Data;
using MongoDB.Bson;
using System.Reflection;

namespace AccountyMinAPI.Repositories;

public static class NewBaseType
{
    public static int GetCustomMonth(DateTime date)
    {
        var month = date.Day >= 11 ?
            (date.Month == 12 ? 1 : date.Month + 1) : date.Month;
        if (date.Month == 12 && date.Day >= 11)
            return 1;
        return month;
    }
}

public class MongoTransactionRepository : ITransactionRepository
{
    private readonly IMongoCollection<TransactionModel> transactionCollection;
    private readonly IMongoCollection<AccountModel> accountCollection;
    private readonly FilterDefinitionBuilder<TransactionModel> filterBuilder = 
        Builders<TransactionModel>.Filter;
    private readonly FilterDefinitionBuilder<AccountModel> accountfilterBuilder = 
        Builders<AccountModel>.Filter;
    private readonly ProjectionDefinitionBuilder<TransactionModel> projectionBuilder = 
        Builders<TransactionModel>.Projection;

    public MongoTransactionRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        var database = mongoClient.GetDatabase(configuration["Database:Current"]);
        transactionCollection = database.GetCollection<TransactionModel>("transactions");
        accountCollection = database.GetCollection<AccountModel>("accounts");
    }

    public async Task DeleteTransactionById(string transactionId)
    {
        var filter = filterBuilder.Eq(transaction => transaction.Id, transactionId);
        var deleteResult = await transactionCollection.DeleteOneAsync(filter);
        if (deleteResult.DeletedCount == 0)
            throw new NotFoundException($"ID - {transactionId} was not found");
    }

    public async Task<TransactionModel> EditTransaction(string transactionId, TransactionModel model)
    {
        var filter = filterBuilder.Eq(transaction => transaction.Id, transactionId);
        var patchDefinition = Builders<TransactionModel>.Update;
        var fields = new List<UpdateDefinition<TransactionModel>>();
        foreach (var prop in model.GetType().GetProperties())
        {
            var propName = prop.Name;
            if (propName == "Id") continue;
            if (propName == "LinkedUserId") continue;
            var propValue = prop.GetValue(model);
            if (propValue is not null)
                fields.Add(patchDefinition.Set(propName, propValue));
        }
        var patchResult = await transactionCollection.UpdateOneAsync(filter, patchDefinition.Combine(fields));
        if (patchResult.MatchedCount == 0)
            throw new NotFoundException($"transaction '{transactionId}' not found");
        TransactionModel updatedModel = await transactionCollection.Find(filter)
            .FirstOrDefaultAsync();
        return updatedModel;
    }

    public async Task<(long, IEnumerable<TransactionModel>)> GetAllTransactions(
        TransactionsFilters filters)
    {
        if (filters.Users == Enumerable.Empty<string>())
            return (0, Enumerable.Empty<TransactionModel>());
        var filter = filterBuilder.Empty;
        var dateFilterUser = filterBuilder.And(
            filterBuilder.Gte(x => x.Date, filters.FromDate),
            filterBuilder.Lte(x => x.Date, filters.ToDate),
            filterBuilder.In("LinkedUserId", filters.Users));
        filter &= dateFilterUser;
        if (filters.Category is not null)
        {
            var catFilter = filterBuilder.Eq(tr => tr.Category, filters.Category);
            filter &= catFilter;
        }
        if (filters.PaymentType is not null)
        {
            var paymentTypeFilter = filterBuilder.Eq(tr => tr.PaymentType, filters.PaymentType);
            filter &= paymentTypeFilter;
        }
        if (filters.Seen is not null)
        {
            var seenTypeFilter = filterBuilder.Eq(tr => tr.Seen, filters.Seen);
            filter &= seenTypeFilter;
        }
        var allTransactions = transactionCollection.Find(filter)
            .SortByDescending(doc => doc.Date);
        var count = await allTransactions.CountDocumentsAsync();
        var items = await allTransactions.Skip(filters.PageNumber * 10)
            .Limit(10)
            .ToListAsync();

        return (count, items);
    }

    public async Task<TransactionModel> GetTransactionById(string transactionId)
    {
        var filter = filterBuilder.Eq(transaction => transaction.Id, transactionId);
        TransactionModel transaction = await transactionCollection.Find(filter).SingleOrDefaultAsync();
        if (transaction is null)
            throw new NotFoundException($"Transaction id '{transactionId}' not found");
        return transaction;
    }

    public async Task<TransactionModel> InsertTransaction(TransactionModel transaction)
    {
        await transactionCollection.InsertOneAsync(transaction);
        return transaction;
    }

    public async Task<IEnumerable<MonthlyCategorySummaryModel>> GetMonthlySummary(TransactionsFilters filters)
    {
        if (filters.Users == Enumerable.Empty<string>())
            return Enumerable.Empty<MonthlyCategorySummaryModel>();
        var filter = filterBuilder.Empty;
        var monthlyNonIncomefilter = filterBuilder.And(
            filterBuilder.Gte(x => x.Date, filters.FromDate),
            filterBuilder.Lte(x => x.Date, filters.ToDate),
            filterBuilder.In("LinkedUserId", filters.Users),
            filterBuilder.Ne(x => x.Category, "income"));
        filter &= monthlyNonIncomefilter;

        var categoriesedTransactions = await transactionCollection.Aggregate()
            .Match(filter)
            .Group(
                x => x.Category,
                group => new MonthlyCategorySummaryModel
                {
                    Category= group.Key,
                    Total = group.Sum(x => Math.Abs(x.Price.Value))
                })
            .ToListAsync();
        return categoriesedTransactions;
    }

    public async Task<IEnumerable<YearlySummaryModel>> GetYearlySumByMonth(TransactionsFilters filters)
    {
        if (filters.Users == Enumerable.Empty<string>())
            return Enumerable.Empty<YearlySummaryModel>();
        var filter = filterBuilder.Empty;
        var yearlyNonIncomefilter = filterBuilder.And(
            filterBuilder.Gte(x => x.Date, filters.FromDate),
            filterBuilder.Lte(x => x.Date, filters.ToDate),
            filterBuilder.In("LinkedUserId", filters.Users),
            filterBuilder.Ne(x => x.Category, "income"));
        filter &= yearlyNonIncomefilter;

        var yearlyTransactions = await transactionCollection.Find(filter).ToListAsync();

        //TODO The following logic must be done my mongo driver
        List<YearlySummaryModel> yearlyTransactionsMapped = yearlyTransactions.GroupBy(tr => 
        {
            var customMonth = tr.Date.Value.Month;
            var customYear = tr.Date.Value.Year;

            if (tr.Date.Value.Day < 11)
            {
                customMonth--;
                if (customMonth == 0)
                {
                    customMonth = 12;
                    customYear--;
                }
            }
            return new { Year = customYear, Month = customMonth };
        }).Select(x => new YearlySummaryModel()
        {
            Year = x.Key.Year,
            Month = x.Key.Month,
            Total = x.Sum(y => y.Price.Value)
        }).ToList();
        return yearlyTransactionsMapped;

            // Saving for reference
            // var yearlyTransactions = await transactionCollection.Aggregate()
            // .Group(
            //     x => new DateTime(x.Date.Value.Year, NewBaseType.GetCustomMonth(x.Date.Value), 1),
            //     group => new 
            //     {
            //         Date = group.Key,
            //         Total = group.Sum(x => x.Price)
            //     })
            // .Project(g => new YearlySummaryModel
            // {
            //     Total = g.Total.Value,
            //     Month = g.Date.Month,
            //     Year = g.Date.Year
            // })
            // .SortBy(item => item.Year)
            // .ThenBy(item => item.Month)
            // .ToListAsync();
        // return yearlyTransactions;
    }

    public async Task<BalanceModel> GetMonthlyBalance(TransactionsFilters filters)
    {
        if (filters.Users == Enumerable.Empty<string>())
            return new();
        var filter = filterBuilder.Empty;
        var dateNonIncomefilter = filterBuilder.And(
            filterBuilder.Gte(x => x.Date, filters.FromDate),
            filterBuilder.Lte(x => x.Date, filters.ToDate),
            filterBuilder.In("LinkedUserId", filters.Users));
        filter &= dateNonIncomefilter;

        var categoriesedTransactions = await transactionCollection.Aggregate()
            .Match(filter)
            .Group(
                x => x.Category == "Income" ? "income" : "monthlySpent",
                group => new
                {
                    Category = group.Key,
                    Total = group.Sum(x => x.Price)
                })
            .ToListAsync();

        var income = categoriesedTransactions.Where(x => x.Category == "income");
        var outcomes = categoriesedTransactions.Where(x => x.Category == "monthlySpent");
        
        return new BalanceModel()
        {
            Income = income.Count() == 0 ? 0 : income.First().Total.Value,
            Outcome = outcomes.Count() == 0 ? 0 : outcomes.First().Total.Value,
            FromDate = filters.FromDate,
            ToDate = filters.ToDate
        };
    }
}