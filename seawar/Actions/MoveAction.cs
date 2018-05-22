using System;
using NodaTime;
using seawar.Actors;
using seawar.Physics;
using seawar.Vectors;

namespace seawar.Actions {
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
         // how far have we moved sine the last update
         var deltaDist = delta.TotalSeconds * move.Speed * actor.BaseSpeed;
         // accumulate distance
         distance += deltaDist;
         if (distance < move.Vector.Length) return;
         // actor has built up enough distance to move to a new tile
         var result = actor.TryMoveTo(actor.Position + move.Vector);
         switch (result.Result) {
            case MoveResults.Ok:
               // actor can move and nothing else happens
               actor.Position += move.Vector;
               // check if move is complete
               if (actor.Position == moveEndPos) {
                  IsComplete = true;
                  return;
               }
               // decrement accumulated distance
               distance = distance - move.Vector.Length;
               break;
            case MoveResults.InPort:
               // actor is in port
               actor.Position += move.Vector;
               IsComplete = true;
               actor.AddAction(InPortAction.For(actor));
               break;
            case MoveResults.Aground:
               actor.Aground(move.Speed);
               IsComplete = true;
               break;
            case MoveResults.Collision:
               actor.Collide(result, move.Speed);
               IsComplete = true;
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
   }

   public abstract class InPortAction : IAction {

      public bool IsComplete { get; }

      public abstract void Perform(Duration delta);

      public static InPortAction For(Actor actor) {
         switch (actor) {
            case Destroyer destroyer:
               return new DestroyerInPortAction(destroyer);
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
   }

   public class DestroyerInPortAction : InPortAction {
      private readonly Destroyer destroyer;

      public DestroyerInPortAction(Destroyer destroyer) {
         this.destroyer = destroyer;

      }

      public override void Perform(Duration delta) {
         throw new NotImplementedException();
      }
   }
}
