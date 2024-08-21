using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Loggers {
  public interface ILogger {
    public void LogInfo(string message);
    public void LogError(string message);
  }
}
