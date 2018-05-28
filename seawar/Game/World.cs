using System.Collections.Generic;
using NodaTime;
using seawar.Actors;
using seawar.Messages;
using seawar.Vectors;

namespace seawar.Game {
   public class World {
      private readonly int[,] topo;
      private List<Actor> Actors { get; } = new List<Actor>();

      public World(int[,] topo) {
         this.topo = topo;
      }

      public void AddActor(Actor actor) {
         Actors.Add(actor);
      }

      public void RemoveActor(Actor actor) {
         Actors.Remove(actor);
      }

      public Tile GetTile(Vec pos) {
         return new Tile(topo[pos.Y, pos.X], GetActorsAt(pos));
      }

      public void Update(Duration delta) {
         var expiredActors = new List<Actor>();
         foreach (var actor in Actors) {
            actor.Update(delta);
            if (actor.HasExpired) expiredActors.Add(actor);
         }
         foreach (var actor in expiredActors) {
            Actors.Remove(actor);
         }
      }

      private IEnumerable<Actor> GetActorsAt(Vec pos) {
         var actorsAtPos = new List<Actor>();
         foreach (var actor in Actors) {
            if (actor.Position == pos) actorsAtPos.Add(actor);
         }
         return actorsAtPos;
      }

      public PlayerMessage GetMessage() {
         throw new System.NotImplementedException();
      }
   }

   public class Actor {
      public bool HasExpired { get; set; }
      public Vec Position { get; set; }

      public void Update(Duration delta) {
         throw new System.NotImplementedException();
      }
   }
}
