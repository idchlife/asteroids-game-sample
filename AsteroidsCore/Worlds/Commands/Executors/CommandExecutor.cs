using AsteroidsCore.Loggers;
using AsteroidsCore.Physics.Worlds;
using AsteroidsCore.World.Events;
using AsteroidsCore.World.Tasks;
using AsteroidsCore.Worlds.Ids;
using AsteroidsCore.Worlds.Pools;
using System;
using System.Threading.Tasks;

namespace AsteroidsCore.Worlds.Commands.Executors {
  /// <summary>
  /// Executor of all commands that are stored in command
  /// queue.
  /// 
  /// Mainly commands are for adding/removing entities
  /// </summary>
  public sealed class CommandExecutor {
    private Commands commands { get; }

    private EntityPool entityPool { get; }

    private IIdGenerator idGenerator { get; }

    private ILogger logger { get; }

    private GameWorldEvents gameWorldEvents { get; }

    private bool threadsEnabled { get; set; } = false;

    public CommandExecutor(
      Commands commands,
      EntityPool entityPool,
      IIdGenerator idGenerator,
      ILogger logger,
      GameWorldEvents gameWorldEvents
    ) {
      this.commands = commands;
      this.entityPool = entityPool;
      this.idGenerator = idGenerator;
      this.logger = logger;
      this.gameWorldEvents = gameWorldEvents;
    }

    public void EnableThreads() => threadsEnabled = true;

    public void ExecuteCommands(Commands commands) {
      // We need to switch enqueue to temp because after processing commands there is a chance that
      // new commands will be spawned and we need to process current commands first.
      commands.RouteToTemp();

      if (threadsEnabled) {
        Parallel.For(
          0,
          commands.Size,
          (i) => {
            commands.TryRetrieveAndRemoveCommand(out var command);

            if (command != null) ExecuteCommand(command);
          }
        );
      } else {
        while (commands.Size > 0) {
          var command = commands.RetrieveAndRemoveCommand();
          if (command != null) ExecuteCommand(command);
        }
      }
      

      

      // There can be commands gathered after processing another commands
      // E.g. in OnCreate method entities or components were added
      commands.TransferAllFromTempToMain();
      commands.RouteToMain();
    }

    private void ExecuteCommand(Command command) {
      if (command is AddEntityCommand addEntity) ExecuteCommand(addEntity);
      if (command is DestroyEntityCommand destroyEntity) ExecuteCommand(destroyEntity);
    }

    private void ExecuteCommand(AddEntityCommand command) {
      var entity = command.Entity;

      // Providing required dependencies for entity to
      // function properly
      entity.SetId(idGenerator.GetNextId());
      entity.SetCommands(commands);
      entity.SetLogger(logger);

      try {
        entity.OnCreate();

        entityPool.Add(entity);

        gameWorldEvents.EmitEntityCreated(entity);
      } catch (Exception ex) {
        logger.LogError($"Error adding entity to entity pool. More info: {ex}");
        throw;
      }
    }

    private void ExecuteCommand(DestroyEntityCommand command) {
      var entity = entityPool.FindById(command.EntityId);

      if (entity == null) {
        logger.LogInfo($"Tried destroying entity by id {command.EntityId} but it was null. Weird.");
        return;
      }

      var entityId = entity.Id;

      try {
        entity.OnDestroy();
      } catch (Exception ex) {
        logger.LogError($"Error executing OnDestroy on Entity. More info: {ex}");
      } finally {
        entityPool.Remove(entity);

        gameWorldEvents.EmitEntityDestroyed(entityId);
      }
    }
  }
}
