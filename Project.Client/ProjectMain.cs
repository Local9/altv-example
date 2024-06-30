using AltV.Net.Client.Async;
using Project.Client.Events;

namespace Project.Client
{
    internal class ProjectMain : AsyncResource
    {
        public override void OnStart()
        {
            Console.WriteLine("ProjectMain Client Resource started");

            _ = new AdminEventHandler();

            Console.WriteLine("ProjectMain Registered Events");
        }

        public override void OnStop()
        {
            Console.WriteLine("Client Resource stopped");
        }
    }
}
