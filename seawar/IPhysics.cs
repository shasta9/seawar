namespace seawar {
   public interface IPhysics {
      bool CanMoveTo(Tile pos);
      Damage Collide(Tile pos);
   }
}
