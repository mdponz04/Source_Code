using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Repositories.DTOs;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailRepo _orderDetailRepo;

    public OrderDetailController(IOrderDetailRepo orderDetailRepo)
    {
        _orderDetailRepo = orderDetailRepo;
    }

    /// <summary>
    /// Get all order details
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllOrderDetails()
    {
        try
        {
            var orderDetails = await _orderDetailRepo.GetAllOrderDetailsAsync();
            return Ok(orderDetails);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving order details", error = ex.Message });
        }
    }

    /// <summary>
    /// Get order detail by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetailById(int id)
    {
        try
        {
            var orderDetail = await _orderDetailRepo.GetOrderDetailByIdAsync(id);
            if (orderDetail == null)
                return NotFound(new { message = $"Order detail with ID {id} not found" });

            return Ok(new
            {
                id = orderDetail.Id,
                orderId = orderDetail.OrderId,
                orchidId = orderDetail.OrchidId,
                orchidName = orderDetail.Orchid?.OrchidName,
                quantity = orderDetail.Quantity,
                price = orderDetail.Price,
                totalPrice = (orderDetail.Quantity ?? 0) * (orderDetail.Price ?? 0),
                order = orderDetail.Order != null ? new
                {
                    id = orderDetail.Order.Id,
                    orderDate = orderDetail.Order.OrderDate,
                    orderStatus = orderDetail.Order.OrderStatus
                } : null
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the order detail", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new order detail
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrderDetail([FromBody] CreateOrderDetailRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var orderDetail = new OrderDetail
            {
                OrchidId = request.OrchidId,
                Quantity = request.Quantity,
                Price = request.Price,
                OrderId = request.OrderId
            };

            var createdOrderDetail = await _orderDetailRepo.CreateOrderDetailAsync(orderDetail);
            return Ok(createdOrderDetail);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the order detail", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing order detail
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] UpdateOrderDetailRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var existingOrderDetail = await _orderDetailRepo.GetOrderDetailByIdAsync(id);
            if (existingOrderDetail == null)
                return NotFound(new { message = $"Order detail with ID {id} not found" });

            existingOrderDetail.OrchidId = request.OrchidId;
            existingOrderDetail.Quantity = request.Quantity;
            existingOrderDetail.Price = request.Price;


            var updatedOrderDetail = await _orderDetailRepo.UpdateOrderDetailAsync(existingOrderDetail);
            return Ok(updatedOrderDetail);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the order detail", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete an order detail
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderDetail(int id)
    {
        try
        {
            var orderDetailExists = await _orderDetailRepo.IsOrderDetailExistsAsync(id);
            if (!orderDetailExists)
                return NotFound(new { message = $"Order detail with ID {id} not found" });

            var deleted = await _orderDetailRepo.DeleteOrderDetailAsync(id);
            if (deleted)
                return Ok(new { message = "Order detail deleted successfully" });
            else
                return StatusCode(500, new { message = "Failed to delete order detail" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the order detail", error = ex.Message });
        }
    }
}
