using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Loggers;
using AsteroidsCore.Physics.Components;
using AsteroidsCore.Physics.Listeners;
using AsteroidsCore.Physics.Systems;
using AsteroidsCore.Utils;
using System;

namespace AsteroidsCore.Game.Systems {
  public class ShipSystem : ECS.Systems.System, IHasCreateBehaviour, IHasUpdateBehaviour, ICollisionStartDetector {
    private ShipComponent? shipComponent;

    private TransformComponent? transformComponent;

    private MovementSystem? movementSystem;

    public void OnCreate() {
      shipComponent = GetEntity().GetOrCreateComponent<ShipComponent>();
      transformComponent = GetEntity().GetOrCreateComponent<TransformComponent>();
      movementSystem = GetEntity().GetSystem<MovementSystem>();
    }

    public void OnUpdate() {
      if (shipComponent!.RotatingLeft) movementSystem!.RotateLeft();
      if (shipComponent!.RotatingRight) movementSystem!.RotateRight();

      LaserChargingUpdate();
    }

    public void ShootProjectile() {
      if (
        (DateTime.Now.ToUnixTimeMs() - shipComponent!.LastProjectileAt) < shipComponent!.ProjectileTimeoutMs
      ) return;

      var entity = new Entity();

      var colliderComponent = entity.AddComponent<CircleColliderComponent>();
      colliderComponent.Radius = 0.2f;

      entity.AddSystem<PhysicsSystem>();

      var t = entity.GetOrCreateComponent<TransformComponent>();
      t.Rotation = transformComponent!.Rotation;
      t.Pos = transformComponent!.Pos;

      entity.AddSystem<MovementSystem>();

      entity.AddSystem<ProjectileSystem>();

      GetEntity().AddEntityToGameWorld(entity);

      shipComponent!.LastProjectileAt = DateTime.Now.ToUnixTimeMs();
    }

    public void ShootLaser() {
      if (shipComponent!.LaserCharges == 0) return;

      shipComponent!.LaserCharges = (int)MathF.Max(
        0,
        shipComponent!.LaserCharges - 1
      );
      
      if (shipComponent!.LastChargingCheckAtMs == -1) {
        shipComponent!.LastChargingCheckAtMs = DateTime.Now.ToUnixTimeMs();
      }

      CreateLaser();
    }

    private void LaserChargingUpdate() {
      if (shipComponent!.LaserCharges == shipComponent!.MaxLaserCharges) {
        shipComponent!.LaserChargingValue = 0;
        return;
      }

      var diffMs = DateTime.Now.ToUnixTimeMs() - shipComponent!.LastChargingCheckAtMs;

      var chargingTimeMs = shipComponent!.LaserChargingMs;

      var howMuchCharge = diffMs / (float) chargingTimeMs;

      shipComponent!.LaserChargingValue = MathF.Min(
        shipComponent!.LaserChargingValue + howMuchCharge,
        1
      );

      if (shipComponent!.LaserChargingValue >= 1) {
        shipComponent!.LaserCharges = (int)MathF.Min(
          shipComponent!.LaserCharges + 1,
          shipComponent!.MaxLaserCharges
        );

        shipComponent!.LaserChargingValue = 0;
      }

      shipComponent!.LastChargingCheckAtMs = DateTime.Now.ToUnixTimeMs();
    }

    private void CreateLaser() {
      var entity = new Entity();


      var t = entity.GetComponent<TransformComponent>();
      t!.Pos = transformComponent!.Pos;
      t.Rotation = transformComponent!.Rotation;

      entity.AddSystem<PhysicsSystem>();

      entity.AddSystem<LaserSystem>();

      GetEntity().AddEntityToGameWorld(entity);
    }

    public void EnableAcceleration() => movementSystem!.EnableAcceleration();

    public void DisableAcceleration() => movementSystem!.DisableAcceleration();

    public void EnableRotatingLeft() => shipComponent!.RotatingLeft = true;

    public void DisableRotatingLeft() => shipComponent!.RotatingLeft = false;

    public void EnableRotatingRight() => shipComponent!.RotatingRight = true;

    public void DisableRotatingRight() => shipComponent!.RotatingRight = false;

    public void OnCollisionStart(PhysicsSystem physicsSystem) {
      var entity = physicsSystem.GetEntity();

      if (
        entity.GetSystem<FlyingSaucerSystem>() != null
        ||
        entity.GetSystem<AsteroidSystem>() != null
        ||
        entity.GetSystem<AsteroidShardSystem>() != null
      ) {
        GetEntity().Destroy();
      }
    }
  }
}
