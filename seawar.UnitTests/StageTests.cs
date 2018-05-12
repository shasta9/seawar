using System.Linq;
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
         var stage = new Stage(topo);
         Assert.AreEqual(-4, stage.GetTile(new Vec(0, 0)).Elevation);
         Assert.AreEqual(-3, stage.GetTile(new Vec(1, 0)).Elevation);
         Assert.AreEqual(-1, stage.GetTile(new Vec(0, 1)).Elevation);
         Assert.AreEqual(4, stage.GetTile(new Vec(2, 2)).Elevation);
      }

      [Test]
      public void IsWater() {
         var topo = new[,] {
            { -1,  0},
            {  1,  2}
         };
         var stage = new Stage(topo);
         Assert.IsTrue(stage.GetTile(new Vec(0, 0)).IsWater);
         Assert.IsFalse(stage.GetTile(new Vec(0, 1)).IsWater);
         Assert.IsFalse(stage.GetTile(new Vec(1, 1)).IsWater);
      }

      [Test]
      public void IsLand() {
         var topo = new[,] {
            { -1,  0},
            {  1,  2}
         };
         var stage = new Stage(topo);
         Assert.IsFalse(stage.GetTile(new Vec(0, 0)).IsLand);
         Assert.IsFalse(stage.GetTile(new Vec(1, 0)).IsLand);
         Assert.IsTrue(stage.GetTile(new Vec(0, 1)).IsLand);
         Assert.IsTrue(stage.GetTile(new Vec(1, 1)).IsLand);
      }

      [Test]
      public void HasActors() {
         var topo = new[,] {
            { -1,  0},
            {  1,  2}
         };
         var stage = new Stage(topo);
         var a = new Actor(null, null) { Position = new Vec(0, 0), Name = "A" };
         var b = new Actor(null, null) { Position = new Vec(0, 0), Name = "B" };
         stage.AddActor(a);
         stage.AddActor(b);
         Assert.AreEqual(2, stage.GetTile(new Vec(0,0 )).Actors.Count());
         stage.RemoveActor(a);
         Assert.AreEqual(1, stage.GetTile(new Vec(0, 0)).Actors.Count());
         Assert.AreSame(b, stage.GetTile(new Vec(0, 0)).Actors.First());
      }
   }
}