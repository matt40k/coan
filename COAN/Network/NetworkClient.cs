using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using enums;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;

namespace COAN
{
    public class NetworkClient
    {
        private Protocol protocol;
        private OpenTTD openttd;
        private Socket socket;
        private Thread mThread;

        #region Delegates
        public delegate void onChat(enums.NetworkAction action, enums.DestType dest, long clientId, string message, long data);
        public delegate void onClientInfo(Client client);
        public delegate void onProtocol(Protocol protocol);
        public delegate void onWelcome();
        #endregion

        #region Events
        public event onChat OnChat;
        public event onClientInfo OnClientInfo;
        public event onProtocol OnProtocol;
        public event onWelcome OnServerWelcome;
        #endregion

        public NetworkClient(OpenTTD openttd)
        {
            this.openttd = openttd;
            this.protocol = new Protocol();
            mThread = new Thread(() =>
            {
                while (IsConnected() == true)
                    receive();
            });
        }

        public bool Connect(string host, int port)
        {
            if (getOpenTTD().getPassword().Length == 0)
            {
                MessageBox.Show("Can't connect with empty password");
                return false;
            }
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            this.socket.Connect(host, port);

            sendAdminJoin();

            return true;
        }

        public Boolean IsConnected()
        {
            return socket.Connected;
        }

        public void Start()
        {
            mThread.Start();
        }

        public void receive()
        {
            try
            {
                Packet p = NetworkInputThread.getNext(getSocket());
                delegatePacket(p);
            }
            catch (Exception)
            {
            }
        }

        public void delegatePacket(Packet p)
        {
            Type t = this.GetType();
            String dispatchName = p.getType().getDispatchName();

            System.Reflection.MethodInfo method = t.GetMethod(dispatchName);

            System.Reflection.MethodInfo[] mis = t.GetMethods();

            try
            {
                method.Invoke(this, new object[] { getOpenTTD(), p });
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Method: " + dispatchName);
            }
        }

        #region Polls
        public void pollCmdNames()
        {
            sendAdminPoll(AdminUpdateType.ADMIN_UPDATE_CMD_NAMES);
        }

        /// <summary>
        /// Poll for information on a client if clientId is passed
        /// </summary>
        /// <param name="clientId">Optional parameter specifying the Client ID to get info on</param>
        public void pollClientInfos(long clientId = long.MaxValue)
        {
            sendAdminPoll(AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO, clientId);
        }

        /// <summary>
        /// Poll for information on a company if companyId is passed
        /// </summary>
        /// <param name="companyId">Optional parameter specifying the Company ID to get info on</param>
        public void pollCompanyInfos(long companyId = long.MaxValue)
        {
            sendAdminPoll(AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO, companyId);
        }

        public void pollCompanyStats()
        {
            sendAdminPoll(AdminUpdateType.ADMIN_UPDATE_COMPANY_STATS);
        }

        public void pollCompanyEconomy()
        {
            sendAdminPoll(AdminUpdateType.ADMIN_UPDATE_COMPANY_ECONOMY);
        }

        public void pollDate()
        {
            sendAdminPoll(AdminUpdateType.ADMIN_UPDATE_DATE);
        }
        #endregion

        #region Send Packets

        public void sendAdminJoin()
        {
            Packet p = new Packet(getSocket(), enums.PacketType.ADMIN_PACKET_ADMIN_JOIN);

            p.WriteString(getOpenTTD().getPassword());
            p.WriteString(getOpenTTD().getBotName());
            p.WriteString(getOpenTTD().getBotVersion());

            NetworkOutputThread.append(p);
        }

        public void sendAdminChat(NetworkAction action, DestType type, long dest, String msg, long data)
        {
            Packet p = new Packet(getSocket(), enums.PacketType.ADMIN_PACKET_ADMIN_CHAT);
            p.writeUint8((short)action);
            p.writeUint8((short)type);
            p.writeUint32(dest);

            msg = (msg.Length > 900) ? msg.Substring(0, 900) : msg;

            p.WriteString(msg);

            p.writeUint64(data);
            NetworkOutputThread.append(p);
        }

        public void sendAdminGameScript(string command)
        {
            Packet p = new Packet(getSocket(), PacketType.ADMIN_PACKET_ADMIN_GAMESCRIPT);


            p.WriteString(command); // JSON encode
            NetworkOutputThread.append(p);
        }

        public void sendAdminUpdateFrequency(AdminUpdateType type, AdminUpdateFrequency freq)
        {
            if (getProtocol().isSupported(type, freq) == false)
                throw new ArgumentException("The server does not support " + freq + " for " + type);

            Packet p = new Packet(getSocket(), PacketType.ADMIN_PACKET_ADMIN_UPDATE_FREQUENCY);
            p.writeUint16((int)type);
            p.writeUint16((int)freq);

            NetworkOutputThread.append(p);
        }

        public void sendAdminPoll(AdminUpdateType type, long data = 0)
        {
            if (getProtocol().isSupported(type, AdminUpdateFrequency.ADMIN_FREQUENCY_POLL) == false)
                throw new ArgumentException("The server does not support polling for " + type);

            Packet p = new Packet(getSocket(), PacketType.ADMIN_PACKET_ADMIN_POLL);
            p.writeUint8((short)type);
            p.writeUint32(data);

            NetworkOutputThread.append(p);
        }

        #endregion

        #region Receive Packets
        public void receiveServerClientInfo(OpenTTD openttd, Packet p)
        {
            Client client = new Client(p.readUint32());

            client.address = p.readString();
            client.name = p.readString();
            //client.language = NetworkLanguage.valueOf(p.readUint8());
            p.readUint8();
            client.joindate = new GameDate(p.readUint32());
            client.companyId = p.readUint8();

            openttd.clientPool.Add(client.clientId, client);

            if (OnClientInfo != null)
                OnClientInfo(client);
        }

        public void receiveServerProtocol(OpenTTD openttd, Packet p)
        {
            Protocol protocol = getProtocol();

            protocol.version = p.readUint8();

            while (p.readBool())
            {
                int tIndex = p.readUint16();
                int fValues = p.readUint16();

                foreach (AdminUpdateFrequency freq in Enum.GetValues(typeof(AdminUpdateFrequency)))
                {
                    int index = fValues & (int)freq;

                    if (index != 0)
                    {
                        protocol.addSupport(tIndex, (int)freq);
                    }
                }
            }

            if (OnProtocol != null)
                OnProtocol(protocol);
        }

        public void receiveServerWelcome(OpenTTD openttd, Packet p)
        {
            Map map = new Map();
            
            Game game = new Game();
            
            game.name = p.readString();
            game.versionGame = p.readString();
            game.dedicated = p.readBool();

            map.name = p.readString();
            map.seed = p.readUint32();
            map.landscape = (Landscape) p.readUint8();
            map.dateStart = new GameDate(p.readUint32());
            map.width = p.readUint16();
            map.height = p.readUint16();

            game.map = map;

            if (OnServerWelcome != null)
                OnServerWelcome();
            
        }

        public void receiveServerChat(OpenTTD openttd, Packet p)
        {
            NetworkAction action = (NetworkAction) p.readUint8();
            DestType dest = (DestType) p.readUint8();
            long clientId = p.readUint32();
            String message = p.readString();
            long data = p.readUint64();

            if(OnChat != null)
                OnChat(action, dest, clientId, message, data);
            
        }

        public void receiveServerCmdNames(OpenTTD openttd, Packet p)
        {
            while (p.readBool())
            {
                int cmdId = p.readUint16();
                String cmdName = p.readString();

                DoCommandName.enumeration.Add(cmdName, cmdId);
            }
        }

        public void receiveServerCmdLogging(OpenTTD openttd, Packet p)
        {

        }
        #endregion

        #region Getters
        public OpenTTD getOpenTTD()
        {
            return this.openttd;
        }

        public Socket getSocket()
        {
            return this.socket;
        }

        public Protocol getProtocol()
        {
            return this.protocol;
        }
        #endregion

    }
}
