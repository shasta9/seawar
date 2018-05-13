namespace seawar {
   public class DestroyerPhysics : IPhysics {
      public bool CanOccupy(Tile tile) {
         return tile.IsWater;
      }

      public MoveResult Resolve(World world, Vec vec) {
         throw new System.NotImplementedException();
      }
   }
}