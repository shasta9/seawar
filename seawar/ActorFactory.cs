namespace seawar {
   public static class ActorFactory {
      public static Actor MakeMoveTestActor(Game game) {
         return new Actor(game,new AlwaysTrueMoveMechanics()){BaseSpeed = 1.0};
      }
   }
}