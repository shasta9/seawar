using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class ActorTests {
      [Test]
      public void Creation() {
         var a = new Actor(null, null);
         Assert.AreEqual(string.Empty, a.Name);
         var b = new Actor(null, null) { Name = "B" };
         Assert.AreEqual("B", b.Name);
      }

      [Test]
      public void ChangesDepth() {
         Assert.Fail("Not implemented yet");
      }

      [Test]
      public void StopsIfTooShallow() {
         Assert.Fail("Not implemented yet");
      }

      [Test]
      public void StopsAtShore() {
         var topo = new[,] {
            {-1, -1,  0},
            {-1, -1,  0},
            { 0,  0,  0}
         };
         var stage = new World(topo);
         var actor = new Actor(null, null);
         stage.AddActor(actor);
         ////actor.StartMove(new Move(Direction.North, 3, 1));
         //for (int i = 0; i < 30; i++) {
         //   actor.Update(Duration.FromMilliseconds(100));
         //}
         Assert.AreEqual(new Vec(0, 1), actor.Position);
      }

      [Test]
      public void CollidesWithActor() {
         Assert.Fail("Not implemented yet");
      }
   }
}
