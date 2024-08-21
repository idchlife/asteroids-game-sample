using AsteroidsCore.ECS.Components;

namespace AsteroidsCore.Physics.Components {
  public class PhysicsSystemComponent : Component {
    public byte CollisionGroups { get; set; }

    public bool Active { get; set; } = true;
  }
}
