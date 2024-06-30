using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Resources.Chat.Api;
using Project.Server.Factories;

namespace Project.Server.Commands
{
    internal class VehicleCommands : IScript
    {
        [Command("veh")]
        public void SpawnVeh(IAltPlayer player, string vehicleName)
        {
            // fails for addon vehicles

            //if (!Enum.IsDefined(typeof(VehicleModel), Alt.Hash(vehicleName)))
            //{
            //    player.SendChatMessage("{FF0000} Invalid vehicle model!");
            //    return;
            //}

            if (player.InteriorLocation != 0)
            {
                player.SendChatMessage("{FF0000} You can't spawn vehicles in interiors!");
                return;
            }

            //if (player.LastVehicleSpawn.AddSeconds(10) > DateTime.Now)
            //{
            //    player.SendChatMessage("{FF0000} You have to wait 10s before spawning a new vehicle!");
            //    return;
            //}

            if (player.Vehicles.Count >= 3)
            {
                AltVehicle target = player.Vehicles.OrderBy(veh => veh.SpawnTime).First();
                player.Vehicles.Remove(target);
                target.Destroy();
                player.SendChatMessage("{FF0000} You can't have more than 3 vehicles. We removed your oldest one!");
            }

            if (player.IsInVehicle)
            {
                player.SendChatMessage("{FF0000} You are already in a vehicle we replaced it for you!");
                player.Vehicle.Destroy();
            }

            AltVehicle spawnedVeh = (AltVehicle)Alt.CreateVehicle(Alt.Hash(vehicleName),
                player.Position + new Position(1, 0, 0), new Rotation(0, 0, player.Rotation.Yaw));
            player.SetIntoVehicle(spawnedVeh, 1);
            player.LastVehicleSpawn = DateTime.Now;
            player.Vehicles.Add(spawnedVeh);

            if (vehicleName == "karby")
                spawnedVeh.PrimaryColorRgb = new Rgba(251, 231, 239, 255);

            spawnedVeh.Owner = player;
        }

        [Command("vehdel")]
        public void DeleteVeh(IAltPlayer player)
        {
            if (!player.IsInVehicle)
            {
                player.SendChatMessage("{FF0000} You are not in a vehicle!");
                return;
            }

            AltVehicle veh = (AltVehicle)player.Vehicle;
            player.Vehicles.Remove(veh);
            veh.Destroy();
        }

        [Command("vehnuke")]
        public async void NukeVehicles(IAltPlayer player)
        {
            // get all players and convert them to IAltPlayer
            IEnumerable<IAltPlayer> players = Alt.GetAllPlayers().Select(p => (IAltPlayer)p);

            foreach (IAltPlayer p in players)
            {
                foreach (AltVehicle veh in p.Vehicles)
                {
                    veh.SetTimedExplosion(true, p, 0);
                }
            }

            await Task.Delay(5000);

            foreach (IAltPlayer p in players)
            {
                foreach (AltVehicle veh in p.Vehicles)
                {
                    veh.Destroy();
                }

                p.Vehicles.Clear();
            }
        }

        [Command("tune")]
        public void Tune(IAltPlayer player, int index, int value)
        {
            if (!player.IsInVehicle)
            {
                player.SendChatMessage("{FF0000} You're not in a vehicle!");
                return;
            }

            player.Vehicle.ModKit = 1;
            player.Vehicle.SetMod((byte)index, (byte)value);
        }
    }
}
