using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class StageTests {
      [Test]
      public void Elevation() {
         var topo = new[,] {
            { -4, -3, -2},
            { -1,  0,  1},
            {  2,  3,  4}
         };
         var stage = new Map(topo);
         Assert.AreEqual(-4, stage.GetElevation(new Vec(0, 0)));
         Assert.AreEqual(-3, stage.GetElevation(new Vec(1, 0)));
         Assert.AreEqual(-1, stage.GetElevation(new Vec(0, 1)));
         Assert.AreEqual(4, stage.GetElevation(new Vec(2, 2)));
      }

      [Test]
      public void IsWater() {
         var topo = new[,] {
            { -1,  0},
            {  1,  2}
         };
         var stage = new Map(topo);
         Assert.IsTrue(stage.IsWater(new Vec(0, 0)));
         Assert.IsTrue(stage.IsWater(new Vec(1, 0)));
         Assert.IsFalse(stage.IsWater(new Vec(0, 1)));
         Assert.IsFalse(stage.IsWater(new Vec(1, 1)));
      }

      [Test]
      public void IsLand() {
         var topo = new[,] {
            { -1,  0},
            {  1,  2}
         };
         var stage = new Map(topo);
         Assert.IsFalse(stage.IsLand(new Vec(0, 0)));
         Assert.IsFalse(stage.IsLand(new Vec(1, 0)));
         Assert.IsTrue(stage.IsLand(new Vec(0, 1)));
         Assert.IsTrue(stage.IsLand(new Vec(1, 1)));
      }
   }
}