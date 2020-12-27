using System;
using System.Text;
using System.Net.Sockets;
using NLog;

namespace COAN
{ 
    public class Packet
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public const int SEND_MTU = 1460;
        public const int POS_PACKET_TYPE = 2;

        private int pos = 0;
        private readonly byte[] buf = new byte[SEND_MTU];

        private readonly Socket socket;
        PacketType type;

        public Packet(Socket socket, PacketType type)
        {
            this.socket = socket;
            this.buf = new byte[SEND_MTU];
            this.SetType(type);
            this.pos = POS_PACKET_TYPE + 1;
            
        }

        public Packet(Socket socket)
        {
            this.socket = socket;

            if (socket.Connected == false)
                return;

            int leng = socket.Receive(this.buf);

            if (leng == 0)
                throw new SocketException();

            this.pos = POS_PACKET_TYPE + 1;
        }

        public int length()
        {
            int b1 = this.buf[0] & 0xFF;
            int b2 = this.buf[1] & 0xFF;

            int r = (b1 + (b2 << 8));

            return r;
        }

        public Socket getSocket()
        {
            return this.socket;
        }

        void SetType(PacketType type)
        {
            this.buf[POS_PACKET_TYPE] = (byte)type;
        }

        public PacketType getType()
        {
            if (this.type == 0)
            {
                PacketType t = (PacketType)(this.buf[POS_PACKET_TYPE] & 0xFF);
                this.type = t;// PacketType.valueOf(this.buf[POS_PACKET_TYPE] & 0xFF);
            }

            return this.type;
        }

        public void WriteString(string str)
        {
            foreach (byte b in Encoding.Default.GetBytes(str))
            {
                this.buf[this.pos++] = b;
            }
            this.buf[this.pos++] = (byte)'\0';

        }

        public void writeUint8(short n)
        {
            this.buf[this.pos++] = (byte)n;
        }

        public void writeUint16(int n)
        {
            this.buf[this.pos++] = (byte)n;
            this.buf[this.pos++] = (byte)(n >> 8);
        }

        public void writeUint32(long n)
        {
            this.buf[this.pos++] = (byte)n;
            this.buf[this.pos++] = (byte)(n >> 8);
            this.buf[this.pos++] = (byte)(n >> 16);
            this.buf[this.pos++] = (byte)(n >> 24);
        }

        public void writeUint64(long n)
        {
            this.buf[this.pos++] = (byte)n;
            this.buf[this.pos++] = (byte)(n >> 8);
            this.buf[this.pos++] = (byte)(n >> 16);
            this.buf[this.pos++] = (byte)(n >> 24);
            this.buf[this.pos++] = (byte)(n >> 32);
            this.buf[this.pos++] = (byte)(n >> 40);
            this.buf[this.pos++] = (byte)(n >> 48);
            this.buf[this.pos++] = (byte)(n >> 56);
        }

        public int readUint8()
        {
            return (this.buf[this.pos++] & 0xFF);
        }

        public int readUint16()
        {
            int n = this.buf[this.pos++] & 0xFF;
            n += (this.buf[this.pos++] & 0xFF) << 8;

            return n;
        }

        public long readUint32()
        {
            long n = this.buf[this.pos++] & 0xFF;
            n += (this.buf[this.pos++] & 0xFF) << 8;
            n += (this.buf[this.pos++] & 0xFF) << 16;
            n += (this.buf[this.pos++] & 0xFF) << 24;

            return n;
        }

        public long readUint64()
        {
            long l = 0;
            l += (long)(this.buf[this.pos++] & 0xFF);
            l += (long)(this.buf[this.pos++] & 0xFF) << 8;
            l += (long)(this.buf[this.pos++] & 0xFF) << 16;
            l += (long)(this.buf[this.pos++] & 0xFF) << 24;
            l += (long)(this.buf[this.pos++] & 0xFF) << 32;
            l += (long)(this.buf[this.pos++] & 0xFF) << 40;
            l += (long)(this.buf[this.pos++] & 0xFF) << 48;
            l += (long)(this.buf[this.pos++] & 0xFF) << 56;

            return l;
        }

        public String readString()
        {
            String str = "";
            int startIdx = this.pos;

            while (this.buf[this.pos++] != (byte)'\0') ;

            int endIdx = this.pos - startIdx - 1;

            str = Encoding.GetEncoding("UTF-8").GetString(this.buf);
            str = str.Substring(startIdx, (this.pos - startIdx));
            
            return str;
        }


        public bool readBool()
        {
            return (this.buf[this.pos++] & 0xFF) > 0;
        }


        public void Send()
        {
            this.buf[0] = (byte)this.pos;
            this.buf[1] = (byte)(this.pos >> 8);

            this.socket.Send(buf, this.pos, SocketFlags.None);
        }

    }
}
