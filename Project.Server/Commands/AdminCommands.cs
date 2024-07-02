using AltV.Net;
using AltV.Net.Resources.Chat.Api;
using Project.Server.Factories;
using Project.Shared;

namespace Project.Server.Commands
{
    internal class AdminCommands : IScript
    {
        [Command("noclip")]
        public void NoClip(IAltPlayer player)
        {
            // todo: check if player is admin

            Console.WriteLine($"3^NoClip command called by {player.Name}");

            player.NoClip = !player.NoClip;
            player.Streamed = !player.NoClip;
            player.Visible = !player.NoClip;
            player.Collision = !player.NoClip;
            player.ClearTasks();

            player.SendChatMessage($"{{00FF00}}NoClip is now {(player.NoClip ? "enabled" : "disabled")}!");

            player.Emit(AdminEvents.NOCLIP, player.NoClip);
        }
    }
}
