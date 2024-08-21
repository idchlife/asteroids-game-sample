using AsteroidsCore.ECS.Entities;
using AsteroidsCore.World.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Objects {
  public class Laser : AsteroidsGameObject {
    public Laser(Entity entity, GameWorldEvents gameWorldEvents) : base(entity, gameWorldEvents) { }
  }
}
