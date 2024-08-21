using AsteroidsCore.ECS.Components;
using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Components {
  public class MovementComponent : Component {
    public float MaxSpeed { get; set; } = 0.2f;

    public bool Accelerating { get; set; } = false;

    public float Acceleration { get; set; } = 0;

    public bool AutoDecelerationEnabled { get; set; } = true;

    public bool ConstantMaxSpeed {  get; set; } = false;
  }
}
