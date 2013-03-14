using System.Net.Sockets;

namespace MCQuery
{
    public class MCSimpleQuery : ServerQuery
    {
        private bool success = false;
        private ServerInfo info;

        public bool Success () { return success; }
        public ServerInfo Info () { return info; }

        public void Connect (string host, int port = 25565, double timeout = 2.5)
        {
            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                if (sock == null) return;

                sock.Connect(host, port);
                sock.ReceiveTimeout = (int)(timeout * 1000);
                sock.SendTimeout = (int)(timeout * 1000);

                int start = System.Environment.TickCount;

                sock.Send(new byte[2] { 0xFE, 0x01 });

                byte[] buffer = new byte[512];

                int recv = sock.Receive(buffer, 0, 512, SocketFlags.None);

                if (recv < 4 || buffer[0] != 0xFF) return;

                byte[] nbuf = new byte[recv];
                System.Buffer.BlockCopy(buffer, 0, nbuf, 0, recv);

                string packet = System.Text.Encoding.UTF8.GetString(nbuf).Substring(3);
                string[] bits;

                if (packet[1] != 0xA7 && packet[2] != 0x31)
                {
                    // MC 1.4 and later?
                    bits = packet.Split(new string[1] { "\x00\x00\x00" }, System.StringSplitOptions.None);

                    info = new ServerInfo();

                    info.Latency = System.Environment.TickCount - start;

                    if (!double.TryParse(bits[1].Replace("\x00", ""), out info.Protocol)) return;

                    info.Version = bits[2].Replace("\x00", "");
                    info.Name = bits[3].Replace("\x00", "");

                    if (!int.TryParse(bits[4].Replace("\x00", ""), out info.OnlinePlayers)) return;
                    if (!int.TryParse(bits[5].Replace("\x00", ""), out info.MaxPlayers)) return;

                    success = true;
                    return;
                }
                else
                {
                    // Earlier versions
                    bits = packet.Split(new char[1] { '\xA7' });

                    info = new ServerInfo();

                    info.Latency = System.Environment.TickCount - start;

                    info.Version = "1.3";
                    info.Name = bits[0].Replace("\x00", "");

                    if (bits.Length > 1)
                        if (!int.TryParse(bits[4].Replace("\x00", ""), out info.OnlinePlayers)) return;

                    if (bits.Length > 2)
                        if (!int.TryParse(bits[5].Replace("\x00", ""), out info.MaxPlayers)) return;

                    success = true;
                    return;
                }
            }
            catch
            {
                success = false;
            }
        }
    }
}
