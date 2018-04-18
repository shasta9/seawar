using System;
using NodaTime;

namespace seawar {
   public class MoveBehaviour : Behaviour {
      private readonly Move move;
      private double distance;
      private double totalDistance;

      public MoveBehaviour(Actor actor, Move move) : base(actor) {
         this.move = move;
      }

      public override void Update(Duration delta) {
      }
   }

   public static class Commands {
      public static Action StartMove(Actor actor, Move move) {
         return () => actor.StartMove(move);
      }
   }
}