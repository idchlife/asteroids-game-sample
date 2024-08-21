using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Physics.Components;
using AsteroidsCore.Physics.Listeners;
using AsteroidsCore.Physics.Systems;
using AsteroidsCore.Utils;
using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Systems {
  public class AsteroidSystem : ECS.Systems.System,
    IHasCreateBehaviour,
    IHasUpdateBehaviour,
    ICollisionStartDetector {
    private FollowerSystem? followerSystem {  get; set; }

    private AsteroidComponent? asteroidComponent { get; set; }

    private TransformComponent? transformComponent { get; set; }

    private Random? random { get; set; }

    public void OnCreate() {
      followerSystem = GetEntity().GetSystem<FollowerSystem>();
      asteroidComponent = GetEntity().GetOrCreateComponent<AsteroidComponent>();
      transformComponent = GetEntity().GetComponent<TransformComponent>();

      followerSystem!.SetFollowSpeed(0.0002f);

      random = new Random();
    }

    public void OnUpdate() {
      if ((DateTime.Now.ToUnixTimeMs() - asteroidComponent!.NextWanderingTargetSetAt) > asteroidComponent!.NextWanderingTargetTimeoutMs) {
        SetNextWanderingTarget();
      }
    }

    private void SetNextWanderingTarget() {
      asteroidComponent!.NextWanderingTargetPos = CreateRandomPositionAround(transformComponent!.Pos);
      asteroidComponent.NextWanderingTargetTimeoutMs = CreateRandomTimeoutMs();
      asteroidComponent!.NextWanderingTargetSetAt = DateTime.Now.ToUnixTimeMs();

      followerSystem!.SetFollowPos(asteroidComponent!.NextWanderingTargetPos);
    }

    private long CreateRandomTimeoutMs() => random!.Next(1000, 5000);
    private Vec2 CreateRandomPositionAround(Vec2 center) =>
      new(
        random!.Next((int)center.X - 3, (int)center.X + 3),

        random.Next((int)center.Y - 3, (int)center.Y + 3)
      );

    private void SpawnShards() {
      var numberOfShards = random!.Next(3, 6);

      for (int i = 0; i < numberOfShards; i++) {
        var direction = CreateRandomPositionAround(transformComponent!.Pos).Normalized();

        CreateShard(direction);
      }
    }

    private void CreateShard(Vec2 targetDirection) {
      var entity = new Entity();

      var colliderComponent = entity.AddComponent<CircleColliderComponent>();
      colliderComponent.Radius = 0.1f;
      entity.AddSystem<PhysicsSystem>();

      entity.AddSystem<MovementSystem>();

      var t = entity.GetComponent<TransformComponent>();
      t!.Pos = transformComponent!.Pos;

      var shardComponent = entity.AddComponent<AsteroidShardComponent>();
      shardComponent.Direction = targetDirection;
      shardComponent.Speed = ((float)random!.Next(1, 10)) / 1000;

      entity.AddSystem<AsteroidShardSystem>();

      GetEntity().AddEntityToGameWorld(entity);
    }

    public void OnCollisionStart(PhysicsSystem physicsSystem) {
      if (physicsSystem.GetEntity().GetSystem<ProjectileSystem>() != null) {
        SpawnShards();

        GetEntity().Destroy();
      }
    }
  }
}
