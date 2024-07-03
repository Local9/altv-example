using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using Project.Server.Factories;

namespace Project.Server
{
    internal class ProjectMain : AsyncResource
    {
        public static IServiceProvider ServiceCollection;

        public override void OnStart()
        {
            Console.WriteLine("Server Resource started");

            ServiceCollection = new Startup().ConfigureServices();

            IRpcService rpcService = ServiceCollection.GetService<IRpcService>();
            rpcService.OnStart();

            foreach (IController controller in ServiceCollection.GetServices<IController>())
            {
                controller.OnStart();
            }
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
