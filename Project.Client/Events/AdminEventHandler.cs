using AltV.Net;
using AltV.Net.Client;
using Project.Shared;

namespace Project.Client.Events
{
    internal class AdminEventHandler : IScript
    {
        public bool IsNoclipEnabled { get; private set; }

        public AdminEventHandler()
        {
            Alt.OnServer(AdminEvents.NOCLIP, ToggleNoclip);
        }

        public void ToggleNoclip()
        {
            IsNoclipEnabled = !IsNoclipEnabled;
        }

        public void Dispose()
        {
            Console.WriteLine($"Disposing Admin Event Handler");
            Alt.OffServer(AdminEvents.NOCLIP, null);
        }
    }
}
