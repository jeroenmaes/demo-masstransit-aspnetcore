using DemoMassTransitAspnetcore.Dto;
using MassTransit;

namespace DemoMassTransitAspnetcore.MessageConsumers
{
    public class CommandConsumer :
    IConsumer<CommandDto>
    {
        readonly ILogger<EventConsumer> _logger;

        public CommandConsumer(ILogger<EventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<CommandDto> context)
        {
            _logger.LogInformation("Received Command: {MessageContent}, Origin: {MessageOrigin}, Timestamp: {MessageDate}", context.Message.MessageContent, context.Message.MessageOrigin, context.Message.MessageDate);

            // Consume from bus and do the actual work (SyncAllData)

            return Task.CompletedTask;
        }
    }
}
