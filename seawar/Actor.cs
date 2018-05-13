using System.Collections.Generic;
using NodaTime;

namespace seawar {
   public class Actor {

      private readonly List<IAction> actions = new List<IAction>();
      private readonly World world;
      private readonly IPhysics physics;

      public Actor(World world, IPhysics physics) {
         this.world = world;
         this.physics = physics;
      }

      public string Name { get; set; } = string.Empty;
      public Vec Position { get; set; } = new Vec();
      public double BaseSpeed { get; set; }
      public bool HasExpired { get; set; }

      public void AddAction(IAction action) {
         actions.Add(action);
      }

      public void RemoveAction(IAction action) {
         actions.Remove(action);
      }

      public void Update(Duration delta) {
         var completeActions = new List<IAction>();
         foreach (var action in actions) {
            action.Perform(delta);
            if (action.IsComplete) {
               completeActions.Add(action);
            }
         }
         foreach (var action in completeActions) {
            RemoveAction(action);
         }
      }

      public void MoveBy(Vec vec) {
         Position = Position + vec;
      }

      public MoveResult TryMoveBy(Vec vec) {
         return physics.Resolve(world, vec);
      }
   }
}