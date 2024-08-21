using AsteroidsCore.ECS.Entities;
using AsteroidsCore.Physics.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Physics.Listeners {
  public interface ICollisionStartDetector {
    public void OnCollisionStart(PhysicsSystem physicsSystem);
  }
}