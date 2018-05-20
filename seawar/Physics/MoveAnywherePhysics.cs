using seawar.Actors;
using seawar.Game;

namespace seawar.Physics {
   public class MoveAnywherePhysics : IPhysics {
      public bool CanMoveTo(Tile pos) {
         return true;
      }

      public Damage Collide(Tile pos) {
         return new Damage();
      }
   }
}