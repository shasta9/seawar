namespace seawar.UnitTests {
   public class AlwaysTrueMoveMechanics : IMoveMechanics {
      public bool CanOccupy(Tile tile) {
         return true;
      }
   }
}