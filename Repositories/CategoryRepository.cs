using System.Data;
using AccountyMinAPI.DB;
namespace AccountyMinAPI.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ISqlDataAccess _db;
    private readonly ILoggerFactory _logger;

    public CategoryRepository(ISqlDataAccess db, ILoggerFactory logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<(int, string)>> GetAllCategories()
    {
        var results = await _db.LoadData<(int, string), dynamic>(CommandType.StoredProcedure,
            "dbo.spCategories_GetAll", new {});
        return results;
    }

    public async Task InsertCategory(string categoryName) =>
        await _db.SaveData(CommandType.StoredProcedure, "dbo.spCategories_Insert", new 
        {
            categoryName
        });

    public async Task DeleteCategoryById(int id) =>
        await _db.SaveData(CommandType.StoredProcedure, "dbo.spCategories_Delete", new { Id = id });
}