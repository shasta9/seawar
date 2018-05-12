using System.Collections.Generic;

namespace seawar {
   public class Stage {
      private readonly List<Actor> actors = new List<Actor>();
      private readonly int[,] topo;

      public Stage(int[,] topo) {
         this.topo = topo;
      }

      public IEnumerable<Actor> Actors => actors;

      public void AddActor(Actor actor) {
         actors.Add(actor);
      }

      public void RemoveActor(Actor actor) {
         actors.Remove(actor);
      }

      private IEnumerable<Actor> GetActorsAt(Vec pos) {
         var actorsAtPos = new List<Actor>();
         foreach (var actor in actors) {
            if (actor.Position == pos) actorsAtPos.Add(actor);
         }
         return actorsAtPos;
      }

      public Tile GetTile(Vec pos) {
         return new Tile(topo[pos.Y, pos.X], GetActorsAt(pos));
      }
   }
}