using System.Threading;

namespace AsteroidsCore.Worlds.Ids {
  public class IdGenerator : IIdGenerator {
    private int ids = 0;

    public int GetNextId() {
      return Interlocked.Increment(ref ids);
    }
  }
}
