using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Physics.Listeners;
using AsteroidsCore.Physics.Systems;
using AsteroidsCore.Utils;
using System;

namespace AsteroidsCore.Game.Systems {
  public class ProjectileSystem : ECS.Systems.System,
    IHasCreateBehaviour,
    ICollisionStartDetector {
    private MovementSystem? movementSystem { get; set; }

    private ProjectileComponent? projectileComponent { get; set; }

    public void OnCollisionStart(PhysicsSystem physicsSystem) {
      var entity = physicsSystem.GetEntity();

      if (entity.GetSystem<ShipSystem>() != null || entity.GetSystem<ProjectileSystem>() != null) {
        return;
      }

      GetEntity().Destroy();
    }

    public void OnCreate() {
      movementSystem = GetEntity().GetSystem<MovementSystem>();

      movementSystem!.SetMaxSpeed(0.012f);
      movementSystem!.EnableConstantMaxSpeed();

      projectileComponent = GetEntity().GetOrCreateComponent<ProjectileComponent>();

      var selfDestructComponent = GetEntity().AddComponent<SelfDestructComponent>();
      selfDestructComponent.DestroyAfterMs = 1000;
      GetEntity().AddSystem<SelfDestructSystem>();
    }
  }
}
