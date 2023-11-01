using MassTransit;
using OrderContract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderConsumerWorkerApp
{
    public class OrderConsumer : IConsumer<Order>
    {
        readonly ILogger _logger;

        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation("Order Received: {OrderId}", context.Message.ItemId);
            await Task.CompletedTask;
        }
    }
}
