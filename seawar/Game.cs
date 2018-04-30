using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class Game {
      private readonly Stage stage;

      public Game(Map map) {
         stage = new Stage(map);
      }

      public Tile GetTile(Vec pos) {
         return stage.GetTile(pos);
      }

      public void Update(Duration delta) {
         foreach (var actor in stage.Actors) {
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