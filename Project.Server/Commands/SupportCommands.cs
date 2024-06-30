using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Resources.Chat.Api;
using Project.Server.Factories;

namespace Project.Server.Commands
{
    internal class SupportCommands : IScript
    {
        [Command("tp")]
        public void Teleport(IAltPlayer player, int id = 0)
        {
            if (id > Misc.SpawnPositions.Length || id <= 0)
            {
                player.SendChatMessage(
                    $"{{FF0000}}Invalid Spawnpoint! (Minimum 1, Maximum: {Misc.SpawnPositions.Length})");
                return;
            }

            Position spawnpoint = Misc.SpawnPositions[id - 1];
            player.Position = spawnpoint + new Position(Misc.RandomInt(0, 10), Misc.RandomInt(0, 10), 0);
        }
    }
}
