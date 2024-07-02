using AltV.Net;
using AltV.Net.Client;
using AltV.Net.Client.Async;
using Project.Client.Events;
using Project.Shared;

namespace Project.Client
{
    internal class ProjectMain : AsyncResource, IScript
    {
        private AdminEventHandler _adminEventHandler;

        public override void OnStart()
        {
            Console.WriteLine("ProjectMain Client Resource started");

            Alt.OnServer<string>(AdminEvents.START, OnStartAdmin);
            Alt.OnServer<string>(AdminEvents.STOP, OnStopAdmin);

            Console.WriteLine("ProjectMain Registered Events");
        }

        private void OnStopAdmin(string obj)
        {
            // Stop the admin event handler
            _adminEventHandler?.Dispose();
        }

        private void OnStartAdmin(string obj)
        {
            _adminEventHandler = new AdminEventHandler();
        }

        public override void OnStop()
        {
            Console.WriteLine("Client Resource stopped");
        }
    }
}
