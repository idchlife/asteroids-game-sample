using AsteroidsCore.Game.Objects;

namespace AsteroidsCore.Game.Events.Objects {
  public class ShipCreatedEvent {
    public Ship Ship { get; }

    public ShipCreatedEvent(Ship ship) {
      Ship = ship;
    }
  }
}
