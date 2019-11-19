using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

      // --> AddHangfire
      services.AddHangfire(configuration =>
      {
        // Install-Package Hangfire.MemoryStorage
        //configuration.UseMemoryStorage();

        configuration.UseSqlServerStorage(connString, _serverStorageOptions);

        // https://docs.hangfire.io/en/latest/background-processing/dealing-with-exceptions.html
        configuration.UseFilter(new AutomaticRetryAttribute { Attempts = 4 });
      });

      // --> AddHangfireServer
      // The server can run in other machines.
      // https://docs.hangfire.io/en/latest/background-processing/placing-processing-into-another-process.html
      services.AddHangfireServer(options =>
      {
        // https://docs.hangfire.io/en/latest/background-processing/configuring-degree-of-parallelism.html
        //options.WorkerCount = 5;

        // https://docs.hangfire.io/en/latest/background-methods/calling-methods-with-delay.html
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

      app.UseAuthorization();

      app.Use(injectFakeUser);

      // http://localhost:5000/hangfire
      app.UseHangfireDashboard("/hangfire", new DashboardOptions
      {
        Authorization = new[] { new MyAuthorizationFilter() }
      });

      //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    private static Task injectFakeUser(HttpContext httpContext, Func<Task> next)
    {
      int userId = 1;

      IEnumerable<Claim> claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Name, $"User#{userId}")
      };

      var claimsIdentity = new ClaimsIdentity(claims, "FakeAuthType");

      httpContext.User = new ClaimsPrincipal(claimsIdentity);

      return next.Invoke();
    }
  }
}
