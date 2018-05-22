using seawar.Actors;
using seawar.Game;

namespace seawar.Physics {
   public class MoveAnywherePhysics : IPhysics {
      public MoveResult TryMoveTo(Tile pos) {
         return MoveResult.Success();
      }

      public Damage Collide(Tile pos) {
         return new Damage();
      }
   }
}