using System.Collections.Generic;
using seawar.Actors;
using seawar.Game;

namespace seawar.Physics {
   public class DestroyerPhysics : IPhysics {
      public MoveResult TryMoveTo(Tile pos) {
         // empty sea?
         if (pos.IsWater && pos.Actors.Count == 0) return MoveResult.Success();
         // port?
         if (pos.IsDestroyerPort) return MoveResult.InPort();
         // aground?
         if (pos.IsLand) {
            var result = new MoveResult {
               Result = MoveResults.Aground,

            }
         }
         // for each actor on the tile
         
         // merchant ship?
         //  collision
         // submarine?
         // surfaced?
         //   collision
         // periscope depth?
         //   collision
         // submerged?
         //   collision
         // torpedo?
         // mine?
         return MoveResult.Success();
      }



      public Damage Collide(Tile pos) {
         return new Damage();
      }
   }

   public enum MoveResults {
      Ok,
      InPort,
      Aground,
      Collision
   }

   public class MoveResult {
      public MoveResults Result { get; set; }
      public List<Damage> Consequences { get; set; }

      public static MoveResult Success() => new MoveResult { Result = MoveResults.Ok, Consequences = null };
      public static MoveResult InPort() => new MoveResult { Result = MoveResults.InPort, Consequences = null };
   }
}
