#if CLIENT
using AltV.Net.Client.Elements.Interfaces;
#elif SERVER
using AltV.Net.Elements.Entities;
#endif

namespace Project.Shared.Interfaces
{
    internal interface IRpcService
    {
        void OnStart();
        void OnStop();

        Task<RpcResult<T>> SendRpc<T>(IPlayer player, double timeout, string eventName, params object[] args);

#if CLIENT
        void OnScriptRpcAnswer(ushort answerId, object answer, string answerError);
#elif SERVER
        void OnScriptRpcAnswer(IPlayer target, ushort answerId, object answer, string answerError);
#endif

    }
}