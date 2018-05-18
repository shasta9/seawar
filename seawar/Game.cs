using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Build.Tasks;
using NodaTime;

namespace seawar {
   public class Game {
      private readonly World world;
      private List<Player> players;
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

         // send messages
         PlayerMessage msg;
         while ((msg = world.GetMessage()) != null) {
            foreach (var player in players) {
               player.SendMessage(msg);
            }
         }
      }
   }

   public class PlayerMessage { }

   internal class CommandDispatcher {
      public void Dispatch(PlayerCommand cmd) {
         throw new System.NotImplementedException();
      }
   }

   public class Player {
      public void SendMessage(PlayerMessage msg) {

      }

      public PlayerCommand GetCommand() {
         throw new System.NotImplementedException();
      }
   }

   public class PlayerCommand { }
}
