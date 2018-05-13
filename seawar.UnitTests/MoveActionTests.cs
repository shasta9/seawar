using NodaTime;
using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class MoveActionTests {
      [Test]
      public void OthogonalMove() {
         var mechanics = new AlwaysTruePhysics();
         var actor = new Actor(null, mechanics) { BaseSpeed = 1.0 };
         var move = new Move(Direction.North, 1, 1.0);
         var action = new MoveAction(actor, move);
         Assert.AreEqual(new Vec(0, 0), actor.Position);
         action.Perform(Duration.FromSeconds(0.5));
         Assert.IsFalse(action.IsComplete);
         Assert.AreEqual(new Vec(0, 0), actor.Position);
         action.Perform(Duration.FromSeconds(0.5));
         Assert.IsTrue(action.IsComplete);
         Assert.AreEqual(new Vec(0, 1), actor.Position);
      }

      [Test]
      public void DiagonalMove() {
         var mechanics = new AlwaysTruePhysics();
         var actor = new Actor(null, mechanics) { BaseSpeed = 1.0 };
         var move = new Move(Direction.NorthEast, 1, 1.0);
         var action = new MoveAction(actor, move);
         Assert.AreEqual(new Vec(0, 0), actor.Position);
         action.Perform(Duration.FromSeconds(0.75));
         Assert.IsFalse(action.IsComplete);
         Assert.AreEqual(new Vec(0, 0), actor.Position);
         action.Perform(Duration.FromSeconds(0.75));
         Assert.IsTrue(action.IsComplete);
         Assert.AreEqual(new Vec(1, 1), actor.Position);
      }
   }
}