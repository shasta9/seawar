using NodaTime;

namespace seawar
{
   public abstract class Behaviour {
      protected Actor actor;

      protected Behaviour(Actor actor) {
         this.actor = actor;
      }

      public abstract void Update(Duration delta);
   }
}