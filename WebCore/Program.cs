
namespace WebCore
{

  using Microsoft.AspNetCore;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Logging;

  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>()
              .UseUrls("https://localhost:4000")
              .ConfigureLogging(conf => conf.ClearProviders());
  }
}
