using BusinessObjects.Models;

namespace Repositories.Interfaces;

public interface IOrderRepo
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(int accountId);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int orderId);
    Task<bool> IsOrderExistsAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalRevenueAsync();
    Task<Order?> GetOrderWithDetailsAsync(int orderId);
}