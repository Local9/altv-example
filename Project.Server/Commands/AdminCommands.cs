using AltV.Net.Resources.Chat.Api;
using Microsoft.Extensions.DependencyInjection;
using Project.Server.Factories;
using System.Numerics;

namespace Project.Server.Commands
{
    internal class AdminCommands : IController
    {
        private readonly ILogger _logger;
        private readonly IRpcService _rpcService;

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
        }

        public void OnStop()
        {

        }

        [Command("noclip")]
        public void NoClip(IAltPlayer player)
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

        [Command("tpw")]
        public async void TeleportWaypoint(IAltPlayer player)
        {
            // SHOULDN'T NEED TO DO THIS BUT SOMETHING IS FUCKING WRONG
            ILogger _logger = ProjectMain.ServiceCollection.GetService<ILogger>();
            IRpcService _rpcService = ProjectMain.ServiceCollection.GetService<IRpcService>();

            try
            {
                // don't know why this is null, but it is null, and its fucking annoying
                // if you know how to fix this, I'm all ears...
                if (_rpcService == null)
                {
                    player.SendChatMessage($"{{FF0000}}Error: RpcService is null.");
                    return;
                }

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
