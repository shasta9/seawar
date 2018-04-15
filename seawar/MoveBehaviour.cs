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
         var distanceInc = delta.TotalSeconds * move.Speed * actor.BaseSpeed;
         distance += distanceInc;
         totalDistance += distanceInc;
         if (distance > move.Vector.Length) {
            // actor is about to move
            // check that the destination tile is water
            // check what else might be at this position
            // move
            actor.Position += move.Vector;
            distance -= move.Vector.Length;
         }
         if (totalDistance >= move.Distance) ;
      }
   }

   public interface ICommand {
      void Execute();
   }

   public class StartMoveCommand : ICommand {
      private readonly Actor actor;
      private readonly Move move;

      public StartMoveCommand(Actor actor, Move move) {
         this.actor = actor;
         this.move = move;
      }

      public void Execute() {
         throw new System.NotImplementedException();
      }
   }

   public class StopMoveCommand : ICommand {
      private readonly Actor actor;
      private readonly MoveBehaviour move;

      public StopMoveCommand(Actor actor, MoveBehaviour move) {
         this.actor = actor;
         this.move = move;
      }
      public void Execute() {
         actor.StopMove(move);
      }
   }
}