using AsteroidsCore.Game.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Events.Objects {
  public class GameObjectDestroyedEvent {
    public AsteroidsGameObject GameObject {  get; private set; }

    public GameObjectDestroyedEvent(AsteroidsGameObject gameObject) {
      GameObject = gameObject;
    }
  }
}
