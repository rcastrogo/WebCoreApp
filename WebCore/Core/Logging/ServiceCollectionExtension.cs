using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Core.Logging {

  public static class ServiceCollectionExtension {

    public static void AddLoggers(this IServiceCollection services,
                                  IConfiguration config) {
      string _basePath = System.IO.Directory.GetCurrentDirectory();
      int __bufferSize = config.GetValue<int>("Trace:MemoryLogger:BufferSize", 2000);
      ILogger[] _loggers = { new Loggers.ConsoleLogger(),
                             new Loggers.FileLogger(_basePath),
                             new Loggers.MemoryLogger(__bufferSize)};
      services.AddSingleton(typeof(Core.Logging.LogManager), 
                            new Core.Logging.LogManager(_loggers));
    }

  }

}
