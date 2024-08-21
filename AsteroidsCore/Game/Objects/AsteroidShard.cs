using AsteroidsCore.ECS.Entities;
using AsteroidsCore.World.Events;

namespace AsteroidsCore.Game.Objects {
  public class AsteroidShard : AsteroidsGameObject {
    public AsteroidShard(Entity entity, GameWorldEvents gameWorldEvents) : base(entity, gameWorldEvents) { }
  }
}
