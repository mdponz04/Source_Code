using BusinessObjects.Models;
using BusinessObjects;
using DataAccess.IDAO;
using Microsoft.EntityFrameworkCore;
using DataAccess.Enums;

namespace DataAccess.DAO;

public class OrderDAO : IOrderDAO
{
    private readonly MyStoreDbContext _context;

    public OrderDAO(MyStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Account)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Orchid)
                .Where(o => o.OrderStatus != OrderStatus.DELETED.ToString())
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Where(o => o.OrderStatus != OrderStatus.DELETED.ToString())
            .Include(o => o.Account)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Orchid)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(int accountId)
    {
        return await _context.Orders
            .Include(o => o.Account)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Orchid)
            .Where(o => o.AccountId == accountId && o.OrderStatus != OrderStatus.DELETED.ToString())
            .ToListAsync();
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsOrderExistsAsync(int orderId)
    {
        return await _context.Orders.AnyAsync(o => o.Id == orderId);
    }
}