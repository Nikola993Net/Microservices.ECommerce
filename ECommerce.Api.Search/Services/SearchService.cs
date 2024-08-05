using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService _orderService;
        private readonly IProductsService _productsService;
        private readonly ICustomersService _customersService;
        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
        {
            _orderService = ordersService;
            _productsService = productsService;
            _customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var customerResult = await _customersService.GetCustomerAsync(customerId);
            var orderResult = await _orderService.GetOrderAsync(customerId);
            var productsResult = await _productsService.GetProductsAsync();
            
            if (orderResult.IsSuccess)
            {
                foreach (var order in orderResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess?
                            productsResult.Products.FirstOrDefault(z => z.Id == item.ProductId).Name
                            : "Product information is not available";
                    }
                }

                var result = new
                {
                    Customer = customerResult.IsSuccess ? customerResult.Customer :  new { Name = "Customer information is not available"},
                    Orders = orderResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
