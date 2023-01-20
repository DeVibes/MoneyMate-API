// // using AccountyMinAPI.Data;
// using MongoDB.Bson;
// using MongoDB.Driver;

// namespace AccountyMinAPI.Repositories;

// public class MongoCategoryRepository : ICategoryRepository
// {
//     private readonly IMongoCollection<CategoryModel> categoryCollection;
//     private readonly FilterDefinitionBuilder<CategoryModel> filterBuilder = 
//         Builders<CategoryModel>.Filter;
//     private readonly int Duplicate_Field = 11000;

//     public MongoCategoryRepository(IMongoClient mongoClient, IConfiguration configuration)
//     {
//         var database = mongoClient.GetDatabase(configuration["Database:current"]);
//         categoryCollection = database.GetCollection<CategoryModel>("categories");
//         createUniqueIndexOnField();
//     }

//     public async Task<IEnumerable<CategoryModel>> GetAllCategories()
//     {
//         return await categoryCollection.Find(_ => true).ToListAsync();
//     }
//     public async Task<bool> DeleteCategoryById(string id)
//     {
//         var _id = new ObjectId(id);
//         var deleteResult = await categoryCollection.DeleteOneAsync(category => category.Id == _id);
//         return deleteResult.DeletedCount != 0;
//     }

//     public async Task InsertCategory(CategoryModel category)
//     {
//         try
//         {
//             await categoryCollection.InsertOneAsync(category);
//         }
//         catch (MongoWriteException ex)
//         {
//             if (ex.WriteError.Code == Duplicate_Field)
//                 throw new CategoryAlreadyExistsException(category.Name);
//             throw ex;
//         }
//         catch (System.Exception ex)
//         {
//             throw ex;
//         }
//     }

//     private void createUniqueIndexOnField()
//     {
//         CreateIndexModel<CategoryModel> indexModel = new(
//             new IndexKeysDefinitionBuilder<CategoryModel>().Ascending(x => x.Name),
//             new CreateIndexOptions() { Unique = true }
//         );
//         this.categoryCollection.Indexes.CreateOne(indexModel);
//     }
// }