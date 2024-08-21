using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Loggers {
  public interface IHasLogger {
    public void SetLogger(ILogger logger);

    public ILogger GetLogger();
  }
}
