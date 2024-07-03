using AltV.Net.Data;
using Project.Server.Factories;

namespace Project.Server.Controllers
{
    internal class AdminController : IController
    {
        private readonly ILogger _logger;
        private readonly IRpcService _rpcService;

        public AdminController()
        {
        }

        public AdminController(ILogger logger, IRpcService rpcService)
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
