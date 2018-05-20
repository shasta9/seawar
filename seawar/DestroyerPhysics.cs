namespace seawar {
   public class DestroyerPhysics : IPhysics {
      public bool CanMoveTo(Tile pos) {
         // not water?
         if (pos.IsLand) {
            // aground
            return false;
         }
         // port?
         // for each actor on the tile
         // merchant ship?
         //  collision
         // submarine?
         // surfaced?
         // resolve collision
         // periscope depth?
         // collision
         // submerged?
         // collision
         // torpedo?
         // mine?
         return true;
      }

      public Damage Collide(Tile pos) {
         return new Damage();
      }
   }
}
