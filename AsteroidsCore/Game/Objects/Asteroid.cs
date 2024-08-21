using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Utils.Geometry;
using AsteroidsCore.World.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Objects {
  public class Asteroid : AsteroidsGameObject {
    public Asteroid(Entity entity, GameWorldEvents gameWorldEvents) : base(entity, gameWorldEvents) { }

    public Vec2 Size { get; set; }
  }
}
