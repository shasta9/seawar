using seawar.Actors;
using seawar.Game;

namespace seawar.Physics {
   public interface IPhysics {
      bool CanMoveTo(Tile pos);
      Damage Collide(Tile pos);
   }
}
