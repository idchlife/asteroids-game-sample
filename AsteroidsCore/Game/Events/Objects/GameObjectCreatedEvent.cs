using AsteroidsCore.Game.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Events.Objects {
  public class GameObjectCreatedEvent {
    public AsteroidsGameObject GameObject { get; }
    public GameObjectCreatedEvent(AsteroidsGameObject gameObject) {
      GameObject = gameObject;
    }
  }
}
