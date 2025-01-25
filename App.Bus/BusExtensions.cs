using App.Application.ServiceBus;
using App.Bus.Consumers;
using App.Domain.Const;
using App.Domain.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Bus;
public static class BusExtensions
{
    public static void AddBusExt(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceBusOption = configuration.GetSection(nameof(ServiceBusOption)).Get<ServiceBusOption>();

        services.AddScoped<IServiceBus,ServiceBus>();

        services.AddMassTransit(services =>
        {
            services.AddConsumer<ProductAddEventConsumer>();

            services.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(serviceBusOption!.Url),
                h => { });

                cfg.ReceiveEndpoint(ServiceBusConst.ProductAddEventQueueName, e =>
                {
                    e.ConfigureConsumer<ProductAddEventConsumer>(context);
                });
            });
        });
    }
}
