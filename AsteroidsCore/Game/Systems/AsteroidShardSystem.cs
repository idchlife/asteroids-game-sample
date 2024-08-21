using AsteroidsCore.Behaviours;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Physics.Listeners;
using AsteroidsCore.Physics.Systems;

namespace AsteroidsCore.Game.Systems {
  public class AsteroidShardSystem : ECS.Systems.System,
    IHasCreateBehaviour,
    ICollisionStartDetector {
    public void OnCreate() {
      var movementSystem = GetEntity().GetSystem<MovementSystem>();
      var asteroidShardComponent = GetEntity().GetComponent<AsteroidShardComponent>();

      movementSystem!.EnableConstantMaxSpeed();
      movementSystem.SetMaxSpeed(asteroidShardComponent!.Speed);
      movementSystem.SetRotationFromDirection(asteroidShardComponent.Direction);

      var selfDestructComponent = GetEntity().AddComponent<SelfDestructComponent>();

      selfDestructComponent.DestroyAfterMs = 5000;

      GetEntity().AddSystem<SelfDestructSystem>();
    }

    public void OnCollisionStart(PhysicsSystem physicsSystem) {
      if (physicsSystem.GetEntity().GetSystem<ProjectileSystem>() != null) {

        GetEntity().Destroy();
      }
    }
  }
}
