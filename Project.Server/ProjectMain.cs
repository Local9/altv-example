using AltV.Net;

namespace Project.Server
{
    internal class ProjectMain : Resource
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
