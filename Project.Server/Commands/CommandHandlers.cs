using AltV.Net.Elements;
using AltV.Net.Elements.Args;
using AltV.Net.Elements.Entities;
using Project.Server.Factories;

namespace Project.Server.Commands
{
    // TODO: Clean this mess up

    public class Chat : IController
    {
        public void OnStart()
        {
            Console.WriteLine("Chat controller started");

            Alt.OnClient<IAltPlayer, string>("chat:message", OnChatMessage, OnChatMessageParser);
        }

        public void OnStop()
        {
        }

        private void OnChatMessage(IAltPlayer player, string message)
        {
            Console.WriteLine($"OnChatMessage fired with message '{message}'");

            if (string.IsNullOrEmpty(message)) return;
            if (message.StartsWith("/"))
            {
                message = message.Trim().Remove(0, 1);
                if (message.Length > 0)
                {
                    Alt.Log("[chat:cmd] " + player.Name + ": /" + message);
                    string[] args = message.Split(' ');
                    string cmd = args[0];
                    CommandHandlers.InvokeCommand(player, cmd, args.Skip(1).ToArray());
                }
            }
            else
            {
                message = message.Trim();
                if (message.Length > 0)
                {
                    Alt.Log("[chat:msg] " + player.Name + ": " + message);

                    Alt.EmitAllClients("chatmessage", player.Name, message);
                }
            }
        }

        private void OnChatMessageParser(IPlayer player, MValueConst[] mValueArray,
            Action<IAltPlayer, string> func)
        {
            if (mValueArray.Length != 1) return;
            MValueBuffer2 reader = mValueArray.Reader();
            if (!reader.GetNext(out string message)) return;

            func((IAltPlayer)player, message);
        }
    }

    internal static class CommandHandlers
    {
        private static readonly IDictionary<string, HashSet<Action<IAltPlayer, string, string[]>>> commandHandlers =
            new Dictionary<string, HashSet<Action<IAltPlayer, string, string[]>>>();

        public static void Add(string command, Action<IAltPlayer, string, string[]> handler)
        {
            if (!commandHandlers.TryGetValue(command, out HashSet<Action<IAltPlayer, string, string[]>>? handlers))
            {
                handlers = new HashSet<Action<IAltPlayer, string, string[]>>();
                commandHandlers[command] = handlers;
            }

            handlers.Add(handler);
        }

        public static void InvokeCommand(IAltPlayer player, string cmd, string[] args)
        {
            if (!commandHandlers.TryGetValue(cmd, out HashSet<Action<IAltPlayer, string, string[]>>? handlers))
            {
                player.SendChatMessage("{FF0000} Unknown command /" + cmd + "");
                return;
            }

            foreach (Action<IAltPlayer, string, string[]> handler in handlers)
            {
                handler(player, cmd, args);
            }
        }
    }

    internal static class PlayerChat
    {
        public static void SendChatMessage(this IPlayer player, string message)
        {
            player.Emit("chatmessage", null, message);
        }
    }
}
