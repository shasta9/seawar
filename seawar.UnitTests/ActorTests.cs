using NodaTime;
using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class ActorTests {
      [Test]
      public void Movement() {
         var a = new Actor();
         a.StartMove(new Move(Direction.North, 1, 1.0));
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
         a.StartMove(new Move(Direction.NorthEast, 1, 1.0));
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


      [Test]
      public void StopsAtShore() {
         var topo = new[,] {
            {-100, -100,   10},
            {-100, -100,   10},
            {  10,   10,   10}
         };
         var stage = new Stage(topo);
         var game = new Game(stage);
         var actor = new Actor();
         game.AddActor(actor);
         actor.StartMove(new Move(Direction.North, 3, 1));
         for (int i = 0; i < 30; i++) {
            game.Update(Duration.FromMilliseconds(100));
         }
         Assert.AreEqual(new Vec(0, 1), actor.Position);
      }
   }
}
