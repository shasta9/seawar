namespace seawar {
   public class Map {
      public int[,] Depth { get; } = new int[100, 100];

   }

   public class Tile {
      public int Depth { get; set; }
   }
}