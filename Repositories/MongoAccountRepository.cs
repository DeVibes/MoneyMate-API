using MongoDB.Bson;
using MongoDB.Driver;

namespace AccountyMinAPI.Repositories;
public class MongoAccountRepository: IAccountRepository
{
    private readonly IMongoCollection<AccountModel> accountCollection;
    private readonly FilterDefinitionBuilder<AccountModel> filterBuilder = Builders<AccountModel>.Filter;
    private readonly ProjectionDefinitionBuilder<AccountModel> projectionBuilder = Builders<AccountModel>.Projection;
    private readonly int DUPLICATE_FIELD = 11000;
    public MongoAccountRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        var database = mongoClient.GetDatabase(configuration["Database:Current"]);
        accountCollection = database.GetCollection<AccountModel>("accounts");
        CreateUniqueIndexOnField();
    }

    public async Task<AccountModel> InsertAccount(AccountModel account)
    {
        try
        {
            await accountCollection.InsertOneAsync(account);
            var createdAccountFilter = filterBuilder.Eq(acc => acc.AccountName, account.AccountName);
            var createdAccount = await accountCollection.Find(createdAccountFilter).FirstOrDefaultAsync();
            if (createdAccount == null)
                throw new ServerException($"Failed inserting account name '{account.AccountName}'");
            return createdAccount;
        }
        catch (MongoWriteException ex)
        {
            // log error
            if (ex.WriteError.Code == DUPLICATE_FIELD)
                throw new AlreadyExistsException($"Account name '{account.AccountName}' already exists");
            throw new ServerException($"Failed inserting account name '{account.AccountName}'");
        }
    }

    public async Task<IEnumerable<AccountModel>> GetAllAccounts()
    {
        var emptyFilter = filterBuilder.Empty;
        return await accountCollection.Find(emptyFilter).ToListAsync();
    }

    public async Task<AccountModel> GetAccountById(string accountId)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        AccountModel account = await accountCollection.Find(filter).SingleOrDefaultAsync();
        if (account is null)
        {
            // log error
            throw new NotFoundException($"Account id '{accountId}' not found");
        }
        return account;
    }

    public async Task DeleteAccountById(string accountId)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        var deleteResult = await accountCollection.DeleteOneAsync(filter);
        if (deleteResult.DeletedCount == 0)
        {
            // log error
            throw new NotFoundException($"Account id '{accountId}' not found");
        }
    }
    
    public async Task<AccountModel> AssignUserToAccount(string accountId, string userId)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        var updateUsersDefine = Builders<AccountModel>.Update
            .AddToSet(acc => acc.AccountUsers, userId);
        UpdateResult result = await accountCollection.UpdateOneAsync(filter, updateUsersDefine);
        if (result.MatchedCount == 0)
        {
            // log error
            throw new NotFoundException($"Account '{accountId}' not found");
        }
        if (result.ModifiedCount == 0)
        {
            // log error
            throw new AlreadyExistsException($"Account '{accountId}' already has user id '{userId}'");
        }
        AccountModel updatedAccount = await accountCollection.Find(filter).FirstOrDefaultAsync();
        return updatedAccount;
    }

    public async Task<AccountModel> DeassignUserToAccount(string accountId, string userId)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        var updateUsersDefine = Builders<AccountModel>.Update
            .Pull(acc => acc.AccountUsers, userId);
        UpdateResult result = await accountCollection.UpdateOneAsync(filter, updateUsersDefine);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"Account '{accountId}' not found");
        if (result.ModifiedCount == 0)
            throw new NotFoundException($"Account '{accountId}' doesnt have user id '{userId}'");
        AccountModel updatedModel = await accountCollection.Find(filter).FirstOrDefaultAsync();
        return updatedModel;
    }

    public async Task<AccountModel> AssignPaymentTypeToAccount(string accountId, string paymentTypeId)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        var updateUsersDefine = Builders<AccountModel>.Update
            .AddToSet(acc => acc.AccountPaymentTypes, paymentTypeId);
        UpdateResult result = await accountCollection.UpdateOneAsync(filter, updateUsersDefine);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"Account '{accountId}' not found");
        if (result.ModifiedCount == 0)
            throw new AlreadyExistsException($"Account '{accountId}' already has payment type id '{paymentTypeId}'");
        AccountModel updatedModel = await accountCollection.Find(filter).FirstOrDefaultAsync();
        return updatedModel;
    }

    public async Task<AccountModel> DeassignPaymentTypeToAccount(string accountId, string paymentTypeId)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        var updateUsersDefine = Builders<AccountModel>.Update
            .Pull(acc => acc.AccountPaymentTypes, paymentTypeId);
        UpdateResult result = await accountCollection.UpdateOneAsync(filter, updateUsersDefine);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"Account '{accountId}' not found");
        if (result.ModifiedCount == 0)
            throw new NotFoundException($"Account '{accountId}' dont have payment type id '{paymentTypeId}'");
        AccountModel updatedModel = await accountCollection.Find(filter).FirstOrDefaultAsync();
        return updatedModel;
    }

    public async Task<AccountModel> AddAccountCategory(string accountId, CategoryModel model)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        var updateCategoriesDefine = Builders<AccountModel>.Update
            .AddToSet(acc => acc.AccountCategories, model.CategoryName);
        UpdateResult result = await accountCollection.UpdateOneAsync(filter, updateCategoriesDefine);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"Account '{accountId}' not found");
        if (result.ModifiedCount == 0)
            throw new AlreadyExistsException($"Account '{accountId}' already has category '{model.CategoryName}'");
        AccountModel updatedModel = await accountCollection.Find(filter).FirstOrDefaultAsync();
        return updatedModel;
    }

    public async Task<AccountModel> RemoveAccountCategory(string accountId, CategoryModel model)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        var updateCategoriesDefine = Builders<AccountModel>.Update
            .Pull(acc => acc.AccountCategories, model.CategoryName);
        UpdateResult result = await accountCollection.UpdateOneAsync(filter, updateCategoriesDefine);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"Account '{accountId}' not found");
        if (result.ModifiedCount == 0)
            throw new NotFoundException($"Account '{accountId}' doesnt have category '{model.CategoryName}'");
        AccountModel updatedModel = await accountCollection.Find(filter).FirstOrDefaultAsync();
        return updatedModel;
    }

    public async Task<AccountModel> EditAccount(string accountId, AccountModel accountModel)
    {
        var filter = filterBuilder.Eq(account => account.Id, accountId);
        var updateAccountDefine = Builders<AccountModel>.Update
            .Set(acc => acc.AccountName, accountModel.AccountName);
        UpdateResult result = await accountCollection.UpdateOneAsync(filter, updateAccountDefine);
        if (result.MatchedCount == 0)
        {
            // log error
            throw new NotFoundException($"Account '{accountId}' not found");
        }
        AccountModel updatedModel = await accountCollection.Find(filter)
            .FirstOrDefaultAsync();
        return updatedModel;
    }

    public async Task ValidateTransactionData(string accountId, TransactionModel model)
    {
        var userfilter = filterBuilder.Eq(account => account.Id, accountId) &
            filterBuilder.Where(acc => acc.AccountUsers.Contains(model.LinkedUserId));
        var categoryfilter = filterBuilder.Eq(account => account.Id, accountId) &
            filterBuilder.Where(acc => acc.AccountCategories.Contains(model.Category));
        var paymentfilter = filterBuilder.Eq(account => account.Id, accountId) &
            filterBuilder.Where(acc => acc.AccountPaymentTypes.Contains(model.Payment));
        var result = await accountCollection.Find(userfilter).FirstOrDefaultAsync();
        if (result is null)
            throw new RequestException($"Account '{accountId}' and user '{model.LinkedUserId}' are not linked");
        result = await accountCollection.Find(categoryfilter).FirstOrDefaultAsync();
        if (result is null)
            throw new RequestException($"Account '{accountId}' and category '{model.Category}' are not linked");
        result = await accountCollection.Find(paymentfilter).FirstOrDefaultAsync();
        if (result is null)
            throw new RequestException($"Account '{accountId}' and payment type '{model.Payment}' are not linked");
    }

    private void CreateUniqueIndexOnField()
    {
        CreateIndexModel<AccountModel> indexModel = new(
            new IndexKeysDefinitionBuilder<AccountModel>().Ascending(x => x.AccountName),
            new CreateIndexOptions() { Unique = true }
        );
        this.accountCollection.Indexes.CreateOne(indexModel);
    }
}