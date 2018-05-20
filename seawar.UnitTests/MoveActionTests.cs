using NodaTime;
using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class MoveActionTests {
      private IMoveable moveable;

      [SetUp]
      public void SetUp() {
         moveable = new MoveableStub {
            Position = Vec.Zero,
            BaseSpeed = 1.0
         };
      }

      [Test]
      public void OthogonalMove() {
         var move = new Move(Direction.North, 1, 1.0);
         var action = new MoveAction(moveable, move);
         Assert.AreEqual(new Vec(0, 0), moveable.Position);
         action.Perform(Duration.FromSeconds(0.5));
         Assert.IsFalse(action.IsComplete);
         Assert.AreEqual(new Vec(0, 0), moveable.Position);
         action.Perform(Duration.FromSeconds(0.5));
         Assert.IsTrue(action.IsComplete);
         Assert.AreEqual(new Vec(0, 1), moveable.Position);
      }

      [Test]
      public void DiagonalMove() {
         var move = new Move(Direction.NorthEast, 1, 1.0);
         var action = new MoveAction(moveable, move);
         Assert.AreEqual(new Vec(0, 0), moveable.Position);
         action.Perform(Duration.FromSeconds(0.75));
         Assert.IsFalse(action.IsComplete);
         Assert.AreEqual(new Vec(0, 0), moveable.Position);
         action.Perform(Duration.FromSeconds(0.75));
         Assert.IsTrue(action.IsComplete);
         Assert.AreEqual(new Vec(1, 1), moveable.Position);
      }
   }
}