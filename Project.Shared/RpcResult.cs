using Project.Shared.Data;
using System.Diagnostics.CodeAnalysis;

namespace Project.Shared
{
    internal class RpcResult<T>
    {
        [MemberNotNullWhen(true, nameof(Result))]
        public bool Succeeded => Error == RpcError.None;

        public RpcError Error { get; set; } = RpcError.None;

        public T? Result { get; set; }

        public static implicit operator RpcResult<T>(RpcError error) => new() { Error = error };
        public static implicit operator RpcResult<T>(T result) => new() { Result = result };
    }
}
