using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Worlds.Events.Entities {
  public class EntityDestroyedEvent {
    public int EntityId { get; }

    public EntityDestroyedEvent(
      int entityId
    ) {
      EntityId = entityId;
    }
  }
}
