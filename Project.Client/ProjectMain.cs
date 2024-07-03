using AltV.Net.Client.Async;
using Microsoft.Extensions.DependencyInjection;
using Project.Client.Controllers;
using Project.Client.Interfaces;
using System.Reflection;

namespace Project.Client
{
    internal class ProjectMain : AsyncResource
    {
        private ServiceCollection _serviceCollection = new();

        public override void OnStart()
        {
            Console.WriteLine("ProjectMain Client Resource started");

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

            Console.WriteLine("ProjectMain Client Resource Loading Completed");
        }

        public override void OnStop()
        {
            Console.WriteLine("Client Resource stopped");
        }
    }
}
