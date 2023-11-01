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
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<OrderConsumer>();
                        var serviceBusSettings = hostContext.Configuration.GetSection("ServiceBus").Get<ServiceBusSettings>();
                        x.UsingAzureServiceBus((context, cfg) =>
                        {
                            cfg.Host(serviceBusSettings.ConnectionString);
                            cfg.Message<Order>(m => m.SetEntityName(serviceBusSettings.TopicName));
                            cfg.SubscriptionEndpoint<Order>(serviceBusSettings.SubscriptionName, e =>
                            {
                                e.ConfigureConsumer<OrderConsumer>(context);

                            });

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
        }
    }
}