using AltV.Net.Elements.Entities;

namespace Project.Server.Interfaces
{
    internal interface IRpcService
    {
        void OnStart();
        void OnStop();

        Task<RpcResult<T>> SendRpc<T>(IPlayer player, double timeout, string eventName, params object[] args);
        void OnScriptRpcAnswer(IPlayer target, ushort answerId, object answer, string answerError);
    }
}
