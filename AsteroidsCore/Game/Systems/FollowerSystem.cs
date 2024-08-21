using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Utils.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Systems {
  public class FollowerSystem : ECS.Systems.System, IHasCreateBehaviour, IHasUpdateBehaviour {
    private FollowerComponent? followerComponent {  get; set; }

    private MovementComponent? movementComponent { get; set; }

    private TransformComponent? transformComponent { get; set; }

    private MovementSystem? movementSystem { get; set; }

    public void OnCreate() {
      followerComponent = GetEntity().GetComponent<FollowerComponent>();
      movementComponent = GetEntity().GetComponent<MovementComponent>();
      transformComponent = GetEntity().GetComponent<TransformComponent>();

      movementSystem = GetEntity().GetSystem<MovementSystem>();
      movementSystem!.EnableConstantMaxSpeed();

      UpdateMovementSpeed();
    }

    public void OnUpdate() {
      if (transformComponent!.Pos.DistanceTo(followerComponent!.FollowPos) < 0.3) {
        movementSystem!.Active = false;
      } else {
        movementSystem!.Active = true;
      }

      movementSystem!.SetRotationFromDirection(
        Vec2.DirectionFromPointToPoint(transformComponent!.Pos, followerComponent!.FollowPos),
        lerp: true
      );
    }

    public void SetFollowPos(Vec2 pos) {
      followerComponent!.FollowPos = pos;
    }

    public void SetFollowSpeed(float speed) {
      followerComponent!.FollowSpeed = speed;

      UpdateMovementSpeed();
    }

    private void UpdateMovementSpeed() {
      movementSystem!.SetMaxSpeed(followerComponent!.FollowSpeed);
    }
  }
}
