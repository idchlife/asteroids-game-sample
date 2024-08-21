using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Configs {
  public class GameConfig {
    public float ShipMaxSpeed { get; set; } = 0.2f;

    public float ProjectileSpeed { get; set; } = 0.3f;

    public float FlyingSaucerSpeed { get; set; } = 0.1f;

    public float MapWidth { get; set; } = 16;

    public float MapHeight { get; set; } = 10;
  }
}
