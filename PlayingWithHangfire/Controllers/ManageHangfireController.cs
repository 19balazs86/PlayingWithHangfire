using System;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using PlayingWithHangfire.Jobs;

namespace PlayingWithHangfire.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ManageHangfireController : ControllerBase
  {
    private readonly IBackgroundJobClient _backgroundJobs;

    public ManageHangfireController(IBackgroundJobClient backgroundJobs)
    {
      _backgroundJobs = backgroundJobs;
    }

    [HttpGet(nameof(EnqueueConsole))]
    public string EnqueueConsole([FromQuery] int randomInt)
    {
      return _backgroundJobs.Enqueue(() => Console.WriteLine("Hello EnqueueConsole with number: {0}.", randomInt));
    }

    [HttpGet(nameof(EnqueueRandomNumberJob))]
    public void EnqueueRandomNumberJob([FromQuery] int randomInt)
    {
      _backgroundJobs.Enqueue<IRandomNumberJob>(job => job.PrintNumber(randomInt));
    }

    [HttpGet(nameof(ScheduleEnqueueAt))]
    public void ScheduleEnqueueAt([FromQuery] int randomInt)
    {
      var input     = new ScheduledJobInput { Number = randomInt, ScheduleAt = DateTimeOffset.UtcNow };
      var enqueueAt = input.ScheduleAt.AddSeconds(5);

      _backgroundJobs.Schedule<IScheduledJob>(job => job.DoWork(input), enqueueAt);
    }

    [HttpGet(nameof(ScheduleDelay))]
    public void ScheduleDelay([FromQuery] int randomInt)
    {
      var input = new ScheduledJobInput { Number = randomInt, ScheduleAt = DateTimeOffset.UtcNow };
      var delay = TimeSpan.FromSeconds(5);

      _backgroundJobs.Schedule<IScheduledJob>(job => job.DoWork(input), delay);
    }
  }
}
