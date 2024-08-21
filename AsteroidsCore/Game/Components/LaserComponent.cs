using AsteroidsCore.ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Components {
  public class LaserComponent : Component {
    public bool Activated { get; set; } = false;
  }
}
