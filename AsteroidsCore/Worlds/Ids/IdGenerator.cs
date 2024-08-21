using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Worlds.Ids {
  public class IdGenerator : IIdGenerator {
    private int ids = 0;

    public int GetNextId() => ++ids;
  }
}
