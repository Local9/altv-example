using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.DependencyInjection;
using Project.Server.Controllers;
using Project.Server.Factories;
using Project.Server.Interfaces;
using System.Reflection;

namespace Project.Server
{
    internal class ProjectMain : AsyncResource
    {
        private ServiceCollection _serviceCollection = new();

        public override void OnStart()
        {
            Console.WriteLine("Server Resource started");

            // Thank you SmOkEwOw for helping me even understanding this process
            // even if he didn't know that he was helping me

            _serviceCollection.AddSingleton<ILogger, ConsoleLogger>();
            _serviceCollection.AddSingleton<IRpcService, RpcController>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass && typeof(IController).IsAssignableFrom(type))
                {
                    _serviceCollection.AddSingleton(typeof(IController), type);
                }
            }

            IServiceProvider serviceProvider = _serviceCollection.BuildServiceProvider();

            IRpcService? rpcService = serviceProvider.GetService<IRpcService>();
            rpcService?.OnStart();

            foreach (IController controller in serviceProvider.GetServices<IController>())
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
