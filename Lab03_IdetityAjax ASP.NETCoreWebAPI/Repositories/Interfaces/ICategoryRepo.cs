using BusinessObjects.Models;

namespace Repositories.Interfaces;

public interface ICategoryRepo
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int categoryId);
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int categoryId);
    Task<bool> IsCategoryExistsAsync(int categoryId);
    Task<Category?> GetCategoryByNameAsync(string categoryName);
    Task<IEnumerable<Category>> GetCategoriesWithOrchidsAsync();
}