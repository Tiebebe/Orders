using Microsoft.Data.Sqlite;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Application.Contracts;
using Application.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ApplicationTests;

public class TestFixture : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly OrdersContext DbContext;
    public ServiceProvider ServiceProvider { get; private set; }
    public TestFixture()
    {
        _connection = new SqliteConnection("datasource=:memory:");
        _connection.Open();
        var services = new ServiceCollection();
        services.AddDbContext<OrdersContext>(options => options.UseSqlite(_connection));
        services.AddScoped<ICacheService, MemeCacheService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrdersService, OrderService>();

        services.AddSingleton<IMemoryCache>(provider =>
        {
            var options = new MemoryCacheOptions();
            return new MemoryCache(options);
        });

        ServiceProvider = services.BuildServiceProvider();
    }
    public void Dispose()
    {
        _connection.Close();
    }
}

