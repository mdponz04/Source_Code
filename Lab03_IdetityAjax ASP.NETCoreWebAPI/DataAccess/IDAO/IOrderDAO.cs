using BusinessObjects.Models;

namespace DataAccess.IDAO;

public interface IOrderDAO
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(int accountId);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int orderId);
    Task<bool> IsOrderExistsAsync(int orderId);
}