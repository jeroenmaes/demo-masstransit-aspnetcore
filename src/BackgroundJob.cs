using MassTransit;
using NCrontab;

namespace DemoMassTransitAspnetcore
{
    public class BackgroundJob : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private string Schedule => "*/1 * * * *"; //Runs every 1 minutes

        readonly IBus _bus;

        public BackgroundJob(IBus bus)
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = false });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
                if (now > _nextRun)
                {
                    await ExecuteTaskAsync(stoppingToken);
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private async Task ExecuteTaskAsync(CancellationToken stoppingToken)
        {
            // Put message on bus following the CRON schedule

            await _bus.Publish(new EventDto { MessageText = "Hello!", MessageDate = DateTime.UtcNow, MessageOrigin = "BackgroundJob" }, stoppingToken);
        }
    }
}
