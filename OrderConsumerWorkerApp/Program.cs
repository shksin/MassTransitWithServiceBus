using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OrderContract;
using System.Threading.Tasks;

namespace OrderConsumerWorkerApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<OrderConsumer>();
                    var serviceBusSettings = hostContext.Configuration.GetSection("ServiceBus").Get<ServiceBusSettings>();
                    x.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(serviceBusSettings.ConnectionString);

                        // Subscribe to OrderSubmitted directly on the topic, instead of configuring a queue
                        cfg.SubscriptionEndpoint<Order>(serviceBusSettings.SubscriptionName, e =>
                        {
                            e.ConfigureConsumer<OrderConsumer>(context);
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                });
            })
            .Build()
            .RunAsync();
        }

    }
}
    