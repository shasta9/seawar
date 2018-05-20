using System.Collections.Generic;
using NodaTime;
using seawar.Messages;
using seawar.PlayerCommands;

namespace seawar.Game {
   public class Game {
      private readonly World world;
      private readonly List<Player> players;
      private readonly CommandDispatcher cmdDispatcher;

      public Game(World world) {
         this.world = world;
         players = new List<Player>();
         cmdDispatcher = new CommandDispatcher();
      }

      public void ConnectPlayer(Player player) {
         players.Add(player);
      }

      public void Update(Duration delta) {
         // get user commands
         foreach (var player in players) {
            var cmd = player.GetCommand();
            cmdDispatcher.Dispatch(cmd);
         }
         // update world
         world.Update(delta);

         // send world messages to players
         PlayerMessage msg;
         while ((msg = world.GetMessage()) != null) {
            foreach (var player in players) {
               player.SendMessage(msg);
            }
         }
      }
   }

   public class CommandDispatcher {
      public void Dispatch(PlayerCommand cmd) {
         throw new System.NotImplementedException();
      }
   }

   public class Player {
      public void SendMessage(PlayerMessage msg) {
         throw new System.NotImplementedException();
      }

      public PlayerCommand GetCommand() {
         throw new System.NotImplementedException();
      }
   }
}
