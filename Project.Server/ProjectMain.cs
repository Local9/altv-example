using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Project.Server.Factories;

namespace Project.Server
{
    internal class ProjectMain : AsyncResource
    {
        public ProjectMain() : base(true)
        {
        }

        public override void OnStart()
        {
            Console.WriteLine("Server Resource started");
        }

        public override void OnStop()
        {
            Console.WriteLine("Server Resource stopped");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new AltPlayerFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new AltVehicleFactory();
        }
    }
}
