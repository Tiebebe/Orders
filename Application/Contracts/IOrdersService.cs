using Domain.Models;

namespace Application.Contracts;

public interface IOrdersService
{
    Task<Order> AddOrder(Order order);
    Task DeleteAllOrders();
    Task<Order> GetOrder(int Id);
    Task<List<Order>> GetOrders();
    Task<Order> UpdateOrder(Order order);
}