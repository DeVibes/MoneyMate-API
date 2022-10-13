namespace AccountyMinAPI.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryModel>> GetAllCategories();
    Task InsertCategory(CategoryModel category);
    Task<bool> DeleteCategoryById(string id);
}