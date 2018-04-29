using NodaTime;
using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class MoveActionTests {
      [Test]
      public void OthogonalMove() {
         var mechanics = new AlwaysTrueMoveMechanics();
         var actor = new Actor(null, mechanics) { BaseSpeed = 1.0 };
         var move = new Move(Direction.North, 1, 1.0);
         var action = new MoveAction(actor, move);
         Assert.AreEqual(new Vec(0, 0), actor.Position);
         Assert.IsFalse(action.Perform(Duration.FromSeconds(0.5)));
         Assert.AreEqual(new Vec(0, 0), actor.Position);
         Assert.IsTrue(action.Perform(Duration.FromSeconds(0.5)));
         Assert.AreEqual(new Vec(0, 1), actor.Position);
      }

      [Test]
      public void DiagonalMove() {
         var mechanics = new AlwaysTrueMoveMechanics();
         var actor = new Actor(null, mechanics) { BaseSpeed = 1.0 };
         var move = new Move(Direction.NorthEast, 1, 1.0);
         var action = new MoveAction(actor, move);
         Assert.AreEqual(new Vec(0, 0), actor.Position);
         Assert.IsFalse(action.Perform(Duration.FromSeconds(0.75)));
         Assert.AreEqual(new Vec(0, 0), actor.Position);
         Assert.IsTrue(action.Perform(Duration.FromSeconds(0.75)));
         Assert.AreEqual(new Vec(1, 1), actor.Position);
      }
   }
}