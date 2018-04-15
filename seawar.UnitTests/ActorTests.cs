using NodaTime;
using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class ActorTests {
      [Test]
      public void Movement() {
         var game = new Game(null);
         var a = new Actor();
         game.AddActor(a);
         game.SetCommand(new StartMoveCommand(a, new Move(Direction.North, 1, 1)));
         Assert.AreEqual(new Vec(0, 0), a.Position);
         a.Update(Duration.FromSeconds(0.5));
         Assert.AreEqual(new Vec(0, 0), a.Position);
         a.Update(Duration.FromSeconds(0.5));
         Assert.AreEqual(new Vec(0, 1), a.Position);
         a.Update(Duration.FromSeconds(2.0));
         Assert.AreEqual(new Vec(0, 1), a.Position);
      }
   }

}
