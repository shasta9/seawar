using NodaTime;

namespace seawar {
   public interface IAction {
      bool Perform(Duration delta);
   }
}