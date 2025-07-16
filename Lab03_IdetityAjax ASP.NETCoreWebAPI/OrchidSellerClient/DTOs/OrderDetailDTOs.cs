namespace Repositories.DTOs;


public class CreateOrderDetailRequestDTO
{
    public int? OrchidId { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public int? OrderId { get; set; }
}
public class UpdateOrderDetailRequestDTO
{
    public int? OrchidId { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
}
