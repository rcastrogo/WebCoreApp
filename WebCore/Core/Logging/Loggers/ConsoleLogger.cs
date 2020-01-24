using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Logging.Loggers {

   public class ConsoleLogger : ILogger {

    public void Write(string message) {
      Console.Write(message);
    }

  }

}
