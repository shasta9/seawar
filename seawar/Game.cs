using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class Game {
      private readonly Stage stage;
      private readonly List<Actor> actors = new List<Actor>();

      public Game(Stage stage) {
         this.stage = stage;
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