using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Loggers {
  public sealed class ConsoleLogger : ILogger {
    public void LogError(string message) => Console.WriteLine($"[Error]: {message}");

    public void LogInfo(string message) => Console.WriteLine($"[Info]: {message}");
  }
}
