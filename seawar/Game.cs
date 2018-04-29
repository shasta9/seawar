using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class Game {
      private readonly List<Actor> actors = new List<Actor>();
      private readonly Map map;

      public Game(Map map) {
         this.map = map;
      }

      public void AddActor(Actor actor) {
         actors.Add(actor);
      }

      public Tile GetTile(Vec pos) {
         return new Tile(map.GetElevation(pos), GetActorsAt(pos));
      }

      public void Update(Duration delta) {
         foreach (var actor in actors) {
            var completeActions = new List<IAction>();
            foreach (var action in actor.GetActions()) {
               if (action.Perform(delta)) {
                  completeActions.Add(action);
               }
            }
            foreach (var action in completeActions) {
               actor.RemoveAction(action);
            }
         }
      }

      private IEnumerable<Actor> GetActorsAt(Vec pos) {
         var actorsAtPos = new List<Actor>();
         foreach (var actor in actors) {
            if (actor.Position == pos) actorsAtPos.Add(actor);
         }
         return actorsAtPos;
      }
   }
}