using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Logging {

  public class LogManager : ILogger {

    readonly List<ILogger> _loggers;
    
    public LogManager(IEnumerable<ILogger> loggers){
      _loggers = new List<ILogger>();
      foreach(ILogger logger in loggers){
        _loggers.Add(logger);
      }    
    }

    public void Write(string message) {
      string __message = $"{DateTime.Now.TimeOfDay.ToString()} {message}\n";
      foreach(ILogger logger in _loggers){
        logger.Write(__message);
      }
    }
      
    public IEnumerable<string> GetLines() {
      Loggers.MemoryLogger __logger = (Loggers.MemoryLogger) _loggers.SingleOrDefault(log => log.GetType() == typeof(Loggers.MemoryLogger));
      if (__logger != null)
        return __logger.Lines;
      else
        return new string[] { };
    }

    
  }

}
