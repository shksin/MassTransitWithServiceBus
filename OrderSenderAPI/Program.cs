using MassTransit;
using OrderContract;

namespace OrderSenderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           // Add MassTransit
            builder.Services.AddMassTransit(x =>
            {
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    var serviceBusSettings = builder.Configuration.GetSection("ServiceBus").Get<ServiceBusSettings>();
                    cfg.Host(serviceBusSettings.ConnectionString);
                    cfg.Message<Order>(m => m.SetEntityName(serviceBusSettings.TopicName));
                    
                });
            });
            
            // Add services to the container.
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}