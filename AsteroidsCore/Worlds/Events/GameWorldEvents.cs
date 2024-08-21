using AsteroidsCore.ECS.Entities;
using AsteroidsCore.World.Events.Entities;
using AsteroidsCore.Worlds.Events.Entities;
using System;

namespace AsteroidsCore.World.Events {
  public class GameWorldEvents {
    private object lockObject { get; } = new object();

    public event EventHandler<EntityCreatedEvent>? EntityCreated;

    public event EventHandler<EntityDestroyedEvent>? EntityDestroyed;

    public void EmitEntityDestroyed(int entityId) {
      lock (lockObject) {
        EntityDestroyed?.Invoke(this, new EntityDestroyedEvent(entityId));
      }
    }

    public void EmitEntityCreated(Entity entity) {
      lock (lockObject) {
        EntityCreated?.Invoke(this, new EntityCreatedEvent(entity));
      }
    }
  }
}
