namespace seawar {
   public interface IPhysics {
      bool CanOccupy(Tile tile);
      MoveResult Resolve(World world, Vec vec);
   }
}