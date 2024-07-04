using AltV.Net.Data;
using Project.Server.Factories;

namespace Project.Server.Commands
{
    internal class SupportCommands : IController
    {
        public void OnStart()
        {
            CommandHandlers.Add("teleport", Teleport);
        }

        public void OnStop()
        {

        }

        public void Teleport(IAltPlayer player, string cmd, string[] args)
        {
            uint id = args.Length > 0 ? uint.Parse(args[0]) : 0;

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
