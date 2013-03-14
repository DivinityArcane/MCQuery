using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace MCQuery
{
    public class MCQuery : ServerQuery
    {
        private bool success = false;
        private ServerInfo info;
        private int challenge = 0;
        private int SID = 0;
        private int ping = 0;

        private Socket sock;

        public bool Success () { return success; }
        public ServerInfo Info () { return info; }

        public void Connect (string host, int port = 25565, double timeout = 2.5)
        {
            try
            {
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                if (sock == null) return;

                sock.Connect(host, port);
                sock.ReceiveTimeout = (int)(timeout * 1000);
                sock.SendTimeout = (int)(timeout * 1000);

                this.GetChallenge();
            }
            catch
            {
                success = false;
            }
        }

        internal void GetChallenge ()
        {
            SID = new Random().Next() & 0xCF;
            Packet data = this.GrabData(Packets.Challenge(this.SID));
            if (data.Read<byte>() == (byte)0x09 && data.Read<int>() == this.SID)
            {
                int.TryParse(data.Read<String>(), out this.challenge);
                byte[] iabi = BitConverter.GetBytes(this.challenge);
                Array.Reverse(iabi);
                this.challenge = BitConverter.ToInt32(iabi, 0);

                ping = Environment.TickCount;

                this.GetInfo();
            }
            else GetChallenge();
        }

        internal void GetInfo ()
        {
            Packet data = this.GrabData(Packets.QueryData(this.SID, this.challenge));

            if (data == null)
            {
                this.GetChallenge();
                return;
            }

            if (data.Read<byte>() == (byte)0x00 && data.Read<int>() == this.SID)
            {
                info = new ServerInfo();

                info.Latency = Environment.TickCount - ping;

                data.Skip(11); // Unknown padding.

                string key, value;

                while (true)
                {
                    key = data.Read<string>();
                    value = data.Read<string>();

                    if (key.Length == 0) break;

                    if (key == "hostname")
                        info.Name = value;

                    else if (key == "gametype")
                        info.GameType = value;

                    else if (key == "game_id")
                        info.GameID = value;

                    else if (key == "version")
                        info.Version = value;

                    else if (key == "plugins")
                        info.Plugins = value;

                    else if (key == "map")
                        info.Map = value;

                    else if (key == "numplayers")
                    {
                        if (!int.TryParse(value, out info.OnlinePlayers)) return;
                    }

                    else if (key == "maxplayers")
                    {
                        if (!int.TryParse(value, out info.MaxPlayers)) return;
                    }

                    else if (key == "hostport")
                        info.HostPort = value;

                    else if (key == "hostip")
                        info.HostIP = value;
                }

                data.Skip(1);

                info.Players = new List<string>();

                while (true)
                {
                    key = data.Read<string>();

                    if (key.Length == 0) break;

                    info.Players.Add(key);
                }

                success = true;
            }
            else GetChallenge();
        }

        internal Packet GrabData (byte[] packet)
        {
            this.Send(packet);

            Packet recv = this.Receive(2048, packet[2]);

            if (recv == null) return null;

            return recv;
        }

        internal Packet Receive (int len, byte check)
        {
            try
            {
                byte[] buffer = new byte[len];
                int recv = this.sock.Receive(buffer, 0, len, SocketFlags.None);

                if (recv < 5 || buffer[0] != check)
                    return null;

                byte[] nbuf = new byte[recv];
                Buffer.BlockCopy(buffer, 0, nbuf, 0, recv);

                return new Packet(nbuf);
            }
            catch
            {
                return null;
            }
        }

        internal void Send (byte[] data)
        {
            try
            {
                sock.Send(data);
            }
            catch
            {

            }
        }

        internal void Send (string data)
        {
            try
            {
                this.Send(Encoding.Unicode.GetBytes(data));
            }
            catch
            {

            }
        }
    }
}
