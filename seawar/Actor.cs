using NodaTime;

namespace seawar {
   public class Actor {

      private readonly Game game;
      private IAction nextAction;

      public Actor(Game game) {
         this.game = game;
      }

      public Vec Position { get; set; } = new Vec(0, 0);
      public double BaseSpeed { get; set; } = 1.0;

      public void SetNextAction(IAction action) {
         nextAction = action;
      }

      public IAction GetNextAction() {
         return nextAction;
      }

      public bool CanOccupy(Vec pos) {
         return game.Stage.IsWater(pos);
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