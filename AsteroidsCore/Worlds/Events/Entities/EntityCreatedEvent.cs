using AsteroidsCore.ECS.Entities;
using System;
using System.Text;

namespace AsteroidsCore.World.Events.Entities {
  public struct EntityCreatedEvent {
    public Entity Entity { get; }

    public EntityCreatedEvent(Entity entity) {
      Entity = entity;
    }
  }
}
