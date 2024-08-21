using AsteroidsCore.ECS.Entities;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace AsteroidsCore.Worlds.Pools {
  public class EntityPool {
    public ConcurrentDictionary<int, Entity> Entities { get; } = new();

    public int Count => Entities.Count;

    public void Clear() {
      Entities.Clear();
    }

    public void Add(Entity entity) {
      Entities.TryAdd(entity.Id, entity);
    }

    public void Remove(Entity entity) {
      Entities.TryRemove(entity.Id, out entity);
    }

    public Entity? FindById(int id) {
      if (Entities.TryGetValue(id, out var entity)) return entity;

      return null;
    }
  }
}
