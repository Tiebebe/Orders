using Domain.Models;

namespace OrderApi.DTOs;

public class NewOrderItemDto
{
    public string ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public OrderItem ToOrderItem()
    {
        return new OrderItem { ProductId = ProductId, UnitPrice = UnitPrice, Quantity = Quantity };
    }
}
