using AltV.Net.Client.Async;
using AltV.Net.Shared.Elements.Entities;
using System.Numerics;

namespace Project.Client.Controllers.Admin
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

            _logger.Log("AdminController constructor");
        }

        public void OnStart()
        {
            Alt.OnScriptRPC += AdminGetWayPointPosition;
        }

        public void OnStop()
        {

        }

        private async void AdminGetWayPointPosition(IScriptRPCEvent scriptRpcEvent, string name, object[] args, ushort answerId)
        {
            Vector3 waypointPos = GetWaypointPosition();

            if (waypointPos == Vector3.Zero)
            {
                scriptRpcEvent.Answer(waypointPos);
                return;
            }

            Alt.FocusData.OverrideFocusPosition(waypointPos, Vector3.Zero);
            Vector3 startPos = new Vector3(waypointPos.X, waypointPos.Y, 1500f);
            Vector3 endPos = startPos;
            Vector3 groundPos = Vector3.Zero;

            try
            {
                await AltAsync.WaitFor(() =>
                {
                    endPos -= new Vector3(0, 0, 200.0f);

                    Alt.FocusData.OverrideFocusPosition(endPos, Vector3.Zero);

                    if (endPos.Z < -500f)
                        throw new Exception("No ground found");

                    (bool hit, groundPos, uint hitEntity) = RayCast(startPos, endPos);

                    return hit;
                }, 3000);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
            }

            if (groundPos == Vector3.Zero)
            {
                _logger.Log("failed to find ground position, trying groundZ");

                Alt.FocusData.OverrideFocusPosition(waypointPos, Vector3.Zero);

                float groundZ = 0f;
                bool foundGround = false;

                try
                {
                    await AltAsync.WaitFor(() =>
                    {
                        foundGround = Alt.Natives.GetGroundZAndNormalFor3dCoord(waypointPos.X, waypointPos.Y, 1500f, ref groundZ, ref groundPos);

                        return foundGround;
                    }, 3000);
                }
                catch (Exception ex)
                {
                    _logger.Log(ex.Message);
                }

                if (!foundGround)
                {
                    _logger.Log("failed to get ground z for waypoint");
                    groundPos = Vector3.Zero;
                }
                else
                    groundPos = new Vector3(waypointPos.X, waypointPos.Y, groundZ + 2f);
            }

            Alt.FocusData.ClearFocusOverride();

            scriptRpcEvent.Answer(groundPos);
        }

        private Vector3 GetWaypointPosition()
        {
            int waypoint = Alt.Natives.GetFirstBlipInfoId(8);
            if (Alt.Natives.DoesBlipExist(waypoint))
            {
                return Alt.Natives.GetBlipInfoIdCoord(waypoint);
            }

            return Vector3.Zero;
        }

        private (bool, Vector3, uint) RayCast(Vector3 start, Vector3 end, int flags = 99999)
        {
            int raycast = Alt.Natives.StartExpensiveSynchronousShapeTestLosProbe(start.X, start.Y, start.Z, end.X, end.Y, end.Z, flags, Alt.LocalPlayer, 0);

            bool hit = false;
            Vector3 hitPosition = new Vector3();
            Vector3 surfaceNormal = new Vector3();
            uint hitEntity = 0;

            Alt.Natives.GetShapeTestResult(raycast, ref hit, ref hitPosition, ref surfaceNormal, ref hitEntity);

            if (!hit) return (false, Vector3.Zero, 0);

            return (true, hitPosition, hitEntity);
        }
    }
}
