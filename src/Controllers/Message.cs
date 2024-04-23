namespace DemoMassTransitAspnetcore.Controllers
{
    public class Message
    {
        public DateTime Date { get; set; }
        public MessageType Type { get; set; }
        public string Content { get; set; }
    }

    public enum MessageType
    {
        Event,
        Command
    }
}
