﻿namespace DemoMassTransitAspnetcore.Dto
{
    public class EventDto
    {
        public DateTime MessageDate { get; set; }
        public string MessageContent { get; set; }
        public string MessageOrigin { get; set; }
    }
}