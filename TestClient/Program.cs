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
            mcq.Query(host);

            if (mcq.Success)
            {
                Console.WriteLine("Server name: {0}", mcq.Info.Name);
                Console.WriteLine("Server version: {0}", mcq.Info.Version);
                Console.WriteLine("Server protocol: {0}", mcq.Info.Protocol);
                Console.WriteLine("Online players: {0}", mcq.Info.OnlinePlayers);
                Console.WriteLine("Max players: {0}", mcq.Info.MaxPlayers);
                Console.WriteLine("Latency: {0}ms", mcq.Info.Latency);
            }
            else Console.WriteLine("Failed. Is the game up?");*/

            var q = new MCQuery.MCQuery();
            q.Connect(host);
            if (q.Success)
            {
                Console.WriteLine("Server name: {0}", q.Info.Name);
                Console.WriteLine("Server version: {0}", q.Info.Version);
                Console.WriteLine("Server protocol: {0}", q.Info.Protocol);
                Console.WriteLine("Game ID: {0}", q.Info.GameID);
                Console.WriteLine("Game type: {0}", q.Info.GameType);
                Console.WriteLine("Map name: {0}", q.Info.Map);
                Console.WriteLine("Plugins: {0}", q.Info.Plugins);
                Console.WriteLine("Protocol: {0}", q.Info.Protocol);
                Console.WriteLine("Software: {0}", q.Info.Software);
                Console.WriteLine("Host IP: {0}", q.Info.HostIP);
                Console.WriteLine("Host Port: {0}", q.Info.HostPort);
                Console.WriteLine("Online players: {0}", q.Info.OnlinePlayers);
                Console.WriteLine("Max players: {0}", q.Info.MaxPlayers);
                Console.WriteLine("Latency: {0}ms", q.Info.Latency);

                if (q.Info.Players != null && q.Info.Players.Count > 0)
                {
                    Console.WriteLine("Players: ");

                    foreach (String player in q.Info.Players)
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
