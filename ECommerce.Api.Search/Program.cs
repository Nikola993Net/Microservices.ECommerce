
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Services;
using Polly;

namespace ECommerce.Api.Search
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<ISearchService, SearchService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped<IProductsService, ProductsService>();
            builder.Services.AddScoped<ICustomersService, CustomersService>();
            builder.Services.AddHttpClient("CustomersService", config =>
            {
                var customerSerivceUrl = builder.Configuration["Services:Customers"];
                config.BaseAddress = new Uri(customerSerivceUrl);
            });
            builder.Services.AddHttpClient("OrdersService", config =>
            {
                var ordersServiceUrl = builder.Configuration["Services:Orders"];
                config.BaseAddress = new Uri(ordersServiceUrl);
            });
            builder.Services.AddHttpClient("ProductsService", config =>
            {
                var productsServiceUrl = builder.Configuration["Services:Products"];
                config.BaseAddress = new Uri(productsServiceUrl);
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));
            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
