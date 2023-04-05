using Domain.Models;

namespace Application.Contracts;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order> GetByIdAsync(int id);
    Task<Order> AddAsync(Order order);
    Task<Order> UpdateOrder(Order order);
    Task DeleteAllOrders();
}
