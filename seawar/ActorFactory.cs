namespace seawar {
   public static class ActorFactory {
      public static Actor MakeMoveTestActor(World world) {
         return new Actor(world, new TestActorPhysics()) { BaseSpeed = 1.0 };
      }
   }
}