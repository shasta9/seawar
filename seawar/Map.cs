namespace seawar {
   public class Map {
      private readonly int[,] topo;

      public Map(int[,] topo) {
         this.topo = topo;
      }

      public int GetElevation(Vec pos) {
         return topo[pos.Y, pos.X];
      }

      public bool IsWater(Vec pos) => GetElevation(pos) <= 0;

      public bool IsLand(Vec pos) => !IsWater(pos);
   }
}