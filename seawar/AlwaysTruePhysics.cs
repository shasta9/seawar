namespace seawar {
   public class AlwaysTruePhysics : IPhysics {
      public bool CanOccupy(Tile tile) {
         return true;
      }

      public MoveResult Resolve(World world, Vec vec) {
         throw new System.NotImplementedException();
      }
   }
}