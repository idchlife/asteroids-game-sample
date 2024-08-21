using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Worlds.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.World.Tasks
{
    public class AddEntityCommand : Command {
    public Entity Entity { get; }

    public AddEntityCommand(Entity entity) => Entity = entity;
  }
}
