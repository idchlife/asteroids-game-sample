using AsteroidsCore.Worlds.Commands;

namespace AsteroidsCore.World.Tasks {
  public class DestroyEntityCommand : Command {
    public int EntityId { get; }
    public DestroyEntityCommand(int entityId) => EntityId = entityId;
  }
}
