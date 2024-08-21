using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Game.Events.Objects;
using AsteroidsCore.Utils.Geometry;
using AsteroidsCore.World.Events;
using System;

namespace AsteroidsCore.Game.Objects {
  public class AsteroidsGameObject {
    // Listen to this to remove from renderer
    public event EventHandler<GameObjectDestroyedEvent> Destroyed;

    private Entity entity { get; }

    private GameWorldEvents gameWorldEvents { get; }

    private TransformComponent transformComponent { get; }

    public AsteroidsGameObject(Entity entity, GameWorldEvents gameWorldEvents) {
      this.entity = entity;
      this.transformComponent = entity.GetOrCreateComponent<TransformComponent>()!;
      this.gameWorldEvents = gameWorldEvents;
      
      // When entity in game world is destoryed we need to inform about this
      // our end renderer, so it would dispose of the visual representation :)
      this.gameWorldEvents.EntityDestroyed += (obj, args) => {
        if (args.EntityId == entity.Id) Destroyed?.Invoke(this, new GameObjectDestroyedEvent(this));
      };
    }

    public Vec2 Position => transformComponent.Pos;

    public double Rotation => transformComponent.Rotation;
  }
}
