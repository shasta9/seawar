using System.Collections.Generic;
using NodaTime;
using NUnit.Framework.Constraints;

namespace seawar {
   public class Actor {

      private readonly Game game;
      private readonly List<IAction> actions=new List<IAction>();

      public Actor(Game game) {
         this.game = game;
      }

      public Vec Position { get; set; } = new Vec(0, 0);
      public double BaseSpeed { get; set; } = 1.0;

      public void AddAction(IAction action) {
         actions.Add( action);
      }

      public void RemoveAction(IAction action) {
         actions.Remove(action);
      }

      public IEnumerable<IAction> GetActions() {
         return actions;
      }

      public bool CanOccupy(Vec pos) {
         return true;
         //return game.Stage.IsWater(pos);
      }
   }

   public interface IAction {
      bool IsComplete { get; }
      void Perform(Duration delta);
   }

   public class MoveAction : IAction {
      private readonly Actor actor;
      private readonly Move move;
      private readonly Vec moveEndPos;
      private double distance;

      public MoveAction(Actor actor, Move move) {
         this.actor = actor;
         this.move = move;
         moveEndPos = actor.Position + move.Vector * move.Distance;
      }

      public bool IsComplete { get; private set; }

      public void Perform(Duration delta) {
         var deltaDist = delta.TotalSeconds * move.Speed * actor.BaseSpeed;
         distance += deltaDist;
         if (distance < move.Vector.Length) return;
         // actor is about to move, check new position is OK
         if (!actor.CanOccupy(actor.Position + move.Vector)) {
            IsComplete = true;
            return;
         }
         // move actor
         actor.Position = actor.Position + move.Vector;
         if (actor.Position == moveEndPos) {
            IsComplete = true;
            return;
         }
         distance = distance - move.Vector.Length;
      }
   }
}