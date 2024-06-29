using AltV.Net.Async;

namespace Project.Server
{
    internal class ProjectMain : AsyncResource
    {
        public override void OnStart()
        {
            Console.WriteLine("Server Resource started");
        }

        public override void OnStop()
        {
            Console.WriteLine("Server Resource stopped");
        }
    }
}
