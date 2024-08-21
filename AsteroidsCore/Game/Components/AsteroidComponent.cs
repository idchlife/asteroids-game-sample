using AsteroidsCore.ECS.Components;
using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Components {
  public class AsteroidComponent : Component {
    public Vec2 NextWanderingTargetPos {  get; set; }

    public long NextWanderingTargetTimeoutMs { get; set; }

    public long NextWanderingTargetSetAt { get; set; }
  }
}
