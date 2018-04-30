using System.Collections.Generic;

namespace seawar {
   public class Actor {

      private readonly Game game;
      private readonly List<IAction> actions = new List<IAction>();
      private readonly IMoveMechanics moveMechanics;

      public Actor(Game game, IMoveMechanics moveMechanics) {
         this.game = game;
         this.moveMechanics = moveMechanics;
      }

      public string Name { get; set; } = string.Empty;
      public Vec Position { get; set; } = new Vec();
      public double BaseSpeed { get; set; }

      public void AddAction(IAction action) {
         actions.Add(action);
      }

      public void RemoveAction(IAction action) {
         actions.Remove(action);
      }

      public IEnumerable<IAction> GetActions() {
         return actions;
      }

      public bool CanOccupy(Vec vec) {
         return moveMechanics.CanOccupy(game.GetTile(Position + vec));
      }

      public void MoveBy(Vec vec) {
         Position = Position + vec;
      }
   }

}