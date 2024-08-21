using AsteroidsCore.ECS.Components;
using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Physics.Components {
  public class ColliderComponent : Component {
    public Vec2 Pos { get; set; }
  }
}
