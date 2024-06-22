
using Domain;
using Domain.Entities;

namespace GemicleAppTest.Presentation;

public class TaskSchedulerService : IDisposable
{
    private Timer _timer;
    private readonly TimeSpan _dayMilliseconds = TimeSpan.FromMilliseconds(86400000); // 24 hours in milliseconds
    private readonly TimeOnly _scheduledTime;
    private readonly Campaign _campaign;
    private readonly MultiThreadFileWriterService _writerService;

    public TaskSchedulerService(Campaign campaign, MultiThreadFileWriterService writerService)
    {
        _scheduledTime = campaign.ScheduleTime;
        _campaign = campaign;
        _writerService = writerService;
    }

    public Timer StartAsync()
    {
        var now = DateTime.Now.TimeOfDay;
        TimeSpan delay;

        if (_scheduledTime.ToTimeSpan() > now)
        {
            delay = _scheduledTime.ToTimeSpan() - now;
        }
        else
        {
            delay = _dayMilliseconds - now + _scheduledTime.ToTimeSpan();
        }

        _timer = new Timer(Work, null, delay, _dayMilliseconds);
        return _timer;
    }

    private async void Work(object state)
    {
        FacadeService.WriteToFile(_campaign, _writerService);
        _timer?.Change(_dayMilliseconds, Timeout.InfiniteTimeSpan);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}