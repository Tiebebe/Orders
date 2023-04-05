namespace Domain.Models;

public  class Order
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerId { get; set; }
    public string ShippingAddress { get; set; }
    public virtual IEnumerable<OrderItem> Items { get; set; }
}

