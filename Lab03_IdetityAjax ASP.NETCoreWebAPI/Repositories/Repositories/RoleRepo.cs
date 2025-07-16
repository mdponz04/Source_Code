using BusinessObjects.Models;
using DataAccess.IDAO;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class RoleRepo : IRoleRepo
{
    private readonly IRoleDAO _roleDAO;

    public RoleRepo(IRoleDAO roleDAO)
    {
        _roleDAO = roleDAO;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _roleDAO.GetAllRolesAsync();
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        if (roleId <= 0)
            throw new ArgumentException("Invalid Role ID", nameof(roleId));

        return await _roleDAO.GetRoleByIdAsync(roleId);
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        if (string.IsNullOrWhiteSpace(role.RoleName))
            throw new ArgumentException("Role name is required", nameof(role));

        // Check if role with same name already exists
        var existingRole = await GetRoleByNameAsync(role.RoleName);
        if (existingRole != null)
            throw new InvalidOperationException($"Role with name '{role.RoleName}' already exists");

        return await _roleDAO.CreateRoleAsync(role);
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        if (role.RoleId <= 0)
            throw new ArgumentException("Invalid Role ID", nameof(role));

        if (string.IsNullOrWhiteSpace(role.RoleName))
            throw new ArgumentException("Role name is required", nameof(role));

        var existingRole = await _roleDAO.GetRoleByIdAsync(role.RoleId);
        if (existingRole == null)
            throw new InvalidOperationException($"Role with ID {role.RoleId} not found");

        return await _roleDAO.UpdateRoleAsync(role);
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        if (roleId <= 0)
            throw new ArgumentException("Invalid Role ID", nameof(roleId));

        var role = await _roleDAO.GetRoleByIdAsync(roleId);
        if (role != null && role.Accounts.Any())
            throw new InvalidOperationException("Cannot delete role that has associated accounts");

        return await _roleDAO.DeleteRoleAsync(roleId);
    }

    public async Task<bool> IsRoleExistsAsync(int roleId)
    {
        return await _roleDAO.IsRoleExistsAsync(roleId);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return null;

        var allRoles = await _roleDAO.GetAllRolesAsync();
        return allRoles.FirstOrDefault(r => 
            string.Equals(r.RoleName, roleName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Role>> GetRolesWithAccountsAsync()
    {
        var allRoles = await _roleDAO.GetAllRolesAsync();
        return allRoles.Where(r => r.Accounts.Any());
    }

    public async Task<int> GetAccountCountByRoleAsync(int roleId)
    {
        if (roleId <= 0)
            throw new ArgumentException("Invalid Role ID", nameof(roleId));

        var role = await _roleDAO.GetRoleByIdAsync(roleId);
        return role?.Accounts.Count ?? 0;
    }
}