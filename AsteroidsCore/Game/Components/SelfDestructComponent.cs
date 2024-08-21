using AsteroidsCore.ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Components {
  public class SelfDestructComponent : Component {
    public long DestroyAfterMs { get; set; }

    public long CreatedAtMs { get; set; }

    public bool Destroyed { get; set; } = false;
  }
}
