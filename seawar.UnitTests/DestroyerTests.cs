using NodaTime;
using NUnit.Framework;
using seawar.Actions;
using seawar.Actors;
using seawar.Game;
using seawar.Vectors;

namespace seawar.UnitTests {
   [TestFixture]
   public class DestroyerTests {
      private DestroyerEx dest;

      [SetUp]
      public void SetUp() {
         var topo = new[,] {
            { -1, -1},
            { 0, -1}
         };
         var world = new World(topo);
         dest = new DestroyerEx(world) {
            Position = Vec.Zero,
            BaseSpeed = 1.0
         };
      }

      [Test]
      public void OrthogonalMove() {
         var move = new Move(Direction.North, 1, 1.0);
         dest.StartMove(move);
         Assert.AreEqual(new Vec(0, 0), dest.Position);
         dest.Update(Duration.FromSeconds(0.5));
         Assert.AreEqual(new Vec(0, 0), dest.Position);
         dest.Update(Duration.FromSeconds(0.5));
         Assert.AreEqual(new Vec(0, 1), dest.Position);
         dest.Update(Duration.FromSeconds(1.0));
         Assert.AreEqual(new Vec(0, 1), dest.Position);
      }

      [Test]
      public void DiagonalMove() {
         var move = new Move(Direction.NorthEast, 1, 1.0);
         dest.StartMove(move);
         Assert.AreEqual(new Vec(0, 0), dest.Position);
         dest.Update(Duration.FromSeconds(0.75));
         Assert.AreEqual(new Vec(0, 0), dest.Position);
         dest.Update(Duration.FromSeconds(0.75));
         Assert.AreEqual(new Vec(1, 1), dest.Position);
         dest.Update(Duration.FromSeconds(2.0));
         Assert.AreEqual(new Vec(1, 1), dest.Position);
      }
   }
}