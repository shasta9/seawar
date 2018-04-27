using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class Game {
      private readonly List<Actor> actors = new List<Actor>();

      public Game(Stage stage) {
         Stage = stage;
      }

      public Stage Stage { get; }

      public void AddActor(Actor actor) {
         actors.Add(actor);
      }

      public void Update(Duration delta) {
         foreach (var actor in actors) {
            var action =actor.GetNextAction();
            action.Perform(delta);
            if (action.IsComplete) {
               //delete action
            }
         }
      }
   }
}