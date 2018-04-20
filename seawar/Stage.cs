namespace seawar {
   public class Stage {
      private readonly int[,] topo;

      public Stage(int[,] topo) {
         this.topo = topo;
      }

      public int GetElevation(Vec pos) {
         return topo[pos.Y, pos.X];
      }
   }
}