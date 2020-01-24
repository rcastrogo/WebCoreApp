using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Logging.Loggers {

  public class MemoryLogger : ILogger {

    int _maxSize;
    List<string> _buffer;

    public MemoryLogger(int maxSize) {
      _buffer = new List<string>();
      _maxSize = maxSize;
    }

    public void Write(string message) {
      _buffer.Add(message);
    }

    public IEnumerable<string> Lines{
      get{
        return _buffer.ToArray();
      }
    }

    public MemoryLogger Clear(){
      _buffer.Clear();
      return this;
    }

  }

}
