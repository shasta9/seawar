using System.Linq;
using NUnit.Framework;
using seawar.Actors;
using seawar.Game;
using seawar.Vectors;

namespace seawar.UnitTests {
   [TestFixture]
   public class WorldTests {
      [Test]
      public void Elevation() {
         var topo = new[,] {
            { -4, -3, -2},
            { -1,  0,  1},
            {  2,  3,  4}
         };
         var world = new World(topo);
         Assert.AreEqual(-4, world.GetTile(new Vec(0, 0)).Elevation);
         Assert.AreEqual(-3, world.GetTile(new Vec(1, 0)).Elevation);
         Assert.AreEqual(-1, world.GetTile(new Vec(0, 1)).Elevation);
         Assert.AreEqual(4, world.GetTile(new Vec(2, 2)).Elevation);
      }

      [Test]
      public void IsWater() {
         var topo = new[,] {
            { -1,  0},
            {  1,  2}
         };
         var world = new World(topo);
         Assert.IsTrue(world.GetTile(new Vec(0, 0)).IsWater);
         Assert.IsFalse(world.GetTile(new Vec(0, 1)).IsWater);
         Assert.IsFalse(world.GetTile(new Vec(1, 1)).IsWater);
      }

      [Test]
      public void IsLand() {
         var topo = new[,] {
            { -1,  0},
            {  1,  2}
         };
         var world = new World(topo);
         Assert.IsFalse(world.GetTile(new Vec(0, 0)).IsLand);
         Assert.IsFalse(world.GetTile(new Vec(1, 0)).IsLand);
         Assert.IsTrue(world.GetTile(new Vec(0, 1)).IsLand);
         Assert.IsTrue(world.GetTile(new Vec(1, 1)).IsLand);
      }

      [Test]
      public void HasActors() {
         //var topo = new[,] {
         //   { -1,  0},
         //   {  1,  2}
         //};
         //var world = new World(topo);
         //var a = new Actor(null, null) { Position = new Vec(0, 0), Name = "A" };
         //var b = new Actor(null, null) { Position = new Vec(0, 0), Name = "B" };
         //world.AddActor(a);
         //world.AddActor(b);
         //Assert.AreEqual(2, world.GetTile(new Vec(0,0 )).Actors.Count());
         //world.RemoveActor(a);
         //Assert.AreEqual(1, world.GetTile(new Vec(0, 0)).Actors.Count());
         //Assert.AreSame(b, world.GetTile(new Vec(0, 0)).Actors.First());
      }
   }
}