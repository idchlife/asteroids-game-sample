using AsteroidsCore.ECS.Components;
using AsteroidsCore.Utils;
using AsteroidsCore.Utils.Geometry;
using System;

namespace AsteroidsCore.Game.Components {
  public class SpeedComponent : Component {
    public long LastUpdateMs { get; set; } = DateTime.Now.ToUnixTimeMs();

    public Vec2 LastPos { get; set; } = Vec2.Zero;
    
    // Distance/Second
    public float Speed { get; set; } = 0;
  }
}
