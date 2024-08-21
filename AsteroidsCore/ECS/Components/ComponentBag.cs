using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AsteroidsCore.ECS.Components {
  public sealed class ComponentBag : KeyedCollection<Type, Component> {
    protected override Type GetKeyForItem(Component item) => item.GetType();
  }
}
