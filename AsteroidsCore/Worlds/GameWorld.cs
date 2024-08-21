using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Loggers;
using AsteroidsCore.Physics.Worlds;
using AsteroidsCore.Utils.Geometry;
using AsteroidsCore.World.Events;
using AsteroidsCore.World.Tasks;
using AsteroidsCore.Worlds.Commands;
using AsteroidsCore.Worlds.Commands.Executors;
using AsteroidsCore.Worlds.Ids;
using AsteroidsCore.Worlds.Pools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsteroidsCore.World {
  public class GameWorld : IDisposable, IHasLogger {
    public Vec2 MapSize { get; }

    public GameWorldEvents Events { get; } = new();

    public bool Active { get; set; } = true;

    private EntityPool entityPool { get; } = new();

    private Commands commands = new();

    private List<Entity> physicsEntities { get; } = new();

    private IIdGenerator idGenerator { get; }

    private CommandExecutor commandExecutor { get; }

    private ILogger logger { get; set; }

    private PhysicsWorld physicsWorld { get; set; }

    private bool threadsEnabled { get; set; } = false;

    public GameWorld(
      IIdGenerator idGenerator,
      ILogger logger,
      Vec2 mapSize
    ) {
      this.idGenerator = idGenerator;
      MapSize = mapSize;
      this.logger = logger;

      physicsWorld = new PhysicsWorld(Events, logger);

      commandExecutor = new CommandExecutor(
        commands,
        entityPool,
        idGenerator,
        logger,
        Events
      );
    }

    public void EnableThreads() {
      threadsEnabled = true;
      physicsWorld.EnableThreads();
      commandExecutor.EnableThreads();
    }

    public void Tick() {
      if (!Active) return;

      // First we process commands as top priority.
      ExecuteCommandsInTick();

      ProcessEntitiesInTick();

      ProcessPhysicsWorldInTick();
    }

    protected virtual void ExecuteCommandsInTick() {
      commandExecutor.ExecuteCommands(commands);
    }

    protected virtual void ProcessEntitiesInTick() {
      if (threadsEnabled) {
        Parallel.ForEach(entityPool.Entities.Values, EntityTick);
      } else {
        foreach (var entity in entityPool.Entities.Values) {
          EntityTick(entity);
        }
      }
    }

    protected virtual void ProcessPhysicsWorldInTick() {
      physicsWorld.Tick();
    }

    protected void EntityTick(Entity entity) {
      if (!entity.MarkedForDestruction) {
        entity.OnUpdate();
      }
    }

    public void AddEntity(Entity entity) {
      commands.AddCommand(new AddEntityCommand(entity));
    }

    public void RemoveAllEntities(bool immediately = false, bool noNewCommands = false) {
      foreach (var entity in entityPool.Entities.Values) {
        commands.AddCommand(new DestroyEntityCommand(entity.Id));
      }

      if (immediately) {
        if (noNewCommands) commands.RouteToNowhere();
        
        commandExecutor.ExecuteCommands(commands);

        commands.RouteToMain();
        
        // Just to be sure...
        if (entityPool.Count > 0) entityPool.Clear();
      }
    }

    public void Dispose() {
      Active = false;

      RemoveAllEntities(immediately: true, noNewCommands: true);
      physicsWorld.Dispose();
    }

    public void SetLogger(ILogger logger) => this.logger = logger;

    public ILogger GetLogger() => logger;
  }
}
