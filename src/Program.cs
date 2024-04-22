
using DemoMassTransitAspnetcore.Cron;
using DemoMassTransitAspnetcore.MessageConsumers;
using DemoMassTransitAspnetcore.Messaging;
using MassTransit;
using System.Reflection;

namespace DemoMassTransitAspnetcore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // in-memory bus for send/receive
            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<EventConsumer>();
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });

            });

            // second bus for receive only fro Azure SB
            builder.Services.AddMassTransit<ISecondBus>(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<EventConsumer>();
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host("Endpoint=sb://xxx.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zzzz");
                    cfg.ReceiveEndpoint("events", cfg =>
                    {                        
                        cfg.ConfigureConsumeTopology = false;
                        cfg.ConfigureConsumer<EventConsumer>(context);
                    });
                    cfg.DefaultContentType = new System.Net.Mime.ContentType("application/json");

                    // use RAW serializer as messages don't originate from MassTransit!
                    cfg.UseNewtonsoftRawJsonDeserializer();                    
                });
            });

            builder.Services.AddHostedService<BackgroundJob>();

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
