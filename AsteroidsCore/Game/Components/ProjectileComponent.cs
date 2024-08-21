using AsteroidsCore.ECS.Components;
using AsteroidsCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Components {
  public class ProjectileComponent : Component {
    public long CreatedAtMs { get; set; } = DateTime.Now.ToUnixTimeMs();
  }
}
