using BusinessObjects.Models;
using BusinessObjects;
using DataAccess.IDAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class RoleDAO : IRoleDAO
{
    private readonly MyStoreDbContext _context;

    public RoleDAO(MyStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles
            .Include(r => r.Accounts)
            .ToListAsync();
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _context.Roles
            .Include(r => r.Accounts)
            .FirstOrDefaultAsync(r => r.RoleId == roleId);
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        var role = await _context.Roles.FindAsync(roleId);
        if (role == null)
            return false;

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsRoleExistsAsync(int roleId)
    {
        return await _context.Roles.AnyAsync(r => r.RoleId == roleId);
    }
}