using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class World {
      private readonly int[,] topo;

      public World(int[,] topo) {
         this.topo = topo;
      }

      public List<Actor> Actors { get; } = new List<Actor>();

      public void AddActor(Actor actor) {
         Actors.Add(actor);
      }

      public void RemoveActor(Actor actor) {
         Actors.Remove(actor);
      }

      private IEnumerable<Actor> GetActorsAt(Vec pos) {
         var actorsAtPos = new List<Actor>();
         foreach (var actor in Actors) {
            if (actor.Position == pos) actorsAtPos.Add(actor);
         }
         return actorsAtPos;
      }

      public Tile GetTile(Vec pos) {
         return new Tile(topo[pos.Y, pos.X], GetActorsAt(pos));
      }

      public void Update(Duration delta) {
         foreach (var actor in Actors) {
            actor.Update(delta);
         }
      }
   }
}