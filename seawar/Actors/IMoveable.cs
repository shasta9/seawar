using seawar.Physics;
using seawar.Vectors;

namespace seawar.Actors {
   public interface IMoveable {
      Vec Position { get; set; }
      double BaseSpeed { get; set; }
      MoveResult TryMoveTo(Vec pos);
      void Collide(MoveResult result, double speed);
      void Aground(double speed);
   }
}