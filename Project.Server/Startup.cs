using Microsoft.Extensions.DependencyInjection;
using Project.Shared.Services;
using System.Reflection;

namespace Project.Server
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            // Thank you SmOkEwOw for helping me even understanding this process
            // even if he didn't know that he was helping me

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ILogger, ConsoleLogger>();
            serviceCollection.AddSingleton<IRpcService, RpcService>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass && typeof(IController).IsAssignableFrom(type))
                {
                    serviceCollection.AddSingleton(typeof(IController), type);
                }
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
