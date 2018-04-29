using System.Collections.Generic;

namespace seawar {
   public class Tile {
      private readonly List<Actor> actors = new List<Actor>();

      public Tile(int elevation, IEnumerable<Actor> actors) {
         Elevation = elevation;
         this.actors.AddRange(actors);
      }

      public int Elevation { get; }
      public bool IsWater => Elevation <= 0;
      public bool Island => !IsWater;
      public IEnumerable<Actor> Actors => actors;
   }
}