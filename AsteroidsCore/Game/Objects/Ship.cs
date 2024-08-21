using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Game.Systems;
using AsteroidsCore.Utils.Geometry;
using AsteroidsCore.World.Events;

namespace AsteroidsCore.Game.Objects {
  public class Ship : AsteroidsGameObject {
    private ShipComponent? shipComponent { get; set; }

    private SpeedComponent? speedComponent { get; set; }

    private TransformComponent? transformComponent { get; set; }

    public Ship(Entity entity, GameWorldEvents gameWorldEvents) : base(entity, gameWorldEvents) {
      shipComponent = entity.GetComponent<ShipComponent>();
      speedComponent = entity.GetComponent<SpeedComponent>();
      transformComponent = entity.GetComponent<TransformComponent>();
    }

    public Vec2 GetCoordinates() => transformComponent!.Pos;

    public float GetSpeed() => speedComponent!.Speed;

    public double GetAngleRadians() => transformComponent!.Rotation;

    public int GetLaserCharges() => shipComponent!.LaserCharges;

    public float GetLaserChargingProgress() => shipComponent!.LaserChargingValue;
  }
}
