using Domain.Models;

namespace OrderApi.DTOs;

public class OrderDto
{
    public OrderStatus Status { get; set; }
    public int? OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerId { get; set; }
    public ShippingAddressDto ShippingAddress { get; set; }
    public IList<OrderItemDto> Items { get; set; }
    public OrderDto(Order order)
    {
        Status = order.Status;
        OrderDate = order.OrderDate;
        ShippingAddress = new ShippingAddressDto(order.ShippingAddress);
        CustomerId = order.CustomerId;
        Items = order.Items?.Select(oi => new OrderItemDto(oi)).ToList();
        OrderId = order.Id;
        TotalAmount = order.TotalAmount;
    }
}
