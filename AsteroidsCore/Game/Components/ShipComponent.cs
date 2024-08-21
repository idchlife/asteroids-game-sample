using AsteroidsCore.ECS.Components;

namespace AsteroidsCore.Game.Components {
  public class ShipComponent : Component {
    public int Health { get; set; } = 100;

    public int LaserCharges { get; set; } = 3;

    public float LaserChargingValue { get; set; } = 0;

    public long LastChargingCheckAtMs { get; set; } = -1;

    public bool Accelerating { get; set; } = false;

    public bool RotatingLeft { get; set; } = false;

    public bool RotatingRight { get; set; } = false;


    public int MaxLaserCharges { get; } = 3;

    public int LaserChargingMs { get; } = 2000;

    public int ProjectileTimeoutMs { get; } = 500;

    public long LastProjectileAt { get; set; } = -1;
  }
}
