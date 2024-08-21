using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.ECS.Components.Transform {
  public class TransformComponent : Component {
    public Vec2 Pos { get; set; } = Vec2.Zero;

    /// <summary>
    /// Radians
    /// </summary>
    public double Rotation { get; set; } = 0;
  }
}
