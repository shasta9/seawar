using System.Collections.Generic;
using NUnit.Framework;
using seawar.Actions;
using seawar.Actors;
using seawar.Game;

namespace seawar.Physics {
   public class DestroyerPhysics : IPhysics {
      public bool CanMoveTo(Tile pos) {
         // not water?
         if (pos.IsLand) return false;
         // port?
         if (pos.IsDestroyerPort) return true;
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
         return true;
      }



      public Damage Collide(Tile pos) {
         return new Damage();
      }
   }

   public class MoveResult {
      public bool Success { get; set; }
      public List<Damage> Consequences { get; set; }
   }
}
