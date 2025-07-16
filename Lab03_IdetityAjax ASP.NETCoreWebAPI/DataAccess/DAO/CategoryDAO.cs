using BusinessObjects.Models;
using BusinessObjects;
using DataAccess.IDAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class CategoryDAO : ICategoryDAO
{
    private readonly MyStoreDbContext _context;

    public CategoryDAO(MyStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Include(c => c.Orchids)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId)
    {
        return await _context.Categories
            .Include(c => c.Orchids)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category == null)
            return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsCategoryExistsAsync(int categoryId)
    {
        return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
    }
}