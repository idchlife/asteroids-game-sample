using System;
using System.Collections.Concurrent;

namespace AsteroidsCore.ECS.Components {
  public sealed class ComponentBag {
    public ConcurrentDictionary<Type, Component> Components = new();

    public void Add(Component component) {
      Components.TryAdd(component.GetType(), component);
    }

    public bool TryGetValue(Type componentType, out Component? component) {
      if (Components.TryGetValue(componentType, out Component c)) {
        component = c;
        return true;
      } else {
        component = null;
        return false;
      }
    }
  }
}
