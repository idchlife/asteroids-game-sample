using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Worlds.Commands {
  public interface IHasCommands {
    public void SetCommands(Commands commands);

    public Commands GetCommands();
  }
}
