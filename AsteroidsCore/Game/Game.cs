using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Game.Components;
using AsteroidsCore.Game.Configs;
using AsteroidsCore.Game.Controls;
using AsteroidsCore.Game.Events;
using AsteroidsCore.Game.Events.Objects;
using AsteroidsCore.Game.Objects;
using AsteroidsCore.Game.Systems;
using AsteroidsCore.Loggers;
using AsteroidsCore.Physics.Components;
using AsteroidsCore.Physics.Systems;
using AsteroidsCore.Utils;
using AsteroidsCore.Utils.Geometry;
using AsteroidsCore.World;
using AsteroidsCore.World.Events.Entities;
using AsteroidsCore.Worlds.Events.Entities;
using AsteroidsCore.Worlds.Ids;
using System;
using System.Threading;

namespace AsteroidsCore.Game {
  [Flags]
  internal enum PhysicsLayers {
    Player = 0,
    Asteroid = 1,
    FlyingSaucer = 2
  }

  public class Game : IDisposable, IHasLogger {
    public AsteroidGameEvents Events { get; }

    private GameWorld gameWorld { get; }

    private int _score = 0;

    private ILogger logger { get; set; }

    private Random random = new();

    public int Score {
      get => _score;

    }

    public GameControls Controls { get; }

    private TransformComponent? shipTransformComponent;
    private ShipSystem? shipSystem { get; set; }

    private GameConfig config { get; set; }

    private bool threadsEnabled { get; set; } = false;

    private long lastSpawnActionAtMs { get; set; } = DateTime.Now.ToUnixTimeMs();

    public Game(ILogger? logger = null, GameConfig? config = null) {
      Events = new AsteroidGameEvents();
      Controls = new GameControls();

      this.logger = logger ?? new ConsoleLogger();

      this.config = config ?? new GameConfig();

      gameWorld = new GameWorld(
        new IdGenerator(),
        this.logger,
        new Vec2(this.config.MapWidth, this.config.MapHeight)
      );

      gameWorld.SetLogger(this.logger!);

      AttachGameWorldEvents();
      AttachControls();
    }

    public void EnableThreads() => gameWorld.EnableThreads();

    public void RestartGame() {
      gameWorld.RemoveAllEntities(immediately: true, noNewCommands: true);

      shipSystem = null;
      shipTransformComponent = null;

      CreateStarterGameEntities();

      CreateFlyingSaucer();

      CreateAsteroid();
      CreateAsteroid();
      CreateAsteroid();
      CreateAsteroid();
      CreateAsteroid();
      CreateAsteroid();
      CreateAsteroid();
      CreateAsteroid();
      CreateAsteroid();

      Interlocked.Exchange(ref _score, 0);
    }

    public void CreateStarterGameEntities() {
      CreateShip();
    }

    private void CreateShip() {
      var ship = new Entity();

      var circleCollider = ship.AddComponent<CircleColliderComponent>();
      circleCollider.Radius = 0.3f;

      ship.AddSystem<PhysicsSystem>();

      // Configuring movement component prior with desired speed
      var movement = ship.GetOrCreateComponent<MovementComponent>();
      movement.MaxSpeed = config!.ShipMaxSpeed;

      // Basic movement and ship system for shooting etc
      ship.AddSystem<MovementSystem>();
      shipSystem = ship.AddSystem<ShipSystem>();

      // Wrapper to teleport ship on sides
      var mapWrapper = ship.AddComponent<MapPositionWrapperComponent>();
      mapWrapper.Center = Vec2.Zero;
      mapWrapper.Size = new Vec2(config.MapWidth, config.MapHeight);
      ship.AddSystem<MapPositionWrapperSystem>();

      shipTransformComponent = ship.GetComponent<TransformComponent>();

      ship.AddSystem<SpeedSystem>();

      gameWorld.AddEntity(ship);
    }

    private void CreateFlyingSaucer() {
      var entity = new Entity();

      var circleCollider = entity.AddComponent<CircleColliderComponent>();
      circleCollider.Radius = 0.3f;

      entity.AddSystem<PhysicsSystem>();

      entity.AddComponent<MovementComponent>();
      entity.AddSystem<MovementSystem>();

      entity.AddComponent<FollowerComponent>();
      entity.AddSystem<FollowerSystem>();

      var transform = entity.GetComponent<TransformComponent>();
      transform!.Pos = GetPositionFarFromShip();

      var flyingSaucerComponent = entity.AddComponent<FlyingSaucerComponent>();
      flyingSaucerComponent.TargetTransformComponent = shipTransformComponent;

      entity.AddSystem<FlyingSaucerSystem>();

      gameWorld.AddEntity(entity);
    }

    private void CreateAsteroid() {
      var entity = new Entity();

      var circleCollider = entity.AddComponent<CircleColliderComponent>();
      circleCollider.Radius = 0.3f;

      entity.AddSystem<PhysicsSystem>();

      entity.AddComponent<MovementComponent>();
      entity.AddSystem<MovementSystem>();

      entity.AddComponent<FollowerComponent>();
      entity.AddSystem<FollowerSystem>();

      entity.AddComponent<AsteroidComponent>();

      entity.AddSystem<AsteroidSystem>();

      var transform = entity.GetComponent<TransformComponent>();
      transform!.Pos = GetPositionFarFromShip();

      gameWorld.AddEntity(entity);
    }

    private Vec2 GetPositionFarFromShip(int iterations = 0) {
      if (shipTransformComponent == null) return Vec2.Zero;

      var minDistance = config.MapHeight / 8;

      var center = Vec2.Zero;

      var xSide = config.MapWidth / 2;
      var ySide = config.MapHeight / 2;

      var position = new Vec2(
        random.Next((int)center.X - (int)xSide, (int)center.X + (int)xSide),
        random.Next((int)center.Y - (int)ySide, (int)center.Y + (int)ySide)
      );

      if (position.DistanceTo(shipTransformComponent!.Pos) < minDistance) {
        if (iterations > 10) return position;

        return GetPositionFarFromShip(iterations + 1);
      }

      return position;
    }

    public void Tick() {
      PotentiallySpawn();

      gameWorld?.Tick();
    }

    private void PotentiallySpawn() {
      if (shipSystem == null) return;

      var diffMs = (DateTime.Now.ToUnixTimeMs() - lastSpawnActionAtMs);

      if (diffMs < 3000) return;

      if (random.Next(0, 100) > 90) return;

      if (random.Next(0, 100) > 50) {
        CreateAsteroid();
      } else {
        CreateFlyingSaucer();
      }

      lastSpawnActionAtMs = DateTime.Now.ToUnixTimeMs();
    }

    public void Dispose() {
      DetachControls();
      DetachGameWorldEvents();
      gameWorld.Dispose();
    }

    private void AttachGameWorldEvents() {
      gameWorld.Events.EntityCreated += HandlerWorldEntityCreated;
    }

    private void DetachGameWorldEvents() {
      gameWorld.Events.EntityCreated -= HandlerWorldEntityCreated;
    }

    private void AttachControls() {
      Controls.ForwardPressed += HandlerForwardPressed;
      Controls.ForwardReleased += HandlerForwardReleased;
      Controls.LeftPressed += HandlerLeftPressed;
      Controls.LeftReleased += HandlerLeftReleased;
      Controls.RightPressed += HandlerRightPressed;
      Controls.RightReleased += HandlerRightReleased;
      Controls.LaserPressedAndReleased += HandlerLaserPressedAndReleased;
      Controls.ProjectilePressedAndReleased += HandlerProjectilePressedAndReleased;
    }

    private void DetachControls() {
      Controls.ForwardPressed -= HandlerForwardPressed;
      Controls.ForwardReleased -= HandlerForwardReleased;
      Controls.LeftPressed -= HandlerLeftPressed;
      Controls.LeftReleased -= HandlerLeftReleased;
      Controls.RightPressed -= HandlerRightPressed;
      Controls.RightReleased -= HandlerRightReleased;
      Controls.LaserPressedAndReleased -= HandlerLaserPressedAndReleased;
      Controls.ProjectilePressedAndReleased -= HandlerProjectilePressedAndReleased;
    }

    // Controls event handlers
    private void HandlerForwardPressed(object s, EventArgs e) =>
      shipSystem?.EnableAcceleration();
    private void HandlerForwardReleased(object s, EventArgs e) =>
      shipSystem?.DisableAcceleration();
    private void HandlerLeftPressed(object s, EventArgs e) =>
      shipSystem?.EnableRotatingLeft();
    private void HandlerLeftReleased(object s, EventArgs e) =>
      shipSystem?.DisableRotatingLeft();
    private void HandlerRightPressed(object s, EventArgs e) =>
      shipSystem?.EnableRotatingRight();
    private void HandlerRightReleased(object s, EventArgs e) =>
      shipSystem?.DisableRotatingRight();
    private void HandlerProjectilePressedAndReleased(object s, EventArgs e) =>
      shipSystem?.ShootProjectile();
    private void HandlerLaserPressedAndReleased(object s, EventArgs e) =>
      shipSystem?.ShootLaser();

    // World event handlers
    private void HandlerWorldEntityCreated(object s, EntityCreatedEvent e) {
      var entity = e.Entity;

      if (entity.GetSystem<ShipSystem>() != null) {
        var obj = new Ship(entity, gameWorld.Events);

        ListenToGameObjectDestruction(obj);

        Events.EmitGameObjectCreated(obj);
      } else if (entity.GetSystem<ProjectileSystem>() != null) {
        Events.EmitGameObjectCreated(new Projectile(entity, gameWorld.Events));
      } else if (entity.GetSystem<FlyingSaucerSystem>() != null) {
        var obj = new FlyingSaucer(entity, gameWorld.Events);

        ListenToGameObjectDestruction(obj);

        Events.EmitGameObjectCreated(obj);
      } else if (entity.GetSystem<AsteroidSystem>() != null) {
        var obj = new Asteroid(entity, gameWorld.Events);

        ListenToGameObjectDestruction(obj);

        Events.EmitGameObjectCreated(obj);
      } else if (entity.GetSystem<AsteroidShardSystem>() != null) {
        var obj = new AsteroidShard(entity, gameWorld.Events);

        ListenToGameObjectDestruction(obj);

        Events.EmitGameObjectCreated(obj);
      } else if (entity.GetSystem<LaserSystem>() != null) {
        Events.EmitGameObjectCreated(new Laser(entity, gameWorld.Events));
      }
    }

    private void ListenToGameObjectDestruction(AsteroidsGameObject gameObject) {
      gameObject.Destroyed += HandlerGameObjectDestroyed;
    }

    private void HandlerGameObjectDestroyed(object sender, GameObjectDestroyedEvent e) {
      var obj = e.GameObject;
      // Self unsubscribe. Uh-uh
      obj.Destroyed -= HandlerGameObjectDestroyed;


      ;


      if (shipSystem != null) {
        if (obj is FlyingSaucer) Interlocked.Add(ref _score, 10);

        if (obj is Asteroid) Interlocked.Add(ref _score, 5);

        if (obj is AsteroidShard) Interlocked.Add(ref _score, 2);

        Events.EmitScoreChanged(Score);
      }

      if (obj is Ship) {
        GameOver();
      }
    }

    private void GameOver() {
      shipSystem = null;
      shipTransformComponent = null;

      Events.EmitGameOver();
    }

    public void SetLogger(ILogger logger) => this.logger = logger;

    public ILogger GetLogger() => logger;
  }
}
