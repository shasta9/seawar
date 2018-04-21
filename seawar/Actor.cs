using NodaTime;

namespace seawar {
   public class Actor {

      private Game game;
      private bool isMoving;
      private Move move;
      private double distance;
      private Vec moveEndPos;

      public Actor(Game game) {
         this.game = game;
      }

      public Vec Position { get; set; } = new Vec(0, 0);
      public double BaseSpeed { get; set; } = 1.0;

      public void Update(Duration delta) {
         if (isMoving) Move(delta);
      }

      public void StartMove(Move move) {
         this.move = move;
         distance = 0.0;
         moveEndPos = move.Vector * move.Distance + Position;
         isMoving = true;
      }

      private void Move(Duration delta) {
         var deltaDist = delta.TotalSeconds * move.Speed * BaseSpeed;
         distance += deltaDist;
         if (distance >= move.Vector.Length) {
            // actor is about to move
            // check that the destination tile is water
            if (game.Stage.IsLand(Position + move.Vector)) {
               StopMove();
               return;
            }
            // check what else might be at this position
            // move
            Position += move.Vector;
            distance -= move.Vector.Length;
         }
         if (Position == moveEndPos) StopMove();
      }

      private void StopMove() {
         isMoving = false;
      }
   }
}