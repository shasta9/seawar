using System.Collections.Generic;
using NodaTime;
using seawar.Actions;
using seawar.Game;
using seawar.Physics;
using seawar.Vectors;

namespace seawar.Actors {

   public class Actor : IMoveable {

      private readonly List<IAction> actions = new List<IAction>();
      private readonly World world;
      private readonly IPhysics physics;

      public Actor(World world, IPhysics physics) {
         this.world = world;
         this.physics = physics;
      }

      public string Name { get; set; } = string.Empty;
      public Vec Position { get; set; } = Vec.Zero;
      public double BaseSpeed { get; set; }
      public bool HasExpired { get; set; }

      public void AddAction(IAction action) {
         actions.Add(action);
      }

      public void RemoveAction(IAction action) {
         actions.Remove(action);
      }

      public void Update(Duration delta) {
         var completeActions = new List<IAction>();
         foreach (var action in actions) {
            action.Perform(delta);
            if (action.IsComplete) {
               completeActions.Add(action);
            }
         }
         foreach (var action in completeActions) {
            RemoveAction(action);
         }
      }

      public MoveResult TryMoveTo(Vec pos) {
         var tile = world.GetTile(pos);
         return physics.TryMoveTo(tile);
      }

      public void Aground(double speed) {
         throw new System.NotImplementedException();
      }

      public void Collide(MoveResult result, double speed) {
      }
   }

   public class ActorEx {
      private readonly InBox inBox;
      private readonly OutBox outBox;
      private readonly World world;
      private readonly IPhysicsEx physics;

      public ActorEx(InBox inBox, OutBox outBox, World world, IPhysicsEx physics) {
         this.inBox = inBox;
         this.outBox = outBox;
         this.world = world;
         this.physics = physics;
      }

      public string Name { get; set; } = string.Empty;
      public Vec Position { get; set; } = Vec.Zero;
      public double BaseSpeed { get; set; } = 1.0;
      public bool HasExpired { get; set; }

      public void StartMove(Move move) {
         physics.StartMove(move);
      }

      public void StopMove() {
         physics.StopMove();
      }

      public void Update(Duration delta) {
         // get messages
         // update physics
         physics.Update(world, delta);
      }
   }

   public interface IPhysicsEx {
      void StartMove(Move move);
      void StopMove();
      void Update(World world, Duration delta);
   }

   public class DestroyerPhysics : IPhysicsEx {
      private readonly ActorEx destroyer;
      private readonly Mover mover;
      private Move currentMove;

      public DestroyerPhysics(ActorEx destroyer) {
         this.destroyer = destroyer;
         mover = new Mover(this);
      }

      public void StartMove(Move move) {
         currentMove = move;
      }

      public void StopMove() {
         currentMove = null;
      }

      public void Update(World world, Duration delta) {
         if (currentMove != null) {
            var result = mover.Update(delta);

         }
      }
   }

   public class Mover {
      private IPhysicsEx physics;

      public Mover(IPhysicsEx physics) {
         this.physics = physics;
      }

      public MoveResult Update(Duration delta) {
         throw new System.NotImplementedException();
      }
   }

   public class OutBox { }

   public class InBox { }
}
