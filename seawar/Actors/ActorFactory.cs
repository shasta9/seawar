using seawar.Game;
using seawar.Physics;

namespace seawar.Actors {
   public static class ActorFactory {
      public static Actor MakeDestroyer(World world) {
         return new Actor(world, new DestroyerPhysics()) { BaseSpeed = 1.0 };
      }
   }
}