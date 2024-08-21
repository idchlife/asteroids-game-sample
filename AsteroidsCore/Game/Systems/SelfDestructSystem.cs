using AsteroidsCore.Behaviours;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Systems {
  public class SelfDestructSystem : ECS.Systems.System, IHasCreateBehaviour, IHasUpdateBehaviour {
    private SelfDestructComponent? selfDestructComponent;

    public void OnCreate() {
      selfDestructComponent = GetEntity().GetComponent<SelfDestructComponent>();

      selfDestructComponent!.CreatedAtMs = DateTime.Now.ToUnixTimeMs();
    }

    public void OnUpdate() {
      if (!selfDestructComponent!.Destroyed) {
        if ((DateTime.Now.ToUnixTimeMs() - selfDestructComponent!.CreatedAtMs) >= selfDestructComponent!.DestroyAfterMs) {
          selfDestructComponent!.Destroyed = true;

          GetEntity().Destroy();
        }
      }
    }
  }
}
