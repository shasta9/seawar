using NodaTime;

namespace seawar {
   public class Actor {

      private bool isMoving;
      private Move move;
      private double distance;
      private double totalDistance;

      public Vec Position { get; set; } = new Vec(0, 0);
      public double BaseSpeed { get; set; } = 1.0;
      
      public void Update(Duration delta) {
        if (isMoving) Move(delta);
      }

      public void StartMove(Move move) {
         this.move = move;
         distance = 0.0;
         totalDistance = 0.0;
         isMoving = true;
      }

      private void Move(Duration delta) {
         var distanceInc = delta.TotalSeconds * move.Speed * BaseSpeed;
         distance += distanceInc;
         totalDistance += distanceInc;
         if (distance >= move.Vector.Length) {
            // actor is about to move
            // check that the destination tile is water
            // check what else might be at this position
            // move
            Position += move.Vector;
            distance -= move.Vector.Length;
         }
         if (totalDistance >= move.Distance) StopMove();
      }

      private void StopMove() {
         isMoving = false;
      }
   }
}