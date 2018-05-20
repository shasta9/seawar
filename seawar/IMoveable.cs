namespace seawar {
   public interface IMoveable {
      Vec Position { get; set; }
      double BaseSpeed { get; set; }
      bool CanMoveTo(Vec pos);
      Damage Collide(Vec pos);
   }
}