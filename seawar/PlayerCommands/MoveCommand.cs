using seawar.Vectors;

namespace seawar.PlayerCommands {
   public class MoveCommand : PlayerCommand {
      public Vec Bearing { get; set; }
      public int Distance { get; set; }
      public double Speed { get; set; }
   }
}