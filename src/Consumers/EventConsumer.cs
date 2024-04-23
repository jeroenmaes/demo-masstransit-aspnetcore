using DemoMassTransitAspnetcore.Dto;
using MassTransit;

namespace DemoMassTransitAspnetcore.MessageConsumers
{
    public class EventConsumer :
    IConsumer<EventDto>
    {
        readonly ILogger<EventConsumer> _logger;

        public EventConsumer(ILogger<EventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<EventDto> context)
        {
            _logger.LogInformation("Received Event: {MessageContent}, Origin: {MessageOrigin}, Timestamp: {MessageDate}", context.Message.MessageContent, context.Message.MessageOrigin, context.Message.MessageDate);

            // Consume from bus and do the actual work (SyncDataForSpecificId)

            return Task.CompletedTask;
        }
    }
}
