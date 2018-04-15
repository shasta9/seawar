using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class Actor {
      private readonly List<Behaviour> behaviours = new List<Behaviour>();

      public Vec Position { get; set; } = new Vec(0, 0);
      public double BaseSpeed { get; set; } = 1.0;

      public void Update(Duration delta) {
         foreach (var behaviour in behaviours) {
            behaviour.Update(delta);
         }
      }

      public void StartMove(Move move) {
         behaviours.Add(new MoveBehaviour(this, move));
      }

      public void StopMove(MoveBehaviour move) {
         behaviours.Remove(move);
      }
   }
}