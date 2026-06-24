namespace WpfApp1.Models;

public class OrderModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PickupPointId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public UserModel? User { get; set; }
    public PickupPointModel? PickupPoint { get; set; }
    public List<OrderItemModel>? OrderItems { get; set; }
}

public class UserModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }

    public override string ToString() => $"{Name} ({Email})";
}

public class ProductModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }

    public override string ToString() => $"{Name} ({Price}₽)";
}

public class PickupPointModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? WorkingHours { get; set; }
    public DateTime CreatedAt { get; set; }

    public override string ToString() => $"{Name} - {Address}";
}

public class OrderItemModel
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public DateTime CreatedAt { get; set; }

    public ProductModel? Product { get; set; }
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
