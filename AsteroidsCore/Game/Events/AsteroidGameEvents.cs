using AsteroidsCore.Game.Events.Game;
using AsteroidsCore.Game.Events.Objects;
using AsteroidsCore.Game.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Events {
  public class AsteroidGameEvents {
    public event EventHandler<GameObjectCreatedEvent>? GameObjectCreated;

    public event EventHandler? GameStarted;

    public event EventHandler? GameOver;

    public event EventHandler<ScoreChangedEvent>? ScoreChanged;

    public void EmitGameStarted() => GameStarted?.Invoke(this, null);

    public void EmitGameOver() => GameOver?.Invoke(this, null);

    public void EmitGameObjectCreated(AsteroidsGameObject obj) =>
      GameObjectCreated?.Invoke(this, new GameObjectCreatedEvent(obj));

    public void EmitScoreChanged(int score) => ScoreChanged?.Invoke(this, new ScoreChangedEvent(score));
  }
}
