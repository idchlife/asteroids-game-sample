using AsteroidsCore.ECS.Entities;
using AsteroidsCore.World.Events.Entities;
using AsteroidsCore.Worlds.Events.Entities;
using System;

namespace AsteroidsCore.World.Events {
  public class GameWorldEvents {
    public event EventHandler<EntityCreatedEvent>? EntityCreated;

    public event EventHandler<EntityDestroyedEvent>? EntityDestroyed;

    public void EmitEntityDestroyed(int entityId) {
      EntityDestroyed?.Invoke(this, new EntityDestroyedEvent(entityId));
    }

    public void EmitEntityCreated(Entity entity) {
      EntityCreated?.Invoke(this, new EntityCreatedEvent(entity));
    }
  }
}
