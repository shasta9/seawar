namespace seawar
{
   public class Move {
      public Move(Vec vector, double distance, double speed) {
         Vector = vector;
         Distance = distance;
         Speed = speed;
      }

      public Vec Vector { get; }
      public double Distance { get; }
      public double Speed { get; }
   }
}