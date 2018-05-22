using System.Collections.Generic;
using seawar.Actors;

namespace seawar.Game {
   public class Tile {
      public Tile(int elevation, IEnumerable<Actor> actors) {
         Elevation = elevation;
         Actors.AddRange(actors);
      }

      public int Elevation { get; }
      public List<Actor> Actors { get; } = new List<Actor>();
      public bool IsWater => Elevation <= 0;
      public bool IsLand => !IsWater;
      public bool IsDestroyerPort { get; set; }
      public bool IsSubmarinePort { get; set; }
   }
}