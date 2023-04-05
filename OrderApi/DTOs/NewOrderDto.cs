using Domain.Models;

namespace OrderApi.DTOs;

public class NewOrderDto
{
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerId { get; set; }
    public ShippingAddressDto ShippingAddress { get; set; }
    public IList<NewOrderItemDto> Items { get; set; }
    public Order ToOrder()
    {
        return new Order
        {
            Status = OrderStatus.Created,
            OrderDate = DateTime.UtcNow,
            ShippingAddress = ShippingAddress.ToString(),
            CustomerId = CustomerId,
            Items = Items.Select(i => i.ToOrderItem()).ToList()
        };
    }
    public bool IsValid(out string errorMessage)
    {
        errorMessage = string.Empty;

        if (Items == null || Items.Count == 0)
        {
            errorMessage += "The order must contain at least one item.";
        }

        if (ShippingAddress == null)
        {
            errorMessage += "The order must contain at valid shipping address.";
        }
        //Lägga till alla validation som behövs enligt verksamhets regler. 

        return string.IsNullOrEmpty(errorMessage);
    }

}
