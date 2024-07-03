using AltV.Net.Client.Elements.Interfaces;

namespace Project.Client.Interfaces
{
    internal interface IRpcService
    {
        void OnStart();
        void OnStop();

        Task<RpcResult<T>> SendRpc<T>(IPlayer player, double timeout, string eventName, params object[] args);
        void OnScriptRpcAnswer(ushort answerId, object answer, string answerError);
    }
}
