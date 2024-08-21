using AsteroidsCore.Behaviours;
using AsteroidsCore.ECS.Components;
using AsteroidsCore.ECS.Components.Transform;
using AsteroidsCore.ECS.Systems;
using AsteroidsCore.Loggers;
using AsteroidsCore.World.Tasks;
using AsteroidsCore.Worlds.Commands;
using System;
using System.Collections.Generic;

namespace AsteroidsCore.ECS.Entities {
  public class Entity :
    IHasCommands,
    IHasLogger,
    IHasUpdateBehaviour,
    IHasCreateBehaviour,
    IHasDestroyBehaviour {

    public int Id { get; private set; }

    public bool MarkedForDestruction { get; private set; } = false;

    public bool Accessible => MarkedForDestruction;

    private ILogger? logger;

    protected Commands? commands { get; set; }

    private ComponentBag components { get; } = new ComponentBag();

    private SystemBag systems { get; } = new SystemBag();

    private bool entityHasUpdatableSystems = false;

    // Switch variable to check whether OnCreate systems right away
    // or postpone because entity is not added to the world
    private bool entityInGameWorld => commands != null;

    /// <summary>
    /// Internal method for game world. Do not use manually.
    /// </summary>
    /// <param name="id"></param>
    public void SetId(int id) {
      Id = id;
    }

    public Entity() {
      // By default every entity should have transform component
      AddComponent<TransformComponent>();
    }

    public void AddEntityToGameWorld(Entity entity) {
      commands!.AddCommand(new AddEntityCommand(entity));
    }

    public T AddComponent<T>() where T : Component, new() {
      var c = new T();
      components.Add(c);
      return c;
    }

    public T? AddSystem<T>() where T : Systems.System, new() {
      var system = new T();

      system.SetEntity(this);

      systems.Add(system);

      if (system is IHasUpdateBehaviour) {
        entityHasUpdatableSystems = true;
      }

      try {
        // Entity already in game world, we should .OnCreate system right away
        if (entityInGameWorld) {
          if (system is IHasCreateBehaviour hasCreate) {
            hasCreate.OnCreate();
          }
        }
      } catch (Exception ex) {
        logger!.LogError($"Error adding system to component with OnCreate behaviour: {ex}");

        return null;
      }

      return system;
    }

    public List<T> FindSystemsWhichImplement<T>() {
      var found = new List<T>();

      foreach (var system in systems) {
        if (system is T implements) {
          found.Add(implements);
        }
      }

      return found;
    }

    public T? GetComponent<T>() where T : Component, new() {
      if (components.TryGetValue(typeof(T), out var component)) return (T) component;

      return null;
    }

    public T? GetComponentThatImplements<T>() where T : Component, new() {
      foreach (var component in components) {
        if (component is T) return (T) component;
      }

      return null;
    }

    public T GetOrCreateComponent<T>() where T : Component, new() {
      if (components.TryGetValue(typeof(T), out var component)) return (T) component;

      return AddComponent<T>();
    }

    public T? GetSystem<T>() where T : Systems.System {
      if (systems.TryGetValue(typeof(T), out var system)) return (T) system;

      return null;
    }

    /// <summary>
    /// Method updates all systems if there are any that need updates
    /// </summary>
    public void OnUpdate() {
      // Using cached state not to iterate through all systems
      if (!entityHasUpdatableSystems) return;

      foreach (var system in systems) {
        if (!system.Active) continue;

        if (system is IHasUpdateBehaviour updatable) {
          try {
            updatable.OnUpdate();
          } catch (Exception ex) {
            logger!.LogError($"Error updating system: {ex}");
          }
        }
      }
    }

    /// <summary>
    /// Use this to destroy entity and it's components.
    /// </summary>
    public void Destroy() {
      if (MarkedForDestruction) return;

      MarkedForDestruction = true;

      commands!.AddCommand(new DestroyEntityCommand(Id));
    }

    /// <summary>
    /// Internal lifecycle method for entity. Do not use manually.
    /// </summary>
    public void OnCreate() {
      systems.OnCreate();
    }

    /// <summary>
    /// Internal lifecycle method for entity. Do not use manually.
    /// </summary>
    public void OnDestroy() {
      systems.OnDestroy();
    }

    public void SetCommands(Commands commandQueue) => this.commands = commandQueue;

    public Commands GetCommands() => commands!;

    public void SetLogger(ILogger logger) => this.logger = logger;

    public ILogger GetLogger() => logger!;
  }
}
