using AltV.Net.Client.Async;
using Project.Client.Events;

namespace Project.Client
{
    internal class ProjectMain : AsyncResource, IScript
    {
        private AdminEventHandler _adminEventHandler;

        public override void OnStart()
        {
            Console.WriteLine("ProjectMain Client Resource started");

            _adminEventHandler = new AdminEventHandler();

            Console.WriteLine("ProjectMain Registered Events");
        }

        public override void OnStop()
        {
            _adminEventHandler.Dispose();
            Console.WriteLine("Client Resource stopped");
        }
    }
}
