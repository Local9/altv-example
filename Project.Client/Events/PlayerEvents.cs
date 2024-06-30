using AltV.Net;

namespace Project.Client.Events
{
    internal class PlayerEvents : IScript
    {
        public PlayerEvents()
        {
            Alt.OnServer<string>("myEvent", MyEvent);
        }

        [ServerEvent("myEvent")]
        public void MyEvent(string message)
        {
            Console.WriteLine(message);
        }
    }
}
