using AsteroidsCore.Behaviours;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AsteroidsCore.ECS.Systems {
  public enum SystemBagDestination {
    Main,
    Temp
  }

  public sealed class SystemBag : KeyedCollection<Type, System>, IHasCreateBehaviour, IHasDestroyBehaviour {
    private SystemBagDestination destination = SystemBagDestination.Main;

    private Queue<System> tempSystems = new();
    
    public new void Add(System system) {
      if (destination == SystemBagDestination.Temp) {
        tempSystems.Enqueue(system);
      } else {
        AddToMain(system);
      }
    }

    private void AddToMain(System system) {
      base.Add(system);
    }

    public void OnDestroy() {
      foreach (var system in this) {
        if (system is IHasDestroyBehaviour @destructable) {
          destructable.OnDestroy();
        }
      }
    }

    public void OnCreate() {
      // Protecting main systems collection while executing
      // OnCreate in systems
      // Why? Because we will modify collection inside loop
      // If we use another collection - we will loose
      // systems that were created in OnCreate in other systems.
      // They will not receive OnCreate execution.
      // So we use this 2 collections approach
      RouteToTemp();

      try {
        var systemsToProcess = new Queue<System>(Items);

        while (systemsToProcess.Count > 0) {
          var system = systemsToProcess.Dequeue();

          if (system is IHasCreateBehaviour @creatable) {
            try {
              creatable.OnCreate();
            } catch (Exception ex) {
              throw new SystemCreationException($"Error in {system.GetType()} system creation. More: {ex}");
            }
          }

          if (tempSystems.Count > 0) {
            // There can be byproduct of new systems created in another system OnCreate
            // we need to process them
            while (tempSystems.Count > 0) {
              var newArrivedSystem = tempSystems.Dequeue();

              // Adding also to main storage for them to be updated
              AddToMain(newArrivedSystem);

              systemsToProcess.Enqueue(newArrivedSystem);
            }
          }
        }
      } catch {
        throw;
      } finally {
        RouteToMain();
      }
    }

    private void RouteToTemp() => destination = SystemBagDestination.Temp;
    private void RouteToMain() => destination = SystemBagDestination.Main;

    protected override Type GetKeyForItem(System item) => item.GetType();
  }

  public class SystemCreationException : Exception {
    public SystemCreationException(string message) : base(message) { }
  }
}
