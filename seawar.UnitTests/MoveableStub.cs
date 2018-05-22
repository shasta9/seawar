using seawar.Actors;
using seawar.Physics;
using seawar.Vectors;

namespace seawar.UnitTests {
   public class MoveableStub : IMoveable {
      public Vec Position { get; set; }
      public double BaseSpeed { get; set; }
 
      public MoveResult TryMoveTo(Vec pos) {
         return MoveResult.Success();
      }

      public Damage Collide(Vec pos) {
         throw new System.NotImplementedException();
      }
   }
}