using System;
using MCQuery;

namespace TestClient
{
    class Program
    {
        static void Main (string[] args)
        {
            String host = "minecraft.shadowkitsune.net";

            /*
            var mcq = new MCSimpleQuery();
            mcq.Connect(host);

            if (mcq.Success())
            {
                var info = mcq.Info();
                Console.WriteLine("Server name: {0}", info.Name);
                Console.WriteLine("Server version: {0}", info.Version);
                Console.WriteLine("Server protocol: {0}", info.Protocol);
                Console.WriteLine("Online players: {0}", info.OnlinePlayers);
                Console.WriteLine("Max players: {0}", info.MaxPlayers);
                Console.WriteLine("Latency: {0}ms", info.Latency);
            }
            else Console.WriteLine("Failed. Is the game up?");*/

            var q = new MCQuery.MCQuery();
            q.Connect(host);
            if (q.Success())
            {
                var info = q.Info();
                Console.WriteLine("Server name: {0}", info.Name);
                Console.WriteLine("Server version: {0}", info.Version);
                Console.WriteLine("Server protocol: {0}", info.Protocol);
                Console.WriteLine("Game ID: {0}", info.GameID);
                Console.WriteLine("Game type: {0}", info.GameType);
                Console.WriteLine("Map name: {0}", info.Map);
                Console.WriteLine("Plugins: {0}", info.Plugins);
                Console.WriteLine("Protocol: {0}", info.Protocol);
                Console.WriteLine("Software: {0}", info.Software);
                Console.WriteLine("Host IP: {0}", info.HostIP);
                Console.WriteLine("Host Port: {0}", info.HostPort);
                Console.WriteLine("Online players: {0}", info.OnlinePlayers);
                Console.WriteLine("Max players: {0}", info.MaxPlayers);
                Console.WriteLine("Latency: {0}ms", info.Latency);

                if (info.Players != null && info.Players.Count > 0)
                {
                    Console.WriteLine("Players: ");

                    foreach (String player in info.Players)
                    {
                        Console.WriteLine(player);
                    }
                }
            }
            else Console.WriteLine("Failed. Is the game up?");

            Console.ReadKey();
        }
    }
}
