using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Worlds.Ids {
  public interface IIdGenerator {
    public int GetNextId();
  }
}
