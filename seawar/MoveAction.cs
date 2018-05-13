using NodaTime;

namespace seawar {
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
         // actor is about to move, get move result
         MoveResult result = actor.TryMoveBy(move.Vector);
         if (result.Success) {
            // move actor
            actor.MoveBy(move.Vector);
            if (actor.Position == moveEndPos) {
               IsComplete = true;
               return;
            }
            distance = distance - move.Vector.Length;
            return;
         }
         // stop the move
         IsComplete = true;
      }
   }

   public class MoveResult {
      public bool Success { get; private set; }
   }
}
