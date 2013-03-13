using System;
using System.Text;

namespace MCQuery
{
    internal class Packets
    {
        internal static byte[] Challenge (int SessionID)
        {
            Packet p = new Packet();
            p.Write((byte)0xFE);
            p.Write((byte)0xFD);
            p.Write((byte)0x09);
            p.Write(SessionID);
            return p.Finalize();
        }

        internal static byte[] QueryInfo (int SessionID, int ChallengeToken)
        {
            Packet p = new Packet();
            p.Write((byte)0xFE);
            p.Write((byte)0xFD);
            p.Write((byte)0x00);
            p.Write(SessionID);
            p.Write(ChallengeToken);
            return p.Finalize();
        }

        internal static byte[] QueryData (int SessionID, int ChallengeToken)
        {
            Packet p = new Packet();
            p.Write((byte)0xFE);
            p.Write((byte)0xFD);
            p.Write((byte)0x00);
            p.Write(SessionID);
            p.Write(ChallengeToken);
            p.Write((int)0);
            return p.Finalize();
        }
    }
}
