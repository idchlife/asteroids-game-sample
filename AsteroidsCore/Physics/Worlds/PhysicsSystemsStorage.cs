using AsteroidsCore.Physics.Systems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AsteroidsCore.Physics.Worlds {
  public class PhysicsSystemsStorage : KeyedCollection<int, PhysicsSystem> {
    protected override int GetKeyForItem(PhysicsSystem item) => item.GetEntity().Id;
  }
}
