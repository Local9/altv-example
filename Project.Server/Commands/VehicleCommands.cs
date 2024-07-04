using AltV.Net.Data;
using Project.Server.Factories;

namespace Project.Server.Commands
{
    internal class VehicleCommands : IController
    {
        public void OnStart()
        {
            CommandHandlers.Add("veh", SpawnVeh);
            CommandHandlers.Add("vehdel", DeleteVeh);
            CommandHandlers.Add("vehnuke", NukeVehicles);
            CommandHandlers.Add("tune", Tune);
        }

        public void OnStop()
        {
            throw new NotImplementedException();
        }

        public void SpawnVeh(IAltPlayer player, string cmd, string[] args)
        {
            // fails for addon vehicles

            //if (!Enum.IsDefined(typeof(VehicleModel), Alt.Hash(vehicleName)))
            //{
            //    player.SendChatMessage("{FF0000} Invalid vehicle model!");
            //    return;
            //}

            string vehicleName = args[0];

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

        public void DeleteVeh(IAltPlayer player, string cmd, string[] args)
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

        public async void NukeVehicles(IAltPlayer player, string cmd, string[] args)
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

        public void Tune(IAltPlayer player, string cmd, string[] args)
        {
            int index = int.Parse(args[0]);
            int value = int.Parse(args[1]);

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
