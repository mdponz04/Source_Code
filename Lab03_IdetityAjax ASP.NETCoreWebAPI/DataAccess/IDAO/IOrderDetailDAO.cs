using BusinessObjects.Models;

namespace DataAccess.IDAO;

public interface IOrderDetailDAO
{
    Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync();
    Task<OrderDetail?> GetOrderDetailByIdAsync(int orderDetailId);
    Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrchidIdAsync(int orchidId);
    Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail);
    Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail);
    Task<bool> DeleteOrderDetailAsync(int orderDetailId);
    Task<bool> IsOrderDetailExistsAsync(int orderDetailId);
}