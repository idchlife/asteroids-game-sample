using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Utils.Geometry;

namespace AsteroidsCore.Game.Systems {
  public class MapPositionWrapperSystem : ECS.Systems.System, IHasCreateBehaviour, IHasUpdateBehaviour {
    private MapPositionWrapperComponent? wrapperComponent;

    private TransformComponent? transformComponent;

    public void OnCreate() {
      wrapperComponent = GetEntity().GetComponent<MapPositionWrapperComponent>();
      transformComponent = GetEntity().GetComponent<TransformComponent>();

      wrapperComponent!.MaxX = wrapperComponent.Center.X + (wrapperComponent.Size.X / 2);
      wrapperComponent!.MinX = wrapperComponent.Center.X - (wrapperComponent.Size.X / 2);

      wrapperComponent!.MaxY = wrapperComponent.Center.Y + (wrapperComponent.Size.Y / 2);
      wrapperComponent!.MinY = wrapperComponent.Center.Y - (wrapperComponent.Size.Y / 2);
    }

    public void OnUpdate() {
      float x = 0;
      float y = 0;

      bool xChanged = false;
      bool yChanged = false;

      if (transformComponent!.Pos.X > wrapperComponent!.MaxX) {
        x = wrapperComponent.MinX;
        xChanged = true;
      }

      if (transformComponent!.Pos.X < wrapperComponent!.MinX) {
        x = wrapperComponent.MaxX;
        xChanged = true;
      }

      if (transformComponent!.Pos.Y > wrapperComponent!.MaxY) {
        y = wrapperComponent.MinY;
        yChanged = true;
      }

      if (transformComponent!.Pos.Y < wrapperComponent!.MinY) {
        y = wrapperComponent.MaxY;
        yChanged = true;
      }

      if (yChanged || xChanged) {
        var newPos = new Vec2(
          xChanged ? x : transformComponent!.Pos.X,
          yChanged ? y : transformComponent!.Pos.Y
        );

        transformComponent!.Pos = newPos;
      }
    }
  }
}
