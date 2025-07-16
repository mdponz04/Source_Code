using BusinessObjects.Models;

namespace Repositories.Interfaces;

public interface IRoleRepo
{
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role?> GetRoleByIdAsync(int roleId);
    Task<Role> CreateRoleAsync(Role role);
    Task<Role> UpdateRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(int roleId);
    Task<bool> IsRoleExistsAsync(int roleId);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<IEnumerable<Role>> GetRolesWithAccountsAsync();
    Task<int> GetAccountCountByRoleAsync(int roleId);
}