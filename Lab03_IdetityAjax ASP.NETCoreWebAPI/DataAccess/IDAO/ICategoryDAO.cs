using BusinessObjects.Models;

namespace DataAccess.IDAO;

public interface ICategoryDAO
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int categoryId);
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int categoryId);
    Task<bool> IsCategoryExistsAsync(int categoryId);
}