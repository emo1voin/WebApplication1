namespace WebApplication1.Models;

public enum OrderStatus
{
    Pending,
    Processing,
    Ready,
    Completed,
    Cancelled
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PickupPointId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveryDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User? User { get; set; }
    public PickupPoint? PickupPoint { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; }
}
