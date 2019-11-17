using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PlayingWithHangfire.Jobs
{
  public interface IRandomNumberJob
  {
    Task PrintNumber(int number);
  }

  public class RandomNumberJob : IRandomNumberJob
  {
    private readonly Guid _id = Guid.NewGuid(); // The class is Scoped, so this has to be different for each call.

    private readonly ILogger<RandomNumberJob> _logger;
    private readonly SingletonDependency _singletonDependency;

    public RandomNumberJob(ILogger<RandomNumberJob> logger, SingletonDependency singletonDependency)
    {
      _logger = logger;
      _singletonDependency = singletonDependency;
    }

    public Task PrintNumber(int number)
    {
      _logger.LogInformation("The given number is: {number} | Instance id: {id} Dependency id {depId}",
        number, _id, _singletonDependency.Id);

      return Task.CompletedTask;
    }
  }
}
