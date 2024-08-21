using AsteroidsCore.ECS.Components;
using AsteroidsCore.ECS.Components.Transform;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Components {
  public class FlyingSaucerComponent : Component {
    public TransformComponent? TargetTransformComponent { get; set; }
  }
}
