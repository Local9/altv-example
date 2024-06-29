using AltV.Net.Client;

namespace Project.Client
{
    internal class ProjectMain : Resource
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
