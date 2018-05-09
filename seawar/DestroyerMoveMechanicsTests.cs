using System.Collections.Generic;
using NUnit.Framework;

namespace seawar {
   [TestFixture]
   public class DestroyerMoveMechanicsTests {
      [Test]
      public void CanMoveToEmptySea() {
         var mechanics = new DestroyerMoveMechanics();
         Assert.IsTrue(mechanics.CanOccupy(new Tile(0, new List<Actor>())));
      }
      [Test]
      public void CannotMoveToLand() {
         var mechanics = new DestroyerMoveMechanics();
         Assert.IsFalse(mechanics.CanOccupy(new Tile(1, new List<Actor>())));
      }
   }

   public class DestroyerMoveMechanics : IMoveMechanics {
      public bool CanOccupy(Tile tile) {
         return tile.IsWater;
      }
   }
}