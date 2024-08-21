using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Utils;
using System;

namespace AsteroidsCore.Game.Systems {
  public class SpeedSystem : ECS.Systems.System, IHasCreateBehaviour, IHasUpdateBehaviour {
    private TransformComponent? transformComponent;
    private SpeedComponent? speedComponent;

    public void OnCreate() {
      transformComponent = GetEntity().GetComponent<TransformComponent>();
      speedComponent = GetEntity().GetOrCreateComponent<SpeedComponent>();
    }

    public void OnUpdate() {
      var diffMs = DateTime.Now.ToUnixTimeMs() - speedComponent!.LastUpdateMs;

      var seconds = (float)diffMs / 1000;

      var distance = speedComponent.LastPos.DistanceTo(transformComponent!.Pos);

      speedComponent.Speed = distance * (1 / seconds);

      speedComponent.LastUpdateMs = DateTime.Now.ToUnixTimeMs();
      speedComponent.LastPos = transformComponent!.Pos;
    }

    public float GetSpeed() => speedComponent!.Speed;
  }
}
