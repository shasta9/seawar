using NodaTime;

namespace seawar {
   public class MoveAction : IAction {
      private readonly IMoveable moveable;
      private readonly Move move;
      private readonly Vec moveEndPos;
      private double distance;

      public MoveAction(IMoveable moveable, Move move) {
         this.moveable = moveable;
         this.move = move;
         moveEndPos = moveable.Position + move.Vector * move.Distance;
      }

      public bool IsComplete { get; private set; }

      public void Perform(Duration delta) {
         var deltaDist = delta.TotalSeconds * move.Speed * moveable.BaseSpeed;
         distance += deltaDist;
         if (distance < move.Vector.Length) return;
         // moveable is ready to move
         if (moveable.CanMoveTo(moveable.Position + move.Vector)) {
            // moveable has moved
            moveable.Position += move.Vector;
            // check if move is complete
            if (moveable.Position == moveEndPos) {
               IsComplete = true;
               return;
            }
            // decrement distance
            distance = distance - move.Vector.Length;
         }
         else {
            // move failed, resolve damage etc
            var dmg = moveable.Collide(moveable.Position + move.Vector);
            // set IsComplete to stop further movement
            IsComplete = true;
         }
      }
   }
}
