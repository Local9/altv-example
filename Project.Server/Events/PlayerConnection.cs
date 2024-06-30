using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using Project.Server.Factories;

namespace Project.Server.Events
{
    internal class PlayerConnection : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public async void PlayerConnectAsync(IAltPlayer player, string reason)
        {
            Position rndSpawnPoint = Misc.SpawnPositions.ElementAt(Misc.RandomInt(0, Misc.SpawnPositions.Length));
            player.Spawn(rndSpawnPoint + new Position(Misc.RandomInt(0, 10), Misc.RandomInt(0, 10), 0));
            player.Model = (uint)PedModel.FreemodeMale01;
            player.SetDateTime(DateTime.UtcNow);
            player.SetWeather(Misc.Weather);

            player.RefreshFace();
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void PlayerDisconnect(IAltPlayer player, string reason)
        {
            IList<AltVehicle> vehicles = player.Vehicles;
            foreach (AltVehicle vehicle in vehicles)
            {
                vehicle.Destroy();
            }
        }

        [ScriptEvent(ScriptEventType.PlayerDead)]
        public async void PlayerDead(IAltPlayer player, IEntity killer, uint weapon)
        {
            if (killer is IAltPlayer)
            {
                IAltPlayer killerPlayer = (IAltPlayer)killer;

                foreach (IPlayer? p in Alt.GetAllPlayers())
                {
                    p.SendChatMessage($"{("{FFFFFF}")} <b>{killerPlayer.Name}({killerPlayer.Id})</b> killed <b>{player.Name}({player.Id})</b>");
                }

                await Task.Delay(5000);

                player.Spawn(player.Position + new Position(Misc.RandomInt(10, 30), Misc.RandomInt(10, 30), 0));

                return;
            }

            Position[] spawnPoints = Misc.SpawnPositions;
            Position rndSpawnPoint = spawnPoints.ElementAt(Misc.RandomInt(0, spawnPoints.Length));
            player.Spawn(rndSpawnPoint + new Position(Misc.RandomInt(0, 10), Misc.RandomInt(0, 10), 0));
        }
    }
}