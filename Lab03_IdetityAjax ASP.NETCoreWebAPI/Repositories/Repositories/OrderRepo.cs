using BusinessObjects.Models;
using DataAccess.Enums;
using DataAccess.IDAO;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class OrderRepo : IOrderRepo
{
    private readonly IOrderDAO _orderDAO;

    public OrderRepo(IOrderDAO orderDAO)
    {
        _orderDAO = orderDAO;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderDAO.GetAllOrdersAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentException("Invalid Order ID", nameof(orderId));

        return await _orderDAO.GetOrderByIdAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(int accountId)
    {
        if (accountId <= 0)
            throw new ArgumentException("Invalid Account ID", nameof(accountId));

        return await _orderDAO.GetOrdersByAccountIdAsync(accountId);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    { 
        if (order.TotalAmount <= 0)
            throw new ArgumentException("Order total amount must be greater than zero", nameof(order));
        order.OrderDate = DateTime.Now;
        order.OrderStatus = OrderStatus.PENDING.ToString();
        return await _orderDAO.CreateOrderAsync(order);
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        if (order.Id <= 0)
            throw new ArgumentException("Invalid Order ID", nameof(order));

        if (order.TotalAmount <= 0)
            throw new ArgumentException("Order total amount must be greater than zero", nameof(order));

        var existingOrder = await _orderDAO.GetOrderByIdAsync(order.Id);
        if (existingOrder == null)
            throw new InvalidOperationException($"Order with ID {order.Id} not found");

        return await _orderDAO.UpdateOrderAsync(order);
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentException("Invalid Order ID", nameof(orderId));

        var order = await GetOrderByIdAsync(orderId);
        order.OrderStatus = OrderStatus.DELETED.ToString();

        if (order == null)
            throw new InvalidOperationException($"Order with ID {orderId} not found");
        await _orderDAO.UpdateOrderAsync(order);
        return true;
    }

    public async Task<bool> IsOrderExistsAsync(int orderId)
    {
        return await _orderDAO.IsOrderExistsAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Order status is required", nameof(status));

        var allOrders = await _orderDAO.GetAllOrdersAsync();
        return allOrders.Where(o => 
            string.Equals(o.OrderStatus, status, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be greater than end date");

        var allOrders = await _orderDAO.GetAllOrdersAsync();
        return allOrders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate);
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        var allOrders = await _orderDAO.GetAllOrdersAsync();
        return allOrders.Where(o => o.TotalAmount.HasValue)
                       .Sum(o => o.TotalAmount.Value);
    }

    public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
    {
        return await GetOrderByIdAsync(orderId);
    }

}
