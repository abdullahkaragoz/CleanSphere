using App.Domain.Events;
using MassTransit;

namespace App.Bus.Consumers;
public class ProductAddEventConsumer() : IConsumer<ProductAddedEvent>
{
    public Task Consume(ConsumeContext<ProductAddedEvent> context)
    {
        Console.WriteLine($"Gelen Event:{context.Message.Id} - {context.Message.Name} - {context.Message.Price}");

        return Task.CompletedTask;
    }
}
