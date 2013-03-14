namespace MCQuery
{
    public struct ServerInfo
    {
        public string   Name;
        public string   GameType;
        public string   GameID;
        public string   Plugins;
        public string   Map;
        public string   HostIP;
        public string   HostPort;
        public string   Software;
        public string   Version;
        public int      OnlinePlayers;
        public int      MaxPlayers;
        public int      Latency;
        public double   Protocol;

        public System.Collections.Generic.List<string> Players;
    }

    public interface ServerQuery
    {
        void Connect (string host, int port = 25565, double timeout = 2.5);
        bool Success ();
        ServerInfo Info ();
    }
}
