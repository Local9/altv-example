using AltV.Net.Resources.Chat.Api;
using Project.Server.Factories;
using Project.Shared;

namespace Project.Server.Commands
{
    internal class AdminCommands
    {
        [Command("noclip")]
        public void NoClip(IAltPlayer player)
        {
            Console.WriteLine($"3^NoClip command called by {player.Name}");
            player.SendChatMessage($"{{00FF00}} No clip command triggered");

            player.Emit(AdminEvents.NOCLIP);
        }
    }
}
