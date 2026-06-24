using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _context;

    public OrdersController(OrdersDbContext context)
    {
        _context = context;
    }

    // POST api/orders - Create new order
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (request?.Items == null || request.Items.Count == 0)
            return BadRequest("Order must contain at least one item");

        var order = new Order
        {
            UserId = request.UserId,
            PickupPointId = request.PickupPointId,
            DeliveryDate = request.DeliveryDate,
            Status = OrderStatus.Pending,
            TotalAmount = 0
        };

        decimal totalAmount = 0;
        var orderItems = new List<OrderItem>();

        foreach (var item in request.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null)
                return BadRequest($"Product with ID {item.ProductId} not found");

            if (product.Stock < item.Quantity)
                return BadRequest($"Insufficient stock for product {product.Name}");

            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                Subtotal = product.Price * item.Quantity
            };

            orderItems.Add(orderItem);
            totalAmount += orderItem.Subtotal;

            // Reduce stock
            product.Stock -= item.Quantity;
        }

        order.TotalAmount = totalAmount;
        order.OrderItems = orderItems;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    // GET api/orders - Get all orders with optional status filter
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromQuery] string? status)
    {
        var query = _context.Orders.Include(o => o.User)
            .Include(o => o.PickupPoint)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            if (Enum.TryParse<OrderStatus>(status, ignoreCase: true, out var orderStatus))
            {
                query = query.Where(o => o.Status == orderStatus);
            }
        }

        return await query.OrderByDescending(o => o.OrderDate).ToListAsync();
    }

    // GET api/orders/{id} - Get order details
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.PickupPoint)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound("Order not found");

        return order;
    }

    // PATCH api/orders/{id}/status - Change order status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound("Order not found");

        if (!Enum.TryParse<OrderStatus>(request.Status, ignoreCase: true, out var newStatus))
            return BadRequest($"Invalid status. Valid values: {string.Join(", ", Enum.GetNames(typeof(OrderStatus)))}");

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return Ok(order);
    }

    // DELETE api/orders/{id} - Cancel order
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    await transaction.RollbackAsync();
                    return NotFound("Order not found");
                }

                // Return products to stock if order not completed
                if (order.Status != OrderStatus.Completed && order.OrderItems != null)
                {
                    foreach (var item in order.OrderItems)
                    {
                        await _context.Database.ExecuteSqlInterpolatedAsync(
                            $"UPDATE products SET stock = stock + {item.Quantity} WHERE id = {item.ProductId}"
                        );
                    }
                }

                order.Status = OrderStatus.Cancelled;
                order.UpdatedAt = DateTime.UtcNow;
                _context.Orders.Update(order);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Order cancelled successfully", orderId = id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { error = "Failed to cancel order", details = ex.Message });
            }
        }
    }
}

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public int PickupPointId { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public List<OrderItemRequest>? Items { get; set; }
}

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateStatusRequest
{
    public string Status { get; set; } = string.Empty;
}
