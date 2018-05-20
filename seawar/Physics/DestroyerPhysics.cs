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
}
