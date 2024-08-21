using AsteroidsCore.ECS.Components;
using AsteroidsCore.Utils.Geometry;

namespace AsteroidsCore.Game.Components {
  public class FollowerComponent : Component {
    public Vec2 FollowPos { get; set; } = Vec2.Zero;

    public float FollowSpeed { get; set; } = 0.1f;
  }
}
