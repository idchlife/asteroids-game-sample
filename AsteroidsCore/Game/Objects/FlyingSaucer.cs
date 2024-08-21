using AsteroidsCore.ECS.Entities;
using AsteroidsCore.World.Events;

namespace AsteroidsCore.Game.Objects {
  public class FlyingSaucer : AsteroidsGameObject {
    public FlyingSaucer(Entity entity, GameWorldEvents gameWorldEvents) : base(entity, gameWorldEvents) { }
  }
}
