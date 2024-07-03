#if SERVER
using AltV.Net.Elements.Entities;
#elif CLIENT
using AltV.Net.Client.Elements.Interfaces;
#endif
using System.Collections.Concurrent;

namespace Project.Shared.Services
{
    internal class RpcService : IRpcService
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<object>> _taskCompletionSources = new();
        private readonly ILogger _logger;

        public RpcService(ILogger logger)
        {
            _logger = logger;
        }

        public void OnStart()
        {
            _logger.Log("RpcController started");
            Alt.OnScriptRPCAnswer += OnScriptRpcAnswer;
        }

        public void OnStop()
        {

        }

        public async Task<RpcResult<T>> SendRpc<T>(IPlayer player, double timeout, string eventName, params object[] args)
        {
#if SERVER
            ushort rpcId = player.EmitRPC(eventName, args);
#elif CLIENT
            ushort rpcId = Alt.EmitRPC(eventName, args);
#endif
            _logger.Log($"player.EmitRPC {rpcId}");
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            _taskCompletionSources.TryAdd($"{player.Id}_{rpcId}", tcs);

            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            cts.Token.Register(() => tcs.TrySetCanceled());

            try
            {
                object result = await tcs.Task;
                if (result is T res) return res;
                return RpcError.InvalidDataReceived;
            }
            catch (OperationCanceledException)
            {
                return RpcError.TimedOut;
            }
        }

#if SERVER
        public void OnScriptRpcAnswer(IPlayer target, ushort answerId, object answer, string answerError)
        {
            _logger.Log($"OnScriptRpcAnswer {answerId} @ {answer}");

            if (!_taskCompletionSources.TryRemove($"{target.Id}_{answerId}", out TaskCompletionSource<object>? tcs)) return;
            tcs.TrySetResult(answer);
        }
#elif CLIENT
        public void OnScriptRpcAnswer(ushort answerId, object answer, string answerError)
        {
            _logger.Log($"OnScriptRpcAnswer {answerId} @ {answer}");

            if (!_taskCompletionSources.TryRemove($"{Alt.LocalPlayer.Id}_{answerId}", out TaskCompletionSource<object>? tcs)) return;
            tcs.TrySetResult(answer);
        }
#endif
    }
}
