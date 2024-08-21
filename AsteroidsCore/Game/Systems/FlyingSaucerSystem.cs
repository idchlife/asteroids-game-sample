using AsteroidsCore.Behaviours;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Physics.Listeners;
using AsteroidsCore.Physics.Systems;

namespace AsteroidsCore.Game.Systems {
  public class FlyingSaucerSystem : ECS.Systems.System,
    IHasCreateBehaviour,
    IHasUpdateBehaviour,
    IHasDestroyBehaviour,
    ICollisionStartDetector {
    private FlyingSaucerComponent? flyingSaucerComponent;

    private FollowerComponent? followerComponent;

    public void OnCreate() {
      flyingSaucerComponent = GetEntity().GetOrCreateComponent<FlyingSaucerComponent>();
      followerComponent = GetEntity().GetComponent<FollowerComponent>();

      var followerSystem = GetEntity().GetSystem<FollowerSystem>();

      followerSystem!.SetFollowSpeed(0.002f);
    }

    public void OnDestroy() {
      // Removing link to component
      flyingSaucerComponent!.TargetTransformComponent = null;
    }

    public void OnUpdate() {
      followerComponent!.FollowPos = flyingSaucerComponent!.TargetTransformComponent!.Pos;
    }

    public void OnCollisionStart(PhysicsSystem physicsSystem) {
      if (physicsSystem.GetEntity().GetSystem<ProjectileSystem>() != null) {
        GetEntity().Destroy();
      }
    }
  }
}
