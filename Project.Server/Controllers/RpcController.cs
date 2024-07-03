using AltV.Net;
using AltV.Net.Elements.Entities;
using Project.Server.Interfaces;
using System.Collections.Concurrent;

namespace Project.Server.Controllers
{
    internal class RpcController : IRpcService
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<object>> _taskCompletionSources = new();
        private readonly ILogger _logger;

        public RpcController(ILogger logger)
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
            ushort rpcId = player.EmitRPC(eventName, args);
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

        public void OnScriptRpcAnswer(IPlayer target, ushort answerId, object answer, string answerError)
        {
            _logger.Log($"OnScriptRpcAnswer {answerId} @ {answer}");

            if (!_taskCompletionSources.TryRemove($"{target.Id}_{answerId}", out TaskCompletionSource<object>? tcs)) return;
            tcs.TrySetResult(answer);
        }
    }
}
