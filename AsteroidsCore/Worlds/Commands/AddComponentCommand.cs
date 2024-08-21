using AsteroidsCore.ECS.Components;

namespace AsteroidsCore.Worlds.Commands
{
    public class AddComponentCommand : Command {
    public int EntityId { get; }
    
    public Component Component { get; }

    public AddComponentCommand(int entityId, Component component) {
      EntityId = entityId;
      Component = component;
    }
  }
}
