using MassTransit;
using OrderContract;
using Microsoft.AspNetCore.Mvc;
using System;

namespace OrderSenderAPI
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly string[] Items = new[]
        {
        "Book", "Pencil", "Pen", "Coffee", "Keyboard", "Remote", "Glasses", "Cables", "Clips", "Markers"
    };

        private readonly ILogger<OrderController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(ILogger<OrderController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;

            _publishEndpoint = publishEndpoint;
        }

        
        [HttpPost]
        public async Task<IActionResult> NewOrder()
        {
           var order = new Order
           {
                ItemId = Guid.NewGuid(),
                ItemName = Items[Random.Shared.Next(Items.Length)],
                Timestamp = DateTime.UtcNow
            };

            await _publishEndpoint.Publish(order);

            return Accepted(order);
        }
    }
}