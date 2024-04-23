using DemoMassTransitAspnetcore.Dto;
using DemoMassTransitAspnetcore.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace DemoMassTransitAspnetcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
       

        private readonly ILogger<MessageController> _logger;
        private readonly IBus _bus;

        public MessageController(ILogger<MessageController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpPost(Name = "PostNewMessage")]
        public async Task<ActionResult> Post(Message evt, CancellationToken token)
        {
            //Receive API call and put event on bus

            if (evt.Type == MessageType.Event)
            {
                var dto = new EventDto { MessageDate = evt.Date, MessageContent = "Event"+evt.Content, MessageOrigin = "API" };
                await _bus.Publish(dto, token);
            }
            else
            {
                var dto = new CommandDto { MessageDate = evt.Date, MessageContent = "Command"+evt.Content, MessageOrigin = "API" };
                await _bus.Publish(dto, token);
            }
            

            return Ok();
        }
    }
}
