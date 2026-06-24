using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<PickupPoint> PickupPoints { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map table names to match database schema
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<PickupPoint>().ToTable("pickup_points");
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<OrderItem>().ToTable("order_items");

        // Map column names for User
        modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("id");
        modelBuilder.Entity<User>().Property(u => u.Name).HasColumnName("name");
        modelBuilder.Entity<User>().Property(u => u.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(u => u.Phone).HasColumnName("phone");
        modelBuilder.Entity<User>().Property(u => u.Address).HasColumnName("address");
        modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasColumnName("created_at");

        // Map column names for PickupPoint
        modelBuilder.Entity<PickupPoint>().Property(p => p.Id).HasColumnName("id");
        modelBuilder.Entity<PickupPoint>().Property(p => p.Name).HasColumnName("name");
        modelBuilder.Entity<PickupPoint>().Property(p => p.Address).HasColumnName("address");
        modelBuilder.Entity<PickupPoint>().Property(p => p.Phone).HasColumnName("phone");
        modelBuilder.Entity<PickupPoint>().Property(p => p.WorkingHours).HasColumnName("working_hours");
        modelBuilder.Entity<PickupPoint>().Property(p => p.CreatedAt).HasColumnName("created_at");

        // Map column names for Product
        modelBuilder.Entity<Product>().Property(p => p.Id).HasColumnName("id");
        modelBuilder.Entity<Product>().Property(p => p.Name).HasColumnName("name");
        modelBuilder.Entity<Product>().Property(p => p.Description).HasColumnName("description");
        modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnName("price");
        modelBuilder.Entity<Product>().Property(p => p.Stock).HasColumnName("stock");
        modelBuilder.Entity<Product>().Property(p => p.CreatedAt).HasColumnName("created_at");

        // Map column names for Order
        modelBuilder.Entity<Order>().Property(o => o.Id).HasColumnName("id");
        modelBuilder.Entity<Order>().Property(o => o.UserId).HasColumnName("user_id");
        modelBuilder.Entity<Order>().Property(o => o.PickupPointId).HasColumnName("pickup_point_id");
        modelBuilder.Entity<Order>().Property(o => o.OrderDate).HasColumnName("order_date");
        modelBuilder.Entity<Order>().Property(o => o.DeliveryDate).HasColumnName("delivery_date");
        modelBuilder.Entity<Order>().Property(o => o.Status).HasColumnName("status").HasConversion<string>();
        modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasColumnName("total_amount");
        modelBuilder.Entity<Order>().Property(o => o.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<Order>().Property(o => o.UpdatedAt).HasColumnName("updated_at");

        // Map column names for OrderItem
        modelBuilder.Entity<OrderItem>().Property(oi => oi.Id).HasColumnName("id");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.OrderId).HasColumnName("order_id");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.ProductId).HasColumnName("product_id");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.Quantity).HasColumnName("quantity");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasColumnName("unit_price");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.Subtotal).HasColumnName("subtotal");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.CreatedAt).HasColumnName("created_at");

        // Configure relationships
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.PickupPoint)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.PickupPointId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
