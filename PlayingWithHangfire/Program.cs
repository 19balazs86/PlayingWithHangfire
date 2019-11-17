using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PlayingWithHangfire
{
  public class Program
  {
    public static void Main(string[] args)
    {
      createHostBuilder(args).Build().Run();
    }

    private static IHostBuilder createHostBuilder(string[] args)
    {
      return Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
          webBuilder
            .UseStartup<Startup>());
    }
  }
}
