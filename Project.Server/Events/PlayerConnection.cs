using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
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
        public void PlayerDead(IPlayer player, IEntity killer, uint weapon)
        {
            Position[] spawnPoints = Misc.SpawnPositions;
            Position rndSpawnPoint = spawnPoints.ElementAt(Misc.RandomInt(0, spawnPoints.Length));
            player.Spawn(rndSpawnPoint + new Position(Misc.RandomInt(0, 10), Misc.RandomInt(0, 10), 0));
        }
    }
}