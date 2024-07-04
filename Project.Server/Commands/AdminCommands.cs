using Project.Server.Factories;
using System.Numerics;

namespace Project.Server.Commands
{
    internal class AdminCommands : IController
    {
        private ILogger _logger;
        private IRpcService _rpcService;

        public AdminCommands()
        {
        }

        public AdminCommands(ILogger logger, IRpcService rpcService)
        {
            _logger = logger;
            _rpcService = rpcService;

            _logger.Log("Admin Commands Initialized");
        }

        public void OnStart()
        {
            _logger.Log("Admin Commands Started");
            CommandHandlers.Add("noclip", NoClip);
            CommandHandlers.Add("tpw", TeleportWaypoint);
        }

        public void OnStop()
        {

        }

        public void NoClip(IAltPlayer player, string cmd, string[] args)
        {
            // todo: check if player is admin

            _logger.Log($"3^NoClip command called by {player.Name}");

            player.NoClip = !player.NoClip;
            player.Streamed = !player.NoClip;
            player.Visible = !player.NoClip;
            player.Collision = !player.NoClip;
            player.ClearTasks();

            player.SendChatMessage($"{{00FF00}}NoClip is now {(player.NoClip ? "enabled" : "disabled")}!");

            player.Emit(AdminEvents.NOCLIP, player.NoClip);
        }

        public async void TeleportWaypoint(IAltPlayer player, string cmd, string[] args)
        {
            try
            {
                _logger.Log($"Attempting to use RPC");

                RpcResult<Vector3> rpcResult = await _rpcService.SendRpc<Vector3>(player, 5, AdminEvents.GET_WAYPOINT_POSITION);

                if (!rpcResult.Succeeded)
                {
                    player.SendChatMessage($"{{FF0000}}Error: {rpcResult.Error}");
                    return;
                }

                if (rpcResult.Result == Vector3.Zero)
                {
                    player.SendChatMessage($"{{FF0000}}Error: Unable to teleport to position.");
                    return;
                }

                player.Position = rpcResult.Result;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
            }
        }
    }
}
