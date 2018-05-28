using System.ComponentModel;
using NodaTime;
using seawar.Actions;
using seawar.Game;
using seawar.Vectors;

namespace seawar.Actors {
   public class DestroyerEx {
      private readonly World world;
      private Move currentMove;
      private Vec moveEndPos;
      private double distance;

      public DestroyerEx( World world) {
         this.world = world;
      }

      public string Name { get; set; } = string.Empty;
      public Vec Position { get; set; } = Vec.Zero;
      public double BaseSpeed { get; set; } = 1.0;
      public bool HasExpired { get; set; }

      public void StartMove(Move move) {
         distance = 0;
         currentMove = move;
         moveEndPos = Position + move.Vector * move.Distance;
      }

      public void StopMove() {
         currentMove = null;
      }

      public void Update(Duration delta) {
         // get messages
         // ...
         // are we moving?
         if (currentMove != null) {
            // how far have we moved since the last update
            var deltaDist = delta.TotalSeconds * currentMove.Speed * BaseSpeed;
            // accumulate distance
            distance += deltaDist;
            if (distance < currentMove.Vector.Length) return;
            // actor has built up enough distance to move to a new tile
            var tile = world.GetTile(Position + currentMove.Vector);
            // what is at that tile?
            if (tile.IsLand) {
               // aground
               currentMove = null;
            }
            else if (tile.IsDestroyerPort) {
               // in port
               currentMove = null;
            }
            else if (tile.Actors.Count != 0) {
               // something already in the tile
               currentMove = null;
            }
            else {
               Position += currentMove.Vector;
               distance = distance - currentMove.Vector.Length;
               if (Position == moveEndPos) currentMove = null;
            }
         }
      }
   }
}