using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Logging.Loggers {
     
  public class FileLogger : ILogger {

    readonly string _location;

    public FileLogger(string location) {
      _location = location;
    }

    public void Write(string message) {
      System.IO.File.AppendAllText(System.IO.Path.Combine(_location, $"Logs/{System.DateTime.Now.ToString("yyyyMMdd")}.log.txt"), message); 
    }

  }

}
