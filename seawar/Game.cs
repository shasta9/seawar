using System.Collections.Generic;
using System.Xml;
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
            var completeActions = new List<IAction>();
            foreach (var action in actor.GetActions()) {
               action.Perform(delta);
               if (action.IsComplete) {
                  completeActions.Add(action);
               }
            }

            foreach (var action in completeActions) {
               actor.RemoveAction(action);
            }
         }
      }
   }
}