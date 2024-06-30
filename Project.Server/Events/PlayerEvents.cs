using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Project.Server.Factories;

namespace Project.Server.Events
{
    internal class PlayerEvents : IScript
    {
        [AsyncScriptEvent(ScriptEventType.PlayerConnect)]
        public async Task PlayerConnectAsync(IAltPlayer player, string reason)
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

        [AsyncScriptEvent(ScriptEventType.PlayerDead)]
        public async Task PlayerDead(IAltPlayer player, IEntity killer, uint weapon)
        {
            if (killer is IVehicle)
            {
                Misc.SendChatMessageToAll($"{player.Name}({player.Id}) was killed by a vehicle...");
            }
            else if (killer is IAltPlayer)
            {
                IAltPlayer killerPlayer = (IAltPlayer)killer;

                if (Enum.IsDefined(typeof(WeaponModel), weapon))
                {
                    Misc.SendChatMessageToAll($"{player.Name}({player.Id}) was killed by {killerPlayer.Name}({killerPlayer.Id}) using a {Enum.GetName(typeof(WeaponModel), weapon)}");
                }
                else
                {
                    Misc.SendChatMessageToAll($"{player.Name}({player.Id}) was killed by {killerPlayer.Name}({killerPlayer.Id})");
                }

                await Task.Delay(5000);

                player.Spawn(player.Position + new Position(Misc.RandomInt(10, 30), Misc.RandomInt(10, 30), 0));
            }
            else
            {
                Misc.SendChatMessageToAll($"{player.Name}({player.Id}) died...");

                Position[] spawnPoints = Misc.SpawnPositions;
                Position rndSpawnPoint = spawnPoints.ElementAt(Misc.RandomInt(0, spawnPoints.Length));
                player.Spawn(rndSpawnPoint + new Position(Misc.RandomInt(0, 10), Misc.RandomInt(0, 10), 0));
            }
        }
    }
}