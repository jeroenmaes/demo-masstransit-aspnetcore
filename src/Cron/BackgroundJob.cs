using DemoMassTransitAspnetcore.Dto;
using DemoMassTransitAspnetcore.MessageConsumers;
using MassTransit;
using NCrontab;

namespace DemoMassTransitAspnetcore.Cron
{
    public class BackgroundJob : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private string Schedule => "*/1 * * * *"; //Runs every 1 minutes

        readonly IBus _bus;
        private readonly ILogger<BackgroundJob> _logger;

        public BackgroundJob(IBus bus, ILogger<BackgroundJob> logger)
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = false });            
            _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);            
            _bus = bus;
            _logger = logger;

            _logger.LogDebug("Next run scheduled at: '{0}'", _nextRun);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.UtcNow;                
                if (now > _nextRun)
                {
                    await ExecuteTaskAsync(stoppingToken);
                    _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);

                    _logger.LogDebug("Next run scheduled at: '{0}'", _nextRun);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private async Task ExecuteTaskAsync(CancellationToken stoppingToken)
        {
            // Put message on bus following the CRON schedule

            await _bus.Publish(new CommandDto { MessageContent = "SyncAllDataCommand", MessageDate = DateTime.UtcNow, MessageOrigin = "BackgroundJob" }, stoppingToken);
        }
    }
}
