using AltV.Net;
using AltV.Net.Data;
using Project.Server.Factories;
using Project.Shared;

namespace Project.Server.Events
{
    internal class AdminEventHandler : IScript
    {
        [ClientEvent(AdminEvents.TELEPORT_TO_COORDS)]
        public void TeleportToCoords(IAltPlayer player, float x, float y, float z)
        {
            if (player.IsInVehicle) player.Vehicle.Position = new Position(x, y, z);
            else player.Position = new Position(x, y, z);
        }
    }
}
