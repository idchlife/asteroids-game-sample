using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Loggers;
using AsteroidsCore.Worlds.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.ECS.Systems {
  public class System {
    private Entity? entity { get; set; }

    public bool Active { get; set; } = true;

    public void SetEntity(Entity entity) => this.entity = entity;

    protected Entity GetEntity() => entity!;

    protected ILogger? logger => GetEntity().GetLogger();
  }
}
