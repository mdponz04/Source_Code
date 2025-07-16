using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Repositories.DTOs;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrchidController : ControllerBase
{
    private readonly IOrchidRepo _orchidRepo;

    public OrchidController(IOrchidRepo orchidRepo)
    {
        _orchidRepo = orchidRepo;
    }

    /// <summary>
    /// Get all orchids
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllOrchids()
    {
        try
        {
            var orchids = await _orchidRepo.GetAllOrchidsAsync();
            return Ok(orchids.Select(o => new
            {
                orchidId = o.OrchidId,
                orchidName = o.OrchidName,
                price = o.Price,
                isNatural = o.IsNatural,
                orchidDescription = o.OrchidDescription,
                orchidUrl = o.OrchidUrl,
                categoryId = o.CategoryId,
                categoryName = o.Category?.CategoryName
            }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving orchids", error = ex.Message });
        }
    }

    /// <summary>
    /// Get orchid by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrchidById(int id)
    {
        try
        {
            var orchid = await _orchidRepo.GetOrchidByIdAsync(id);
            if (orchid == null)
                return NotFound(new { message = $"Orchid with ID {id} not found" });

            return Ok(new
            {
                orchidId = orchid.OrchidId,
                orchidName = orchid.OrchidName,
                price = orchid.Price,
                isNatural = orchid.IsNatural,
                orchidDescription = orchid.OrchidDescription,
                orchidUrl = orchid.OrchidUrl,
                categoryId = orchid.CategoryId,
                categoryName = orchid.Category?.CategoryName
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the orchid", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new orchid
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrchid([FromBody] CreateOrchidRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var orchid = new Orchid
            {
                OrchidName = request.OrchidName,
                Price = request.Price,
                IsNatural = request.IsNatural,
                OrchidDescription = request.OrchidDescription,
                OrchidUrl = request.OrchidUrl,
                CategoryId = request.CategoryId
            };

            var createdOrchid = await _orchidRepo.CreateOrchidAsync(orchid);
            return CreatedAtAction(nameof(GetOrchidById), 
                new { id = createdOrchid.OrchidId }, 
                new
                {
                    orchidId = createdOrchid.OrchidId,
                    orchidName = createdOrchid.OrchidName,
                    price = createdOrchid.Price,
                    message = "Orchid created successfully"
                });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the orchid", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing orchid
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrchid(int id, [FromBody] UpdateOrchidRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var existingOrchid = await _orchidRepo.GetOrchidByIdAsync(id);
            if (existingOrchid == null)
                return NotFound(new { message = $"Orchid with ID {id} not found" });

            existingOrchid.OrchidName = request.OrchidName;
            existingOrchid.Price = request.Price;
            existingOrchid.IsNatural = request.IsNatural;
            existingOrchid.OrchidDescription = request.OrchidDescription;
            if(!String.IsNullOrWhiteSpace(request.OrchidUrl)) 
                existingOrchid.OrchidUrl = request.OrchidUrl;
            existingOrchid.CategoryId = request.CategoryId;


            var updatedOrchid = await _orchidRepo.UpdateOrchidAsync(existingOrchid);
            return Ok(new
            {
                orchidId = updatedOrchid.OrchidId,
                orchidName = updatedOrchid.OrchidName,
                price = updatedOrchid.Price,
                message = "Orchid updated successfully"
            });
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
            return StatusCode(500, new { message = "An error occurred while updating the orchid", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete an orchid
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrchid(int id)
    {
        try
        {
            var orchidExists = await _orchidRepo.IsOrchidExistsAsync(id);
            if (!orchidExists)
                return NotFound(new { message = $"Orchid with ID {id} not found" });

            var deleted = await _orchidRepo.DeleteOrchidAsync(id);
            if (deleted)
                return Ok(new { message = "Orchid deleted successfully" });
            else
                return StatusCode(500, new { message = "Failed to delete orchid" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the orchid", error = ex.Message });
        }
    }
}
