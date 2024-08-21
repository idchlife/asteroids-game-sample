using AsteroidsCore.Loggers;
using AsteroidsCore.Physics.Components;
using AsteroidsCore.Physics.Systems;
using AsteroidsCore.Utils.Geometry;
using AsteroidsCore.World.Events;
using AsteroidsCore.World.Events.Entities;
using AsteroidsCore.Worlds.Events.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidsCore.Physics.Worlds {
  public class PhysicsWorld : IDisposable {
    private PhysicsSystemsStorage physicsSystemsStorage { get; set; } = new();

    private GameWorldEvents gameWorldEvents { get; set; }

    private ConcurrentDictionary<string, bool> collidedEntityIds = new();

    private ILogger logger {  get; set; }

    private bool threadsEnabled { get; set; } = false;

    public PhysicsWorld(
      GameWorldEvents gameWorldEvents,
      ILogger logger
    ) {
      this.gameWorldEvents = gameWorldEvents;
      this.logger = logger;

      AttachGameWorldEvents();
    }

    public bool EnableThreads() => threadsEnabled = true;

    public void AddPhysicsSystem(PhysicsSystem physicsSystem) {
      physicsSystemsStorage.Add(physicsSystem);
    }

    public void Tick() {
      DetectCollidingPhysicsSystems();
    }

    private void AttachGameWorldEvents() {
      gameWorldEvents.EntityDestroyed += HandlerEntityDestroyed;
      gameWorldEvents.EntityCreated += HandlerEntityCreated;
    }

    private void DetachGameWorldEvents() {
      gameWorldEvents.EntityDestroyed -= HandlerEntityDestroyed;
      gameWorldEvents.EntityCreated -= HandlerEntityCreated;
    }
    
    // Listening to entities being added and registering them if they
    // have physics system attached!
    private void HandlerEntityCreated(object sender, EntityCreatedEvent args) {
      var entity = args.Entity;

      var physicsSystem = entity.GetSystem<PhysicsSystem>();

      if (physicsSystem != null) {
        physicsSystem.SetRaycaster(Raycast);
        physicsSystemsStorage.Add(physicsSystem);
      }
    }

    private void HandlerEntityDestroyed(object sender, EntityDestroyedEvent args) {
      if (physicsSystemsStorage.TryGetValue(args.EntityId, out var system)) {
        physicsSystemsStorage.Remove(system);
      }
    }

    private void AddCollidedEntities(int entityIdA, int entityIdB) {
      var key = CreateCollidedEntitiesKey(entityIdA, entityIdB);

      if (!collidedEntityIds.ContainsKey(key)) {
        collidedEntityIds.TryAdd(key, true);
      }
    }

    private bool DidEntitiesCollide(int entityIdA, int entityIdB) =>
      collidedEntityIds.ContainsKey(CreateCollidedEntitiesKey(entityIdA, entityIdB));

    private string CreateCollidedEntitiesKey(int entityIdA, int entityIdB) {
      var builder = new StringBuilder();

      var first = entityIdA < entityIdB ? entityIdA : entityIdB;
      var second = entityIdA == first ? entityIdB : entityIdA;

      builder.Append(first.ToString());
      builder.Append(":");
      builder.Append(second.ToString());

      return builder.ToString();
    }
      

    private void DetectCollidingPhysicsSystems() {
      // This is onboxious but I don't have time rn to create proper
      // flags based collision matrix and stuff. Hopefully it works
      // fine with not so many physics objects
      // Best will be to use some physics engine in PhysicsWorld to actually
      // have performant implementation :) But for the sake of
      // simplicity...
      if (threadsEnabled) {
        Parallel.ForEach(physicsSystemsStorage, DetectCollidingPhysicsSystemsForSystem);
      } else {
        foreach (var system in physicsSystemsStorage) {
          DetectCollidingPhysicsSystemsForSystem(system);
        }
      }
    }

    private void DetectCollidingPhysicsSystemsForSystem(PhysicsSystem s1) {
      foreach (var s2 in physicsSystemsStorage) {
        var c1 = s1.GetColliderComponent();
        var c2 = s2.GetColliderComponent();

        if (c1 == null || c2 == null) continue;

        if (c1 is CircleColliderComponent collider1 && c2 is CircleColliderComponent collider2) {
          if (DoCircleCollidersCollide(collider1, collider2)) {
            var entity1Id = s1.GetEntity().Id;
            var entity2Id = s2.GetEntity().Id;
            
            // This actually wont happen in this game but oh well
            if (entity1Id == entity2Id) continue;

            if (!DidEntitiesCollide(entity1Id, entity2Id)) {
              AddCollidedEntities(entity1Id, entity2Id);
              NotifyPhysicsSystemAboutCollisionStart(s1, s2);
            }
          }
        }
      }
    }

    private List<PhysicsSystem> Raycast(Vec2 origin, Vec2 direction, float length) {
      var segmentStart = origin;
      var segmentEnd = origin + direction * length;

      var collidedWith = new List<PhysicsSystem>();

      foreach (var physicsSystem in physicsSystemsStorage) {
        var colliderComponent = physicsSystem.GetColliderComponent();

        if (colliderComponent is CircleColliderComponent circleCollider) {
          if (DoCircleColliderCollideWithSegment(circleCollider, segmentStart, segmentEnd)) {
            collidedWith.Add(physicsSystem);
          }
        }
      }

      return collidedWith;
    }

    private bool DoCircleCollidersCollide(CircleColliderComponent a, CircleColliderComponent b) {
      var minDistance = a.Radius + b.Radius;

      var centersDistance = a.Pos.DistanceTo(b.Pos);

      return centersDistance <= minDistance;
    }

    private bool DoCircleColliderCollideWithSegment(
      CircleColliderComponent circleCollider,
      Vec2 segmentStart,
      Vec2 segmentEnd
    ) {
      // Found this wonderful code here: https://codereview.stackexchange.com/questions/192477/circle-line-segment-collision
      double distance;

      var v1x = segmentEnd.X - segmentStart.X;
      var v1y = segmentEnd.Y - segmentStart.Y;
      var v2x = circleCollider.Pos.X - segmentStart.X;
      var v2y = circleCollider.Pos.Y - segmentStart.Y;
      // get the unit distance along the line of the closest point to
      // circle center
      var u = (v2x * v1x + v2y * v1y) / (v1y * v1y + v1x * v1x);
      
      
      // if the point is on the line segment get the distance squared
      // from that point to the circle center
      if(u >= 0 && u <= 1){
        distance  = Math.Pow(segmentStart.X + v1x * u - circleCollider.Pos.X, 2) + Math.Pow(segmentStart.Y + v1y * u - circleCollider.Pos.Y, 2);
      } else {
        // if closest point not on the line segment
        // use the unit distance to determine which end is closest
        // and get dist square to circle
        distance = u < 0 ?
              Math.Pow(segmentStart.X - circleCollider.Pos.X, 2) + Math.Pow(segmentStart.Y - circleCollider.Pos.Y, 2):
              Math.Pow(segmentEnd.X - circleCollider.Pos.X, 2) + Math.Pow(segmentEnd.Y - circleCollider.Pos.Y, 2);
      }
      return distance < circleCollider.Radius * circleCollider.Radius;
    }

    private void NotifyPhysicsSystemAboutCollisionStart(
      PhysicsSystem a,
      PhysicsSystem b
    ) {
      var collisionStartDetectorsA = a.FindCollisionStartDetectors();
      var collisionStartDetectorsB = b.FindCollisionStartDetectors();

      if (collisionStartDetectorsA.Count > 0) {
        foreach (var detector in collisionStartDetectorsA) {
          detector.OnCollisionStart(b);
        }
      }

      if (collisionStartDetectorsB.Count > 0) {
        foreach (var detector in collisionStartDetectorsB) {
          detector.OnCollisionStart(a);
        }
      }
    }

    public void Dispose() {
      physicsSystemsStorage.Clear();
    }
  }
}
