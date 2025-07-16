using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Repositories.DTOs;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderRepo _orderRepo;

    public OrderController(IOrderRepo orderRepo)
    {
        _orderRepo = orderRepo;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving orders", error = ex.Message });
        }
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        try
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound(new { message = $"Order with ID {id} not found" });

            return Ok(order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the order", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var order = new Order
            {
                TotalAmount = request.TotalAmount
            };

            var createdOrder = await _orderRepo.CreateOrderAsync(order);
            return Ok(createdOrder);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the order", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing order
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var existingOrder = await _orderRepo.GetOrderByIdAsync(id);
            if (existingOrder == null)
                return NotFound(new { message = $"Order with ID {id} not found" });

            existingOrder.OrderStatus = request.OrderStatus;
            existingOrder.OrderDetails = request.OrderDetails;
            existingOrder.TotalAmount = request.TotalAmount;

            var updatedOrder = await _orderRepo.UpdateOrderAsync(existingOrder);
            return Ok(new
            {
                id = updatedOrder.Id,
                accountId = updatedOrder.AccountId,
                orderDate = updatedOrder.OrderDate,
                orderStatus = updatedOrder.OrderStatus,
                totalAmount = updatedOrder.TotalAmount,
                message = "Order updated successfully"
            });
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
            return StatusCode(500, new { message = "An error occurred while updating the order", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete an order
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            var orderExists = await _orderRepo.IsOrderExistsAsync(id);
            if (!orderExists)
                return NotFound(new { message = $"Order with ID {id} not found" });

            var deleted = await _orderRepo.DeleteOrderAsync(id);
            if (deleted)
                return Ok(new { message = "Order deleted successfully" });
            else
                return StatusCode(500, new { message = "Failed to delete order" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the order", error = ex.Message });
        }
    }

}


