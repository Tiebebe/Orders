using Application.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Infrastructure.Persistence;
using Application.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefualtConnectionString");
var connection = new SqliteConnection("datasource=:memory:");
connection.Open();


if (builder.Environment.IsDevelopment())
{

    builder.Services.AddDbContext<OrdersContext>(options => options.UseSqlite(connection));
    builder.Services.AddSingleton<IMemoryCache, MemoryCache>();  
    builder.Services.AddScoped<ICacheService, MemeCacheService>();  
}
else
{
    builder.Services.AddDbContext<OrdersContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddScoped<ICacheService, RedisCacheService>();  
}


builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrdersService, OrderService>();

builder.Services.AddControllers().AddNewtonsoftJson(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enforce HTTPS
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
        options.HttpsPort = 443;
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

