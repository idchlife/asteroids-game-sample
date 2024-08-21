using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Events.Game {
  public class ScoreChangedEvent {
    public int Score { get; }

    public ScoreChangedEvent(int score) {
      Score = score;
    }
  }
}
