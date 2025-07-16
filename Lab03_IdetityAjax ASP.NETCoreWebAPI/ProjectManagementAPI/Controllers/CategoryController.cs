using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "1,2")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepo _categoryRepo;

    public CategoryController(ICategoryRepo categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync();
            return Ok(categories.Select(c => new
            {
                categoryId = c.CategoryId,
                categoryName = c.CategoryName,
                orchidCount = c.Orchids?.Count ?? 0
            }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving categories", error = ex.Message });
        }
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        try
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found" });

            return Ok(category);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the category", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] string categoryName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var category = new Category
            {
                CategoryName = categoryName
            };

            var createdCategory = await _categoryRepo.CreateCategoryAsync(category);
            return Ok(createdCategory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the category", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] string categoryName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var existingCategory = await _categoryRepo.GetCategoryByIdAsync(id);
            if (existingCategory == null)
                return NotFound(new { message = $"Category with ID {id} not found" });

            existingCategory.CategoryName = categoryName;

            var updatedCategory = await _categoryRepo.UpdateCategoryAsync(existingCategory);
            return Ok(updatedCategory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the category", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var categoryExists = await _categoryRepo.IsCategoryExistsAsync(id);
            if (!categoryExists)
                return NotFound(new { message = $"Category with ID {id} not found" });

            var deleted = await _categoryRepo.DeleteCategoryAsync(id);
            if (deleted)
                return Ok(new { message = "Category deleted successfully" });
            else
                return StatusCode(500, new { message = "Failed to delete category" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the category", error = ex.Message });
        }
    }
}
