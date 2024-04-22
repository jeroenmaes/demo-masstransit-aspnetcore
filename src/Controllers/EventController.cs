using DemoMassTransitAspnetcore.Dto;
using DemoMassTransitAspnetcore.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace DemoMassTransitAspnetcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
       

        private readonly ILogger<EventController> _logger;
        private readonly IBus _bus;

        public EventController(ILogger<EventController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpPost(Name = "PostNewEvent")]
        public async Task<ActionResult> Post(EventMessage evt, CancellationToken token)
        {
            var dto = new EventDto { MessageDate = evt.Date, MessageText = evt.Text, MessageOrigin = "API" };
         
            //Receive API call and put event on bus
            
            await _bus.Publish(dto, token);

            return Ok();
        }
    }
}
