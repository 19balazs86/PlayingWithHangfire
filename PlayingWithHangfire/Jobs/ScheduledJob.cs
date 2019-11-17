using System;
using Microsoft.Extensions.Logging;

namespace PlayingWithHangfire.Jobs
{
  public class ScheduledJobInput
  {
    public DateTimeOffset ScheduleAt { get; set; }
    public int Number { get; set; }
  }

  public interface IScheduledJob
  {
    void DoWork(ScheduledJobInput input);
  }

  public class ScheduledJob : IScheduledJob
  {
    private readonly ILogger<ScheduledJob> _logger;

    public ScheduledJob(ILogger<ScheduledJob> logger) => _logger = logger;

    public void DoWork(ScheduledJobInput input)
    {
      _logger.LogInformation("ScheduledJob({number}): {diff}", input.Number, DateTimeOffset.UtcNow - input.ScheduleAt);
    }
  }
}
