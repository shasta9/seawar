using System.Collections.Generic;

namespace seawar {
   public class Map {

      // TODO probably redundant, use stage instead

      private readonly int[,] topo;

      public Map(int[,] topo) {
         this.topo = topo;
      }

      public int GetElevation(Vec pos) {
         return topo[pos.Y, pos.X];
      }

      public bool IsWater(Vec pos) => GetElevation(pos) <= 0;

      public bool IsLand(Vec pos) => !IsWater(pos);
   }


   public class Stage {
      private readonly List<Actor> actors = new List<Actor>();
      private readonly Map map;

      public Stage(Map map) {
         this.map = map;
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
         return new Tile(map.GetElevation(pos), GetActorsAt(pos));
      }
   }
}