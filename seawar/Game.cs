using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class Game {
      private readonly Map map;
      private readonly List<Actor> actors = new List<Actor>();

      public Game(Map map) {
         this.map = map;
      }

      public void AddActor(Actor actor) {
         actors.Add(actor);
      }

      public void Update(Duration delta) {
         foreach (var actor in actors) {
            actor.Update(delta);
         }
      }
   }
}