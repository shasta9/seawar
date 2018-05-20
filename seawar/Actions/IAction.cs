using NodaTime;

namespace seawar.Actions {
   public interface IAction {
      bool IsComplete { get; }
      void Perform(Duration delta);
   }
}