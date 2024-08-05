using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider _ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            _ordersProvider = ordersProvider;
        }

        [HttpGet("{cutomerID}")]
        public async Task<IActionResult> GetOrderAsync(int cutomerID)
        {
            var result = await _ordersProvider.GetOrdersAsync(cutomerID);
            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }

    }
}
