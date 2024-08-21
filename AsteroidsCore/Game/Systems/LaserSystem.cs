using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Physics.Systems;
using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Systems {
  public class LaserSystem : ECS.Systems.System, IHasCreateBehaviour, IHasUpdateBehaviour {
    private LaserComponent? laserComponent { get; set; }

    private PhysicsSystem? physicsSystem { get; set; }

    private TransformComponent? transformComponent { get; set; }

    public void OnCreate() {
      laserComponent = GetEntity().GetOrCreateComponent<LaserComponent>();

      var selfDestructComponent = GetEntity().AddComponent<SelfDestructComponent>();
      selfDestructComponent.DestroyAfterMs = 200;
      GetEntity().AddSystem<SelfDestructSystem>();
      
      transformComponent = GetEntity().GetComponent<TransformComponent>();
      physicsSystem = GetEntity().GetSystem<PhysicsSystem>();
    }

    public void OnUpdate() {
      if (laserComponent!.Activated) return;

      laserComponent.Activated = true;

      var collisions = physicsSystem!.Raycast(
        transformComponent!.Pos,
        Vec2.DirectionFromRadians(transformComponent.Rotation),
        10
      );

      if (collisions.Count > 0) {
        foreach (var collision in collisions) {
          var entity = collision.GetEntity();

          // We do not want to destroy ourselves lol
          if (entity.GetSystem<ShipSystem>() != null) continue;
          
          if (entity.Id == GetEntity().Id) continue;

          collision.GetEntity().Destroy();
        }
      }
    }
  }
}
