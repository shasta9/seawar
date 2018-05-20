﻿namespace seawar.UnitTests {
   public class MoveableStub : IMoveable {
      public Vec Position { get; set; }
      public double BaseSpeed { get; set; }
 
      public bool CanMoveTo(Vec vec) {
         return true;
      }

      public Damage Collide(Vec pos) {
         throw new System.NotImplementedException();
      }
   }
}