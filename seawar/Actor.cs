using NodaTime;

namespace seawar {
   public class Actor {
      public Vec Position { get; set; } = new Vec(0, 0);
      public double BaseSpeed { get; set; } = 1.0;
      public Behaviour Behaviour { get; set; } = null;

      public void Update(Duration delta) {
         Behaviour?.Update(delta);
      }
   }
}