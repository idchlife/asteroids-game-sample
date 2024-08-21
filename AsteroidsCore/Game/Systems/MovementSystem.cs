using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Utils.Geometry;
using AsteroidsCore.Utils.Math;
using System;

namespace AsteroidsCore.Game.Systems {
  public class MovementSystem : ECS.Systems.System, IHasCreateBehaviour, IHasUpdateBehaviour {
    private MovementComponent? movementComponent { get; set; }

    private TransformComponent? transformComponent { get; set; }

    private float currentSpeed { get; set; } = 0;

    public void OnCreate() {
      movementComponent = GetEntity().GetOrCreateComponent<MovementComponent>();
      transformComponent = GetEntity().GetOrCreateComponent<TransformComponent>();
    }

    public void OnUpdate() {
      if (movementComponent!.ConstantMaxSpeed) {
        transformComponent!.Pos += (GetDirection() * movementComponent.MaxSpeed);
      }

      if (!movementComponent!.Accelerating) {
        // Decelerating over time
        if (movementComponent.AutoDecelerationEnabled) {
          movementComponent.Acceleration = MathUtils.Lerp(
            movementComponent.Acceleration,
            0,
            0.006f
          );
        }
      } else {
        movementComponent.Acceleration = MathUtils.Lerp(
          movementComponent.Acceleration,
          movementComponent.MaxSpeed,
          0.01f
        );
      }
      
      // Actually moving with the help of acceleration
      transformComponent!.Pos = Vec2.Lerp(
        transformComponent.Pos,
        transformComponent.Pos + (GetDirection() * movementComponent.Acceleration),
        0.04f
      );
    }

    private Vec2 GetDirection() => Vec2.DirectionFromRadians(transformComponent!.Rotation);

    public void SetMaxSpeed(float maxSpeed) => movementComponent!.MaxSpeed = maxSpeed;

    public void EnableConstantMaxSpeed() => movementComponent!.ConstantMaxSpeed = true;

    public void EnableAcceleration() => movementComponent!.Accelerating = true;

    public void DisableAcceleration() => movementComponent!.Accelerating = false;

    public void EnableDeceleration() => movementComponent!.AutoDecelerationEnabled = true;

    public void DisableDeceleration() => movementComponent!.AutoDecelerationEnabled = false;


    public void SetRotationFromDirection(Vec2 direction, bool lerp = false) {
      var rotation = direction.DirectionToRadians();

      if (lerp) {
        InstantRotate(
          MathUtils.Lerp(transformComponent!.Rotation, rotation, 0.5f)
        );
      } else {
        InstantRotate(rotation);
      }
    }

    public void RotateLeft() {
      Rotate(transformComponent!.Rotation + 0.2);
    }

    public void RotateRight() {
      Rotate(transformComponent!.Rotation - 0.2);
    }

    public void InstantRotate(double radians) => transformComponent!.Rotation = radians;

    private void Rotate(double destinationRadians) {
      transformComponent!.Rotation = MathUtils.Lerp(
        transformComponent.Rotation,
        destinationRadians,
        0.02f
      );
    }
  }
}
