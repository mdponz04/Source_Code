using BusinessObjects.Models;
using BusinessObjects;
using DataAccess.IDAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class OrderDetailDAO : IOrderDetailDAO
{
    private readonly MyStoreDbContext _context;

    public OrderDetailDAO(MyStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
    {
        return await _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Orchid)
            .ToListAsync();
    }

    public async Task<OrderDetail?> GetOrderDetailByIdAsync(int orderDetailId)
    {
        return await _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Orchid)
            .FirstOrDefaultAsync(od => od.Id == orderDetailId);
    }

    public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
    {
        return await _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Orchid)
            .Where(od => od.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrchidIdAsync(int orchidId)
    {
        return await _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Orchid)
            .Where(od => od.OrchidId == orchidId)
            .ToListAsync();
    }

    public async Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail)
    {
        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();
        return orderDetail;
    }

    public async Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail)
    {
        _context.OrderDetails.Update(orderDetail);
        await _context.SaveChangesAsync();
        return orderDetail;
    }

    public async Task<bool> DeleteOrderDetailAsync(int orderDetailId)
    {
        var orderDetail = await _context.OrderDetails.FindAsync(orderDetailId);
        if (orderDetail == null)
            return false;

        _context.OrderDetails.Remove(orderDetail);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsOrderDetailExistsAsync(int orderDetailId)
    {
        return await _context.OrderDetails.AnyAsync(od => od.Id == orderDetailId);
    }
}