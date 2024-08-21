using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Physics.Components;
using AsteroidsCore.Physics.Listeners;
using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;

namespace AsteroidsCore.Physics.Systems {
  public class PhysicsSystem : ECS.Systems.System, IHasCreateBehaviour, IHasUpdateBehaviour {

    private PhysicsSystemComponent? physicsComponent { get; set; }

    private TransformComponent? transformComponent { get; set; }

    private ColliderComponent? colliderComponent { get; set; }

    private Func<Vec2, Vec2, float, List<PhysicsSystem>>? raycaster {  get; set; }

    public void OnCreate() {
      physicsComponent = GetEntity().GetOrCreateComponent<PhysicsSystemComponent>();
      transformComponent = GetEntity().GetComponent<TransformComponent>();

      colliderComponent = GetEntity().GetComponentThatImplements<ColliderComponent>();
    }

    public void OnUpdate() {
      if (colliderComponent == null) return;

      // Updating collider position so it would be same in the physics
      // world
      colliderComponent!.Pos = transformComponent!.Pos;
    }

    public List<PhysicsSystem> Raycast(Vec2 origin, Vec2 direction, float length) {
      return raycaster!(origin, direction, length);
    }

    public void SetRaycaster(Func<Vec2, Vec2, float, List<PhysicsSystem>> raycaster) {
      this.raycaster = raycaster;
    }

    public List<ICollisionStartDetector> FindCollisionStartDetectors() =>
      GetEntity().FindSystemsWhichImplement<ICollisionStartDetector>();

    public new Entity GetEntity() => base.GetEntity();

    public ColliderComponent? GetColliderComponent() => colliderComponent;

    public bool IsActive() => physicsComponent!.Active;
  }
}
