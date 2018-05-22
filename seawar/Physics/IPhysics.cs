using seawar.Actors;
using seawar.Game;

namespace seawar.Physics {
   public interface IPhysics {
      MoveResult TryMoveTo(Tile pos);
      Damage Collide(Tile pos);
   }
}
