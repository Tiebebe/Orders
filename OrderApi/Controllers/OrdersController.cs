using Microsoft.AspNetCore.Mvc;
using Application.Contracts;
using Domain.Models;
using OrderApi.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace OrderApi.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _service;

    public OrdersController(IOrdersService service)
    {
        _service = service;
    }

    [HttpPost()]
    [ProducesResponseType(typeof(Order), 200)]
    public async Task<IActionResult> CreateOrder([FromBody] NewOrderDto orderDto)
    {
        if (!orderDto.IsValid(out string errorMessage))
        {
            return BadRequest(errorMessage);
        };

        var order = await _service.AddOrder(orderDto.ToOrder());

        return CreatedAtRoute("Show", new { orderId = order.Id }, new OrderDto(order));
    }


    [HttpGet("{orderId}", Name = "Show")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    public async Task<ActionResult<OrderDto>> GetOrder(int orderId)
    {
        var order = await _service.GetOrder(orderId);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(new OrderDto(order));
    }
    [HttpGet()]
    [ProducesResponseType(typeof(List<OrderDto>), 200)]
    public async Task<ActionResult<OrderDto>> GetOrders()
    {
        var orders = await _service.GetOrders();

        if (orders == null)
        {
            return NotFound();
        }

        return Ok(orders.Select(o => new OrderDto(o)).ToList());
    }

    [HttpPatch("{orderId}")]
    public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] JsonPatchDocument<Order> patchDocument)
    {
        //Example request
        //[
        //  {
        //    "path": "/customerId",
        //    "op": "replace",
        //    "value": "newCustomerId"
        //  }
        //]
        //https://learn.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-7.0
        if (patchDocument is not null)
        {
            var order = await _service.GetOrder(orderId);

            if (order is null)
            {
                return NotFound();
            }
            ValidateUpdateRequest(patchDocument, order);

            patchDocument.ApplyTo(order);

            order = await _service.UpdateOrder(order);

            return Ok(new OrderDto(order));
        }
        else
        {
            return BadRequest("Can not accept null!");
        }
    }
    [HttpDelete]
    //[Authorize(Roles = "Admin")] // I have not implemented authorization but having such an endpoint does not seem like a good idea
    //even with authorization.
    public async Task<IActionResult> DeleteAllOrders()
    {
        await _service.DeleteAllOrders();
        return NoContent();      
    }

    private bool ValidateUpdateRequest(JsonPatchDocument<Order> patchDocument, Order order)
    {
        //some validation logic for the updates.  This is just one example
        var validStatuses = new[] { OrderStatus.Created, OrderStatus.ProcessThatAllowsEditing };

        return validStatuses.Contains(order.Status);
    }
}
