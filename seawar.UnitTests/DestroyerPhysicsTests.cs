using System.Collections.Generic;
using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class DestroyerPhysicsTests {
      [Test]
      public void CanMoveToEmptySea() {
         var mechanics = new DestroyerPhysics();
         Assert.IsTrue(mechanics.CanOccupy(new Tile(0, new List<Actor>())));
      }
      [Test]
      public void CannotMoveToLand() {
         var mechanics = new DestroyerPhysics();
         Assert.IsFalse(mechanics.CanOccupy(new Tile(1, new List<Actor>())));
      }
   }
}