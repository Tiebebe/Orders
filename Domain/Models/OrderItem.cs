namespace Domain.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public string ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

}

