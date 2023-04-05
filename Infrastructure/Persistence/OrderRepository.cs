using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersContext _context;

    public OrderRepository(OrdersContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders.Include(o => o.Items).ToListAsync();
    }

    public async Task<Order> GetByIdAsync(int id) => await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Order> AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateOrder(Order order)
    {
        var originalOrder = await _context.Orders.FindAsync(order.Id);

        if (originalOrder == null)
        {
            throw new Exception("Order not found");
        }

        _context.Entry(originalOrder).CurrentValues.SetValues(order);

        await _context.SaveChangesAsync();

        return originalOrder;
    }

    public async Task DeleteAllOrders()
    {
        //cascade delete is enabled.  This will delete OrderItems as well
        var orders = await _context.Orders.ToListAsync();
        _context.Orders.RemoveRange(orders);
        await _context.SaveChangesAsync();
    }
}
