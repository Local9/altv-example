using AltV.Net.Client.Async;
using Project.Client.Interfaces;
using System.Reflection;

namespace Project.Client
{
    internal class ProjectMain : AsyncResource
    {
        public override void OnStart()
        {
            Console.WriteLine("ProjectMain Client Resource started");

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                // if the type is a class and implements IScript
                if (type.IsClass && typeof(IAltScript).IsAssignableFrom(type))
                {
                    // then create an instance of the type and call OnStart without using the IScript interface
                    type?.GetMethod("OnStart")?.Invoke(Activator.CreateInstance(type), null);
                }
            }

            Console.WriteLine("ProjectMain Client Resource Loading Completed");
        }

        public override void OnStop()
        {
            Console.WriteLine("Client Resource stopped");
        }
    }
}
