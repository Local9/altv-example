using AltV.Net.Client.Async;

namespace Project.Client
{
    internal class ProjectMain : AsyncResource
    {
        public override void OnStart()
        {
            Console.WriteLine("Client Resource started");
        }

        public override void OnStop()
        {
            Console.WriteLine("Client Resource stopped");
        }
    }
}
