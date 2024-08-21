using AsteroidsCore.ECS.Components;
using AsteroidsCore.Utils.Geometry;

namespace AsteroidsCore.Game.Components {
  public class AsteroidShardComponent : Component {
    public Vec2 Direction { get; set; }

    public float Speed { get; set; }
  }
}
