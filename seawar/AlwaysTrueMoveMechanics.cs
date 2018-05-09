namespace seawar {
   public class AlwaysTrueMoveMechanics : IMoveMechanics {
      public bool CanOccupy(Tile tile) {
         return true;
      }
   }
}