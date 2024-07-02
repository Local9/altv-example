using AltV.Net;
using AltV.Net.Client;
using Project.Shared;

namespace Project.Client.Events
{
    internal class AdminEventHandler : IScript
    {
        public AdminEventHandler()
        {
            Alt.OnServer<string>(AdminEvents.NOCLIP, Teleport);
        }

        public void Teleport(string fuck)
        {
            Console.WriteLine("Teleporting to spawnpoint");

            Alt.Natives.SetLocalPlayerAsGhost(true, true);
            Alt.Natives.SetPlayerWantedLevel(Alt.LocalPlayer, 5, false);

            Alt.LocalPlayer.Vehicle?.Destroy();
        }
    }
}
