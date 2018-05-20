using System.Collections.Generic;
using seawar.Actors;

namespace seawar.Game {
   public class Tile {
      private readonly List<Actor> actors = new List<Actor>();

      public Tile(int elevation, IEnumerable<Actor> actors) {
         Elevation = elevation;
         this.actors.AddRange(actors);
      }

      public int Elevation { get; }
      public bool IsWater => Elevation <= 0;
      public bool IsLand => !IsWater;
      public bool IsDestroyerPort { get; set; }
      public bool IsSubmarinePort { get; set; }
      public IEnumerable<Actor> Actors => actors;
   }
}