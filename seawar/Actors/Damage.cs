namespace seawar.Actors {

   public enum DamageType {
      Collision,
      Shell,
      Torpedo,
      Mine
   }


   public class Damage {
      public DamageType Type { get; set; }
      public Actor Target { get; set; }
      public int Amount { get; set; }
   }
}