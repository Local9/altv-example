using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;

namespace Project.Server.Scripts
{
    internal class Events : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void PlayerConnect(IPlayer player, string reason)
        {
            player.Spawn(new AltV.Net.Data.Position(-425, 1123, 325));
            player.SetDateTime(DateTime.Now);
            player.Model = (uint)PedModel.FreemodeMale01;
        }
    }
}
