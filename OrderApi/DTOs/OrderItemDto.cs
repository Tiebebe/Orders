using Domain.Models;

namespace OrderApi.DTOs;

public class OrderItemDto : NewOrderItemDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    public OrderItemDto(OrderItem orderItem)
    {
        Id = orderItem.Id;
        OrderId = orderItem.OrderId;
        ProductId = orderItem.ProductId;
        UnitPrice = orderItem.UnitPrice;
        Quantity = orderItem.Quantity;
    }
}