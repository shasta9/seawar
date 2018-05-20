namespace seawar {
   public static class ActorFactory {
      public static Actor MakeMoveTestActor(World world) {
         return new Actor(world, new MoveAnywherePhysics()) { BaseSpeed = 1.0 };
      }
   }
}