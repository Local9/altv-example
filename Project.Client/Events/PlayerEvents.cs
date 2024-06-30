using AltV.Net;

namespace Project.Client.Events
{
    internal class PlayerEvents : IScript
    {
        [ServerEvent("myEvent")]
        public void MyEvent(string message)
        {
            Console.WriteLine(message);
        }
    }
}
