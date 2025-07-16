using BusinessObjects.Models;
using DataAccess.IDAO;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class CategoryRepo : ICategoryRepo
{
    private readonly ICategoryDAO _categoryDAO;

    public CategoryRepo(ICategoryDAO categoryDAO)
    {
        _categoryDAO = categoryDAO;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryDAO.GetAllCategoriesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId)
    {
        if (categoryId <= 0)
            throw new ArgumentException("Invalid Category ID", nameof(categoryId));

        return await _categoryDAO.GetCategoryByIdAsync(categoryId);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.CategoryName))
            throw new ArgumentException("Category name is required", nameof(category));

        // Check if category with same name already exists
        var existingCategory = await GetCategoryByNameAsync(category.CategoryName);
        if (existingCategory != null)
            throw new InvalidOperationException($"Category with name '{category.CategoryName}' already exists");

        return await _categoryDAO.CreateCategoryAsync(category);
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        if (category.CategoryId <= 0)
            throw new ArgumentException("Invalid Category ID", nameof(category));

        if (string.IsNullOrWhiteSpace(category.CategoryName))
            throw new ArgumentException("Category name is required", nameof(category));

        var existingCategory = await _categoryDAO.GetCategoryByIdAsync(category.CategoryId);
        if (existingCategory == null)
            throw new InvalidOperationException($"Category with ID {category.CategoryId} not found");

        return await _categoryDAO.UpdateCategoryAsync(category);
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        if (categoryId <= 0)
            throw new ArgumentException("Invalid Category ID", nameof(categoryId));

        var category = await _categoryDAO.GetCategoryByIdAsync(categoryId);
        if (category != null && category.Orchids.Any())
            throw new InvalidOperationException("Cannot delete category that contains orchids");

        return await _categoryDAO.DeleteCategoryAsync(categoryId);
    }

    public async Task<bool> IsCategoryExistsAsync(int categoryId)
    {
        return await _categoryDAO.IsCategoryExistsAsync(categoryId);
    }

    public async Task<Category?> GetCategoryByNameAsync(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            return null;

        var allCategories = await _categoryDAO.GetAllCategoriesAsync();
        return allCategories.FirstOrDefault(c => 
            string.Equals(c.CategoryName, categoryName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithOrchidsAsync()
    {
        var allCategories = await _categoryDAO.GetAllCategoriesAsync();
        return allCategories.Where(c => c.Orchids.Any());
    }
}