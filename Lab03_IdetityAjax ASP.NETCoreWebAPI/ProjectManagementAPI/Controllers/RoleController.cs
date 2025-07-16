using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "1")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepo _roleRepo;

    public RoleController(IRoleRepo roleRepo)
    {
        _roleRepo = roleRepo;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _roleRepo.GetAllRolesAsync();
            return Ok(roles.Select(r => new
            {
                roleId = r.RoleId,
                roleName = r.RoleName,
                accountCount = r.Accounts?.Count ?? 0
            }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving roles", error = ex.Message });
        }
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(int id)
    {
        try
        {
            var role = await _roleRepo.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound(new { message = $"Role with ID {id} not found" });

            return Ok(role);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the role", error = ex.Message });
        }
    }
    /// <summary>
    /// Create a new role
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var role = new Role
            {
                RoleName = roleName
            };

            var createdRole = await _roleRepo.CreateRoleAsync(role);
            return CreatedAtAction(nameof(GetRoleById), 
                new { id = createdRole.RoleId }, 
                new
                {
                    roleId = createdRole.RoleId,
                    roleName = createdRole.RoleName,
                    message = "Role created successfully"
                });
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
            return StatusCode(500, new { message = "An error occurred while creating the role", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing role
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] string roleName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var existingRole = await _roleRepo.GetRoleByIdAsync(id);
            if (existingRole == null)
                return NotFound(new { message = $"Role with ID {id} not found" });

            existingRole.RoleName = roleName;

            var updatedRole = await _roleRepo.UpdateRoleAsync(existingRole);
            return Ok(new
            {
                roleId = updatedRole.RoleId,
                roleName = updatedRole.RoleName,
                message = "Role updated successfully"
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
            return StatusCode(500, new { message = "An error occurred while updating the role", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var roleExists = await _roleRepo.IsRoleExistsAsync(id);
            if (!roleExists)
                return NotFound(new { message = $"Role with ID {id} not found" });

            var deleted = await _roleRepo.DeleteRoleAsync(id);
            if (deleted)
                return Ok(new { message = "Role deleted successfully" });
            else
                return StatusCode(500, new { message = "Failed to delete role" });
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
            return StatusCode(500, new { message = "An error occurred while deleting the role", error = ex.Message });
        }
    }
}
