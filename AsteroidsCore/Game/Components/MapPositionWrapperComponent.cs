using AsteroidsCore.ECS.Components;
using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Components {
  public class MapPositionWrapperComponent : Component {
    public Vec2 Center { get; set; } = Vec2.Zero;

    public Vec2 Size { get; set; } = Vec2.Zero;

    public float MaxY { get; set; }
    public float MinY { get; set; }

    public float MaxX { get; set; }
    public float MinX { get; set; }
  }
}
