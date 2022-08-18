namespace AccountyMinAPI.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<(int, string)>> GetAllCategories();
    Task InsertCategory(string categoryName);
    Task DeleteCategoryById(int id);
}