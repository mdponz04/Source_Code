using BusinessObjects.Models;

namespace DataAccess.IDAO;

public interface IRoleDAO
{
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role?> GetRoleByIdAsync(int roleId);
    Task<Role> CreateRoleAsync(Role role);
    Task<Role> UpdateRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(int roleId);
    Task<bool> IsRoleExistsAsync(int roleId);
}