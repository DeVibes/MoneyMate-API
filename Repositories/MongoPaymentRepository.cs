using AccountyMinAPI.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AccountyMinAPI.Repositories;

public class MongoPaymentRepository : IPaymentTypeRepository
{
    private readonly IMongoCollection<PaymentModel> paymentCollection;
    private readonly FilterDefinitionBuilder<PaymentModel> filterBuilder = 
        Builders<PaymentModel>.Filter;
    private readonly int Duplicate_Field = 11000;

    public MongoPaymentRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        var database = mongoClient.GetDatabase(configuration["Database:current"]);
        paymentCollection = database.GetCollection<PaymentModel>("paymentTypes");
        createUniqueIndexOnField();
    }

    public async Task<IEnumerable<PaymentModel>> GetAllPaymentTypes()
    {
        return await paymentCollection.Find(_ => true).ToListAsync();
    }
    public async Task<bool> DeletePaymentTypeById(string id)
    {
        var _id = new ObjectId(id);
        var deleteResult = await paymentCollection.DeleteOneAsync(paymentType => paymentType.Id == _id);
        return deleteResult.DeletedCount != 0;
    }

    public async Task InsertPaymentType(PaymentModel payment)
    {
        try
        {
            await paymentCollection.InsertOneAsync(payment);
        }
        catch (MongoWriteException ex)
        {
            if (ex.WriteError.Code == Duplicate_Field)
                throw new PaymentAlreadyExistsException(payment.Name);
            throw ex;
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }

    private void createUniqueIndexOnField()
    {
        CreateIndexModel<PaymentModel> indexModel = new(
            new IndexKeysDefinitionBuilder<PaymentModel>().Ascending(x => x.Name),
            new CreateIndexOptions() { Unique = true }
        );
        this.paymentCollection.Indexes.CreateOne(indexModel);
    }
}