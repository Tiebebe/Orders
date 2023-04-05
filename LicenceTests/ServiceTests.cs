using Application.Contracts;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.DTOs;

namespace ApplicationTests;

public class ApplicationTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly IOrdersService _service;
    private readonly OrdersContext _context;

    public ApplicationTests(TestFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetService<IOrdersService>();
        _context = _fixture.ServiceProvider.GetService<OrdersContext>();
        //Better to test the API instead ?
    }

    [Fact]
    public async Task GetOrder_BySpecificIdentifier_ReturnsOrderCorrectly()
    {
        // Arrange
        var customer1Id = Guid.NewGuid().ToString();
        var order1 = CreateOrderWithItems(customer1Id);
        order1 = await _service.AddOrder(order1);

        // Arrange
        var customer2Id = Guid.NewGuid().ToString();
        var order2 = CreateOrderWithItems(customer1Id);
        order2 = await _service.AddOrder(order2);

        //Act
        var uniqueIdentifier = order1.Id;
        var newOrder = await _service.GetOrder(uniqueIdentifier);

        // Assert
        Assert.NotNull(newOrder);
        Assert.Equal(customer1Id, newOrder.CustomerId);

    }

    [Fact]
    public async Task WhenCreatingNewOrder_WithoutOrderItems_ThrowsArgumentException()
    {
        // Arrange
        var customerId = Guid.NewGuid().ToString();
        var order = CreateOrderWithoutItems(customerId);

        //Act
        Func<Task> act = () => _service.AddOrder(order); ;

        // Assert
        var exception = await Assert.ThrowsAnyAsync<ArgumentException>(act);

    }
    [Fact]
    public async Task WhenSeveralOrdersAreCreated_GetOrders_ReturnAllOrders()
    {
        // Arrange
        CleanDb();
        var numberOfOrdersToCreate = 10;
        for (var i = 0; i < numberOfOrdersToCreate; i++)
        {
            var customerId = Guid.NewGuid().ToString();
            var order = CreateOrderWithItems(customerId);
            order = await _service.AddOrder(order);
        }

        //Act

        var orders = await _service.GetOrders();

        // Assert
        Assert.Equal(numberOfOrdersToCreate, orders.Count);

    }
    [Fact]
    public async Task UpdatingCustomerId_OnAnExistingOrder_WorksProperly()
    {

        // Arrange
        CleanDb();
        var customerId = Guid.NewGuid().ToString();
        var customerId2 = Guid.NewGuid().ToString();
        var order1 = CreateOrderWithItems(customerId);
        order1 = await _service.AddOrder(order1);

        //Act
        order1.CustomerId = customerId2; 
        var order2 = await _service.UpdateOrder(order1);

        order1 = await _service.GetOrder(order1.Id); 

        // Assert
        Assert.Equal(order1.CustomerId, customerId2);

    }

    [Fact]
    public async Task DeletingAllOrders_Works()
    {
        CleanDb();
        // Arrange

        var numberOfOrdersToCreate = 10;
        for (var i = 0; i < numberOfOrdersToCreate; i++)
        {
            var customerId = Guid.NewGuid().ToString();
            var order = CreateOrderWithItems(customerId);
            order = await _service.AddOrder(order);
        }
        var orders = await _service.GetOrders();
        var ordersCount1 = orders.Count();
        //Act

        await _service.DeleteAllOrders();
        orders = await _service.GetOrders();
        var ordersCount2 = orders.Count();


        // Assert
        Assert.Equal(numberOfOrdersToCreate, ordersCount1);
        Assert.Equal(0, ordersCount2); 

    }

    private Order CreateOrderWithItems(string customerId)
    {
        var order = CreateOrderWithoutItems(customerId);
        order.Items = new List<OrderItem>
    {
        new OrderItem
        {
            ProductId = "5678",
            UnitPrice = 10.00m,
            Quantity = 5
        },
        new OrderItem
        {
            ProductId = "9012",
            UnitPrice = 25.00m,
            Quantity = 2
        }

        };
        return order;
    }

    private Order CreateOrderWithoutItems(string customerId)
    {
        var shippingAddress = new ShippingAddressDto
        {
            Street = "Storagatan 1",
            City = "Någotköping",
            ZipCode = "12345"
        };
        var order = new Order
        {
            Status = OrderStatus.Created,
            OrderDate = DateTime.UtcNow,
            TotalAmount = 100.00m,
            CustomerId = customerId,
            ShippingAddress = shippingAddress.ToString(),
        };

        return order;
    }
    private new void CleanDb()
    {
        _context.OrderItems.RemoveRange(_context.OrderItems);
        _context.Orders.RemoveRange(_context.Orders);
        _context.SaveChanges();
    }

}