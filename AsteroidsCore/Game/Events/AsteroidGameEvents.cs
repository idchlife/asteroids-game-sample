using AsteroidsCore.Game.Events.Game;
using AsteroidsCore.Game.Events.Objects;
using AsteroidsCore.Game.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Events {
  public class AsteroidGameEvents {
    private object lockObject { get; } = new object();

    private object lockObjectCreated { get; } = new object();

    public event EventHandler<GameObjectCreatedEvent>? GameObjectCreated;

    public event EventHandler? GameStarted;

    public event EventHandler? GameOver;

    public event EventHandler<ScoreChangedEvent>? ScoreChanged;

    public void EmitGameStarted() {
      lock (lockObject) {
        GameStarted?.Invoke(this, null);
      }
    }

    public void EmitGameOver() {
      lock (lockObject) {
        GameOver?.Invoke(this, null);
      }
    }

    public void EmitGameObjectCreated(AsteroidsGameObject obj) {
      lock (lockObjectCreated) {
        GameObjectCreated?.Invoke(this, new GameObjectCreatedEvent(obj));
      }
    }

    public void EmitScoreChanged(int score) {
      lock (lockObject) {
        ScoreChanged?.Invoke(this, new ScoreChangedEvent(score));
      }
    }
  }
}
