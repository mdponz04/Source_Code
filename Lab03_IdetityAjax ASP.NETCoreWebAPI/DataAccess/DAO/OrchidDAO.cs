using BusinessObjects.Models;
using BusinessObjects;
using DataAccess.IDAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class OrchidDAO : IOrchidDAO
{
    private readonly MyStoreDbContext _context;

    public OrchidDAO(MyStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Orchid>> GetAllOrchidsAsync()
    {
        return await _context.Orchids
            .Include(o => o.Category)
            .Include(o => o.OrderDetails)
            .ToListAsync();
    }

    public async Task<Orchid?> GetOrchidByIdAsync(int orchidId)
    {
        return await _context.Orchids
            .Include(o => o.Category)
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrchidId == orchidId);
    }

    public async Task<IEnumerable<Orchid>> GetOrchidsByCategoryIdAsync(int categoryId)
    {
        return await _context.Orchids
            .Include(o => o.Category)
            .Where(o => o.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Orchid> CreateOrchidAsync(Orchid orchid)
    {
        _context.Orchids.Add(orchid);
        await _context.SaveChangesAsync();
        return orchid;
    }

    public async Task<Orchid> UpdateOrchidAsync(Orchid orchid)
    {
        _context.Orchids.Update(orchid);
        await _context.SaveChangesAsync();
        return orchid;
    }

    public async Task<bool> DeleteOrchidAsync(int orchidId)
    {
        var orchid = await _context.Orchids.FindAsync(orchidId);
        if (orchid == null)
            return false;

        _context.Orchids.Remove(orchid);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsOrchidExistsAsync(int orchidId)
    {
        return await _context.Orchids.AnyAsync(o => o.OrchidId == orchidId);
    }
}