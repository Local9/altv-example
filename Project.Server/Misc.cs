using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;

namespace Project.Server
{
    internal static class Misc
    {
        [ThreadStatic]
        public static Random Random = new Random();

        public static readonly Position[] SpawnPositions = {
            new (-1734.69885f,-1108.47033f, 14.05346f ),    // Pier
            new (-2162.94067f, -398.45275f,14.373657f),     // Parking Lot at the beach-highway
            new (-1687.70104f,-311.49890f,52.63952f),       // Church
            new (-1304.20214f,111.66593f,57.55969f),        // Golf Club
            new (-542.14947f,252.72528f, 84.04760f),        // intercept at tequilala bar
            new (-81.27033f,-611.86810f, 37.30627f),        // Arcadius
            new (165.87692f,-986.98022f, 31.08862f ),       // Good old lesion square
            new (402.21099f,-981.62634f, 30.39782f),        // LSPD Mission Row
            new (6.14505f,-1749.01977f, 30.29675f),         // Mega Mall near Groove Street
            new (102.63296f,-1939.50329f, 20.7964f),        // Groove Street
            new (-279.12527f,-2579.48559f, 6.99340f),       // Harbour
            new (883.93848f, -43.96484f, 79.75098f),        // Casino parkin lot
            new (660.51428f, 29.47253f, 86.37292f),         // Vinewood PD
            new (67.49011f, -726.80438f, 45.20874f),        // FIB Tower
            new (-633.78461f, -1297.49011f, 11.66077f),     // La puerta intersect
            new (684.59338f, 577.60876f, 131.44617f),       // Theatre
            new (-75.01978f, -1084.23291f, 27.81982f),      // motorsports car dealer
            new (257.82858f, -574.00879f, 44.29895f),       // Pillbox hospital
            new (-1092.71204f, -402.59341f, 37.62634f),     // TV Show Production thing
            new (-926.16266f, 295.51648f, 71.86523f),       // little park in Rockford Hills
            new (-410.33408f, 1178.50549f, 326.63440f),     // Observatory
            new (-1732.06152f, 159.11209f, 65.36121f),      // Sports field
            new (1958.4264f, 3722.1626f, 32.363403f),
            new (2026.6285f, 4756.3647f, 41.041016f),
            new (150.73846f, 6424.958f, 31.285034f),
            new (-1997.4198f, 3073.0945f, 32.801514f),
        };

        public static readonly Position[] AirportSpawnPositions = {
            new (-1100.89990234375f, -2659.896240234375f, 13.756650924682617f),
            new (-960.344970703125f, -2753.627685546875f, 13.83371639251709f),
            new (-964.8075561523438f, -3002.284912109375f, 13.945064544677734f),
            new (-1776.1871337890625f, -2773.560546875f, 13.944681167602539f),
            new (-1216.00244140625f, -2799.224609375f, 13.945316314697266f),
            new (-1276.5977783203125f, -3385.822021484375f, 13.940142631530762f),
            new (-1655.9744873046875f, -3149.0458984375f, 13.985773086547852f),
            new (-1460.2476806640625f, -3307.091552734375f, 13.945180892944336f),
            new (-1319.1428f, -3274.2197f, 13.23877f),
            new (-1461.8638f, -3203.8682f, 13.23877f),
            new (-1754.6505f, -3078.1187f, 13.542114f),
            new (-1697.0374f, -2894.2944f, 13.575806f),
            new (-1554.2373f, -2664.356f, 14.064453f),
            new (-1212.9495f, -2572.1538f, 13.23877f),
            new (-1195.0022f, -2274.6858f, 13.23877f),
            new (-958.0615f, -2596.8396f, 13.154541f),
            new (-892.89233f, -2729.5254f, 13.12085f),
            new (-807.5604f, -2664.6594f, 13.104004f),
            new (-675.2044f, -2378.4658f, 13.087158f),
        };

        public static bool ChatState = true;
        public static int Hour = 11;
        public static uint Weather = 0;

        public static bool IsResourceLoaded(string resourceName)
        {
            INativeResource[] allResources = Alt.GetAllResources();
            return allResources.Count(x => x.Name == resourceName) > 0;
        }

        public static int RandomInt(int min, int max)
        {
            int randomNumber = Random.Next(min, max + 1);
            return randomNumber;
        }

        public static void SendChatMessageToAll(string message)
        {
            foreach (IPlayer player in Alt.GetAllPlayers())
            {
                player.SendChatMessage(message);
            }
        }
    }
}
