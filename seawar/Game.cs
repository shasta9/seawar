using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class Game {
      private readonly World world;

      public Game(World world) {
         this.world = world;
      }

      public Tile GetTile(Vec pos) {
         return world.GetTile(pos);
      }

      public void Update(Duration delta) {
         // process messages

         // perform actor actions
         world.Update(delta);
         // remove expired actors

      }
   }
}