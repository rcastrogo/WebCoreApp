using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Logging {

  public interface ILogger {
    void Write(string message);
  }

}
