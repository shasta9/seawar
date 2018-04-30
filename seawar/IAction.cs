using NodaTime;

namespace seawar {
   public interface IAction {
      bool IsComplete { get; }
      void Perform(Duration delta);
   }
}