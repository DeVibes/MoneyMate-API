// using AccountyMinAPI.Data;
// using MongoDB.Bson;
// using MongoDB.Driver;

// namespace AccountyMinAPI.Repositories;

// public class MongoPaymentRepository : IPaymentTypeRepository
// {
//     private readonly IMongoCollection<PaymentModel> paymentCollection;
//     private readonly IMongoCollection<AccountModel> accountCollection;
//     private readonly FilterDefinitionBuilder<PaymentModel> filterBuilder = 
//         Builders<PaymentModel>.Filter;
//     private readonly FilterDefinitionBuilder<AccountModel> accountfilterBuilder = 
//         Builders<AccountModel>.Filter;
//     private readonly int Duplicate_Field = 11000;

//     public MongoPaymentRepository(IMongoClient mongoClient, IConfiguration configuration)
//     {
//         var database = mongoClient.GetDatabase(configuration["Database:current"]);
//         paymentCollection = database.GetCollection<PaymentModel>("paymentTypes");
//         accountCollection = database.GetCollection<AccountModel>("accounts");
//         createUniqueIndexOnField();
//     }

//     public async Task<RepositporyModel> GetAllPaymentTypes(string accountId)
//     {
//         try
//         {
//             var accountFilter = accountfilterBuilder.Eq(acc => acc.Id, new ObjectId(accountId));
//             var accountResult = await accountCollection.Find(accountFilter).FirstOrDefaultAsync();
//             RepositporyModel response = new();
//             if (accountResult is null)
//             {
//                 response.ErrorType = ErrorTypeEnum.ACCOUNT_NOT_FOUND;
//                 response.ErrorMessage = $"Account id {accountId} not found";
//             };
//             var result = await paymentCollection.Find(_ => true).ToListAsync();
//             return new RepositporyModel()
//             {
//                 Payload = result
//             };
//         }
//         catch (System.Exception ex)
//         {
//             return new RepositporyModel()
//             {
//                 ErrorType = ErrorTypeEnum.DB_ERROR,
//                 ErrorMessage = ex.Message
//             };
//         }
//     }
//     public async Task<RepositporyModel> DeletePaymentTypeById(string accountId, string id)
//     {
//         var _id = new ObjectId(id);
//         var deleteResult = await paymentCollection.DeleteOneAsync(paymentType => paymentType.Id == _id);
//         return deleteResult.DeletedCount != 0;
//     }

//     public async Task<RepositporyModel> InsertPaymentType(PaymentModel payment)
//     {
//         try
//         {
//             var accountFilter = accountfilterBuilder.Eq(acc => acc.Id, new ObjectId(payment.LinkedAccountId));
//             var accountResult = await accountCollection.Find(accountFilter).FirstOrDefaultAsync();
//             RepositporyModel response = new();
//             if (accountResult is null)
//             {
//                 response.ErrorType = ErrorTypeEnum.ACCOUNT_NOT_FOUND;
//                 response.ErrorMessage = $"Account id {payment.LinkedAccountId} not found";
//             };
//             await paymentCollection.InsertOneAsync(payment);

//             return response;
//         }
//         catch (MongoWriteException ex)
//         {
//             RepositporyModel response = new();
//             if (ex.WriteError.Code == Duplicate_Field)
//             {
//                 response.ErrorType = ErrorTypeEnum.PAYMENT_ALEADY_EXISTS;
//                 response.ErrorMessage = $"Payment '{payment.Name}' already exists";
//             }
//             else
//             {
//                 response.ErrorType = ErrorTypeEnum.DB_ERROR;
//                 response.ErrorMessage = $"Failed inserting '{payment.Name}'";
//             }
//             return response;
                
//         }
//         catch (System.Exception ex)
//         {
//             return new RepositporyModel()
//             {
//                 ErrorType = ErrorTypeEnum.DB_ERROR,
//                 ErrorMessage = ex.Message
//             };
//         }
//     }

//     private void createUniqueIndexOnField()
//     {
//         CreateIndexModel<PaymentModel> indexModel = new(
//             new IndexKeysDefinitionBuilder<PaymentModel>().Ascending(x => x.Name),
//             new CreateIndexOptions() { Unique = true }
//         );
//         this.paymentCollection.Indexes.CreateOne(indexModel);
//     }
// }