using BusinessObjects.Models;

namespace Repositories.Interfaces;

public interface IOrderDetailRepo
{
    Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync();
    Task<OrderDetail?> GetOrderDetailByIdAsync(int orderDetailId);
    Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrchidIdAsync(int orchidId);
    Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail);
    Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail);
    Task<bool> DeleteOrderDetailAsync(int orderDetailId);
    Task<bool> IsOrderDetailExistsAsync(int orderDetailId);
    Task<decimal> CalculateOrderTotalAsync(int orderId);
    Task<IEnumerable<OrderDetail>> GetTopSellingOrchidsAsync(int count);
    Task<int> GetTotalQuantityByOrchidAsync(int orchidId);
}