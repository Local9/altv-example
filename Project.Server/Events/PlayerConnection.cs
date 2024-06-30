using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Project.Server.Factories;
using Project.Server.Services;

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

            if (Misc.IsResourceLoaded("c_clothesfit"))
            {
                await ClothesFitService.InitPlayer(player);
            }

            player.RefreshFace();
            await player.RefreshClothes();
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void PlayerDisconnect(IAltPlayer player, string reason)
        {
            IList<AltVehicle> vehicles = player.Vehicles;
            foreach (AltVehicle vehicle in vehicles)
            {
                vehicle.Destroy();
            }

            if (Misc.IsResourceLoaded("c_clothesfit"))
            {
                ClothesFitService.DestroyPlayer(player);
            }
        }

        [ScriptEvent(ScriptEventType.PlayerDead)]
        public void PlayerDead(IPlayer player, IEntity killer, uint weapon)
        {
            Console.WriteLine($"{player.Name} died");
            player.Spawn(player.Position);
        }
    }
}