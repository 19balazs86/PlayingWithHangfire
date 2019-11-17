using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PlayingWithHangfire
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

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
        var serverStorageOptions = new SqlServerStorageOptions
        {
          SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
          QueuePollInterval          = TimeSpan.Zero,
          PrepareSchemaIfNecessary   = true
        };

        configuration.UseSqlServerStorage(connString, serverStorageOptions);
      });

      services.AddHangfireServer(options =>
      {
        options.WorkerCount             = 5;
        options.SchedulePollingInterval = TimeSpan.FromSeconds(10);
      });
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

      backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

      app.UseAuthorization();

      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
