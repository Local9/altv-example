using AltV.Net;
using AltV.Net.Data;
using Project.Server.Factories;

namespace Project.Server.Events
{
    internal class AdminEventHandler : IController
    {
        private readonly ILogger _logger;
        private readonly IRpcService _rpcService;

        public AdminEventHandler()
        {
        }

        public AdminEventHandler(ILogger logger, IRpcService rpcService)
        {
            _logger = logger;
            _rpcService = rpcService;

            _logger.Log("AdminEventHandler constructor");
        }

        public void OnStart()
        {

        }

        public void OnStop()
        {

        }

        [ClientEvent(AdminEvents.TELEPORT_TO_COORDS)]
        public void TeleportToCoords(IAltPlayer player, float x, float y, float z)
        {
            if (player.IsInVehicle) player.Vehicle.Position = new Position(x, y, z);
            else player.Position = new Position(x, y, z);
        }
    }
}
