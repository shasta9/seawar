using NodaTime;
using seawar.Actions;

namespace seawar.UnitTests {
   public class ActionSpy : IAction {
      public bool IsComplete { get; private set; }
      public int PerformedCount { get; private set; }

      public void Perform(Duration delta) {
         PerformedCount++;
         IsComplete = true;
      }
   }
}