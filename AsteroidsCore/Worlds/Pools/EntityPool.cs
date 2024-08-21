using AsteroidsCore.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AsteroidsCore.Worlds.Pools
{
    public class EntityPool : KeyedCollection<int, Entity> {
    protected override int GetKeyForItem(Entity item) => item.Id;

    public Entity? FindById(int id) {
      if (TryGetValue(id, out var entity)) return entity;

      return null;
    }
  }
}
