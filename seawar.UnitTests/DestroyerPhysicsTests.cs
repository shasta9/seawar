using System.Collections.Generic;
using NUnit.Framework;
using seawar.Actors;
using seawar.Game;
using seawar.Physics;

namespace seawar.UnitTests {
   [TestFixture]
   public class DestroyerPhysicsTests {
      [Test]
      public void CanMoveToEmptySea() {
         var physics = new DestroyerPhysics();
         Assert.AreEqual(MoveResults.Ok, physics.TryMoveTo(new Tile(0, new List<Actor>())).Result);
      }

      [Test]
      public void CannotMoveToLand() {
         var physics = new DestroyerPhysics();
         Assert.AreEqual(MoveResults.Aground, physics.TryMoveTo(new Tile(1, new List<Actor>())).Result);
      }
   }
}