using Application.Contracts;
using Domain.Models;

public class OrderService : IOrdersService
{
    private readonly IOrderRepository _ordersRepository;
    private readonly ICacheService _cacheService;

    public OrderService(IOrderRepository orderrepository, ICacheService cacheService)
    {
        _ordersRepository = orderrepository;
        _cacheService = cacheService;
    }

    public async Task<Order> AddOrder(Order order)
    {
        ValidateNewOrder(order);
        var addedOrder = await _ordersRepository.AddAsync(order);
        _cacheService.Remove($"Order_{addedOrder.Id}"); 
        //Better to have the cache timespan in appsettings
        _cacheService.Set($"Order_{addedOrder.Id}", addedOrder, TimeSpan.FromMinutes(30)); 
        return addedOrder;
    }

    private static void ValidateNewOrder(Order order)
    {
        var errors = string.Empty;
        //All validations according to business requirements
        //Can add logging
        if (order == null)
        {
            errors += "order is empty";
        }
        else if (order.Items == null || !order.Items.Any())
        {
            errors += "order items is empty";
        }
        if (!string.IsNullOrEmpty(errors))
        {
            Log(errors);
            throw new ArgumentException(errors);
        }
    }

    private static void Log(string logMessage)
    {
        System.Diagnostics.Debug.WriteLine($"{logMessage}");
    }

    public async Task<Order> GetOrder(int Id)
    {
        var cacheKey = $"Order_{Id}";
        if (_cacheService.TryGet(cacheKey, out Order cachedOrder))
        {
            return cachedOrder;
        }
        else
        {
            var dbOrder = await _ordersRepository.GetByIdAsync(Id);
            if (dbOrder != null)
            {
                _cacheService.Set(cacheKey, dbOrder, TimeSpan.FromMinutes(30));
            }
            return dbOrder;
        }
    }

    public async Task<List<Order>> GetOrders()
    {
        var cacheKey = "AllOrders";
        //It's  better to find a way to cache an order only once (by caching either only "all orders", or only 
        //individual orders depending on the size) but I leave it as it is for now. 
        if (_cacheService.TryGet(cacheKey, out List<Order> cachedOrders))
        {
            return cachedOrders;
        }
        else
        {
            var dbOrders = await _ordersRepository.GetAllAsync();
            if (dbOrders != null)
            {
                _cacheService.Set(cacheKey, dbOrders, TimeSpan.FromMinutes(30));
            }
            return dbOrders;
        }
    }

    public async Task<Order> UpdateOrder(Order order)
    {
        var updatedOrder = await _ordersRepository.UpdateOrder(order);
        _cacheService.Remove($"Order_{order.Id}"); 
        return updatedOrder;
    }

    public async Task DeleteAllOrders()
    {
        var allOrders = await _ordersRepository.GetAllAsync();
        var cacheKeys = allOrders.Select(o => $"Order_{o.Id}").ToList(); 

        await _ordersRepository.DeleteAllOrders();
        _cacheService.Remove("AllOrders");        
        foreach (var key in cacheKeys)
        {
            _cacheService.Remove(key);
        }
    }
}
