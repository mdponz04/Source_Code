using BusinessObjects.Models;
using DataAccess.IDAO;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class OrderDetailRepo : IOrderDetailRepo
{
    private readonly IOrderDetailDAO _orderDetailDAO;

    public OrderDetailRepo(IOrderDetailDAO orderDetailDAO)
    {
        _orderDetailDAO = orderDetailDAO;
    }

    public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
    {
        return await _orderDetailDAO.GetAllOrderDetailsAsync();
    }

    public async Task<OrderDetail?> GetOrderDetailByIdAsync(int orderDetailId)
    {
        if (orderDetailId <= 0)
            throw new ArgumentException("Invalid OrderDetail ID", nameof(orderDetailId));

        return await _orderDetailDAO.GetOrderDetailByIdAsync(orderDetailId);
    }

    public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentException("Invalid Order ID", nameof(orderId));

        return await _orderDetailDAO.GetOrderDetailsByOrderIdAsync(orderId);
    }

    public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrchidIdAsync(int orchidId)
    {
        if (orchidId <= 0)
            throw new ArgumentException("Invalid Orchid ID", nameof(orchidId));

        return await _orderDetailDAO.GetOrderDetailsByOrchidIdAsync(orchidId);
    }

    public async Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail)
    {
        if (orderDetail.OrderId <= 0)
            throw new ArgumentException("Valid Order ID is required", nameof(orderDetail));

        if (orderDetail.OrchidId <= 0)
            throw new ArgumentException("Valid Orchid ID is required", nameof(orderDetail));

        if (orderDetail.Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(orderDetail));

        if (orderDetail.Price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(orderDetail));

        return await _orderDetailDAO.CreateOrderDetailAsync(orderDetail);
    }

    public async Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail)
    {
        if (orderDetail.Id <= 0)
            throw new ArgumentException("Invalid OrderDetail ID", nameof(orderDetail));

        if (orderDetail.Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(orderDetail));

        if (orderDetail.Price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(orderDetail));

        var existingOrderDetail = await _orderDetailDAO.GetOrderDetailByIdAsync(orderDetail.Id);
        if (existingOrderDetail == null)
            throw new InvalidOperationException($"OrderDetail with ID {orderDetail.Id} not found");

        return await _orderDetailDAO.UpdateOrderDetailAsync(orderDetail);
    }

    public async Task<bool> DeleteOrderDetailAsync(int orderDetailId)
    {
        if (orderDetailId <= 0)
            throw new ArgumentException("Invalid OrderDetail ID", nameof(orderDetailId));

        return await _orderDetailDAO.DeleteOrderDetailAsync(orderDetailId);
    }

    public async Task<bool> IsOrderDetailExistsAsync(int orderDetailId)
    {
        return await _orderDetailDAO.IsOrderDetailExistsAsync(orderDetailId);
    }

    public async Task<decimal> CalculateOrderTotalAsync(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentException("Invalid Order ID", nameof(orderId));

        var orderDetails = await _orderDetailDAO.GetOrderDetailsByOrderIdAsync(orderId);
        return orderDetails.Where(od => od.Price.HasValue && od.Quantity.HasValue)
                          .Sum(od => od.Price.Value * od.Quantity.Value);
    }

    public async Task<IEnumerable<OrderDetail>> GetTopSellingOrchidsAsync(int count)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be greater than zero", nameof(count));

        var allOrderDetails = await _orderDetailDAO.GetAllOrderDetailsAsync();
        return allOrderDetails
            .GroupBy(od => od.OrchidId)
            .OrderByDescending(g => g.Sum(od => od.Quantity ?? 0))
            .Take(count)
            .SelectMany(g => g)
            .DistinctBy(od => od.OrchidId);
    }

    public async Task<int> GetTotalQuantityByOrchidAsync(int orchidId)
    {
        if (orchidId <= 0)
            throw new ArgumentException("Invalid Orchid ID", nameof(orchidId));

        var orderDetails = await _orderDetailDAO.GetOrderDetailsByOrchidIdAsync(orchidId);
        return orderDetails.Where(od => od.Quantity.HasValue)
                          .Sum(od => od.Quantity.Value);
    }
}