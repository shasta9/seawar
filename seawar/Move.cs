namespace seawar
{
   public class Move {
      public Move(Vec vector, int distance, double speed) {
         Vector = vector;
         Distance = distance;
         Speed = speed;
      }

      public Vec Vector { get; }
      public int Distance { get; }
      public double Speed { get; }
   }
}