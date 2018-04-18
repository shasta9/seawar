using NodaTime;
using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class ActorTests {
      [Test]
      public void Movement() {
         var a = new Actor();
         a.StartMove(new Move(Direction.North, 1.0, 1.0));
         Assert.AreEqual(new Vec(0, 0), a.Position);
         a.Update(Duration.FromSeconds(0.5));
         Assert.AreEqual(new Vec(0, 0), a.Position);
         a.Update(Duration.FromSeconds(0.5));
         Assert.AreEqual(new Vec(0, 1), a.Position);
         a.Update(Duration.FromSeconds(2.0));
         Assert.AreEqual(new Vec(0, 1), a.Position);
      }

      [Test]
      public void DiagonalMove() {
         var a = new Actor();
         a.StartMove(new Move(Direction.NorthEast, 1.5, 1.0));
         Assert.AreEqual(new Vec(0, 0), a.Position);
         a.Update(Duration.FromSeconds(1.0));
         Assert.AreEqual(new Vec(0, 0), a.Position);
         a.Update(Duration.FromSeconds(0.5));
         Assert.AreEqual(new Vec(1, 1), a.Position);
         a.Update(Duration.FromSeconds(0.5));
         Assert.AreEqual(new Vec(1, 1), a.Position);
         a.Update(Duration.FromSeconds(1.0));
         Assert.AreEqual(new Vec(1, 1), a.Position);
      }
   }
}
