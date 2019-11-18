using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlayingWithHangfire.Jobs;

namespace PlayingWithHangfire
{
  public class Startup
  {
    #region Fields
    private readonly IConfiguration _configuration;

    private readonly SqlServerStorageOptions _serverStorageOptions = new SqlServerStorageOptions
    {
      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
      QueuePollInterval          = TimeSpan.Zero,
      PrepareSchemaIfNecessary   = true
    };
    #endregion

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      string connString = _configuration.GetConnectionString("HangfireConnection");

      services.AddHangfire(configuration =>
      {
        configuration.UseSqlServerStorage(connString, _serverStorageOptions);

        // https://docs.hangfire.io/en/latest/background-processing/dealing-with-exceptions.html
        configuration.UseFilter(new AutomaticRetryAttribute { Attempts = 4 });
      });

      services.AddHangfireServer(options =>
      {
        //options.WorkerCount             = 5;
        options.SchedulePollingInterval = TimeSpan.FromSeconds(10);
      });

      services.AddSingleton<SingletonDependency>();
      services.AddScoped<IRandomNumberJob, RandomNumberJob>();
      services.AddSingleton<IScheduledJob, ScheduledJob>();
    }

    public void Configure(
      IApplicationBuilder app,
      IWebHostEnvironment env,
      IBackgroundJobClient backgroundJobs)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseRouting();

      // http://localhost:5000/hangfire
      app.UseHangfireDashboard();

      //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

      app.UseAuthorization();

      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
