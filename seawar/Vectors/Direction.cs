namespace seawar.Vectors {
   public static class Direction {
      public static readonly Vec North = new Vec(0, 1);
      public static readonly Vec NorthEast = new Vec(1, 1);
      public static readonly Vec East = new Vec(1, 0);
      public static readonly Vec SouthEast = new Vec(1, -1);
      public static readonly Vec South = new Vec(0, -1);
      public static readonly Vec SouthWest = new Vec(-1, -1);
      public static readonly Vec West = new Vec(-1, 0);
      public static readonly Vec NorthWest = new Vec(-1, 1);
   }
}