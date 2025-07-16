using BusinessObjects.Models;

namespace Repositories.DTOs;

public class CreateOrderRequestDTO
{
    public decimal TotalAmount { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
public class UpdateOrderRequestDTO
{
    public string OrderStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}
