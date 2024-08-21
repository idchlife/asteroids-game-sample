using System;
using AsteroidsCore.Game;
using AsteroidsCore.Game.Configs;
using AsteroidsCore.Game.Events.Game;
using AsteroidsCore.Game.Events.Objects;
using AsteroidsCore.Game.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidsCoreGame : MonoBehaviour {
  public GameObject ShipRendererPrefab;
  public GameObject ProjectileRendererPrefab;
  public GameObject LaserRendererPrefab;
  public GameObject AsteroidRendererPrefab;
  public GameObject AsteroidShardRendererPrefab;
  public GameObject FlyingSaucerRendererPrefab;

  public TextMeshProUGUI CoordinatesLabel;

  public TextMeshProUGUI SpeedLabel;

  public TextMeshProUGUI AngleLabel;

  public TextMeshProUGUI LaserChargesLabel;

  public TextMeshProUGUI LaserChargingProgressLabel;

  public TextMeshProUGUI ScoreLabel;

  public GameObject GameOverPanel;
  public TextMeshProUGUI GameOverScoreLabel;
  public Button GameOverRestartButton;

  public float ShipMaxSpeed = 0.2f;

  private Game game { get; set; }

  private Ship ship { get; set; }

  private void Start() {
    game = new Game(new UnityLogger(), new GameConfig {
      ShipMaxSpeed = ShipMaxSpeed
    });

    GameOverRestartButton.onClick.AddListener(HandlerRestartGameButtonClicked);

    AttachGameEvents();

    game.RestartGame();
  }

  private void AttachGameEvents() {
    game.Events.GameObjectCreated += HandlerGameObjectCreated;
    game.Events.ScoreChanged += HandlerScoreChange;
    game.Events.GameOver += HandlerGameOver;
  }

  private void DetachGameEvents() {
    game.Events.GameObjectCreated -= HandlerGameObjectCreated;
    game.Events.ScoreChanged -= HandlerScoreChange;
    game.Events.GameOver -= HandlerGameOver;
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.UpArrow)) game.Controls.PressForward();
    if (Input.GetKeyUp(KeyCode.UpArrow)) game.Controls.ReleaseForward();

    if (Input.GetKeyDown(KeyCode.LeftArrow)) game.Controls.PressLeft();
    if (Input.GetKeyUp(KeyCode.LeftArrow)) game.Controls.ReleaseLeft();

    if (Input.GetKeyDown(KeyCode.RightArrow)) game.Controls.PressRight();
    if (Input.GetKeyUp(KeyCode.RightArrow)) game.Controls.ReleaseRight();

    if (Input.GetKeyUp(KeyCode.LeftControl)) game.Controls.PressAndReleaseLaser();
    if (Input.GetKeyUp(KeyCode.Space)) game.Controls.PressAndReleaseProjectile();

    game.Tick();
  }

  private void LateUpdate() {
    if (ship != null) {
      CoordinatesLabel.text = $"Coords: {ship.GetCoordinates().X:0.0}:{ship.GetCoordinates().Y:0.0}";
      AngleLabel.text = $"Angle: {ship.GetAngleRadians():0.0} (r)";
      SpeedLabel.text = $"Speed: {ship.GetSpeed():0.0}";
      LaserChargesLabel.text = $"Laser Charges: {ship.GetLaserCharges()}";
      LaserChargingProgressLabel.text = $"Laser Charging Progress: {ship.GetLaserChargingProgress():0.0}";
    }
  }

  private void HandlerRestartGameButtonClicked() {
    game.RestartGame();

    GameOverPanel.SetActive(false);
  }

  private void HandlerGameOver(object sender, EventArgs args) {
    GameOverScoreLabel.text = ScoreLabel.text;
    ship = null;
    GameOverPanel.SetActive(true);
  }

  private void HandlerScoreChange(object sender, ScoreChangedEvent args) {
    ScoreLabel.text = $"Score: {args.Score}";
  }

  private void HandlerGameObjectCreated(object sender, GameObjectCreatedEvent args) {
    GameObject prefab;

    if (args.GameObject is Ship) {
      prefab = ShipRendererPrefab;

      ship = (Ship) args.GameObject;
    } else if (args.GameObject is Projectile) {
      prefab = ProjectileRendererPrefab;
    } else if (args.GameObject is FlyingSaucer) {
      prefab = FlyingSaucerRendererPrefab;
    } else if (args.GameObject is Asteroid) {
      prefab = AsteroidRendererPrefab;
    } else if (args.GameObject is AsteroidShard) {
      prefab = AsteroidShardRendererPrefab;
    } else if (args.GameObject is Laser) {
      prefab = LaserRendererPrefab;
    } else {
      return;
    }

    var obj = Instantiate(prefab);
    var renderer = obj.AddComponent<AsteroidsGameObjectRendererComponent>();
    renderer.SetAsteroidsGameObject(args.GameObject);

    if (args.GameObject is Asteroid || args.GameObject is AsteroidShard || args.GameObject is FlyingSaucer) {
      renderer.VisualRotationEnabled = false;
    }

    // After setting asteroids game object we set parent to avoid pos jumping
    obj.transform.SetParent(transform);
  }

  // private void HandlerProjectileCreated(object sender, Projectile)

  private void OnDestroy() {
    DetachGameEvents();
    game.Dispose();
  }
}

public class UnityLogger : AsteroidsCore.Loggers.ILogger {
  public void LogError(string message) => Debug.LogError(message);

  public void LogInfo(string message) => Debug.Log(message);
}