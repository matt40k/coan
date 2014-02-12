using System;
using System.Windows.Forms;
using System.Collections.Generic;
namespace COAN
{
    /// <summary>
    /// The OpenTTD Admin Network Bot Framework
    /// Extend this class and implement the methods you wish to make use of.
    /// </summary>
    public class OpenTTD
    {
        public string botName = "Bot Name";
        public string botVersion = "BOT VERSION";

        public string adminHost = "";
        public string adminPassword = "";
        public int adminPort = 3978;

        public Dictionary<long, Client> clientPool = new Dictionary<long, Client>();

        public NetworkClient networkClient;

        public OpenTTD()
        {
            this.networkClient = new NetworkClient(this);
        }

        public string getPassword()
        {
            return this.adminPassword;
        }

        public string getBotName()
        {
            return this.botName;
        }

        public string getBotVersion()
        {
            return this.botVersion;
        }

        public void Connect(string hostname, int port, string password)
        {
            this.adminHost = hostname;
            this.adminPort = port;
            this.adminPassword = password;
            this.Connect();
        }

        public void Connect()
        {
            networkClient.Connect(this.adminHost, this.adminPort);
            networkClient.Start();
        }

        public void registerUpdateFrequency(AdminUpdateType type, AdminUpdateFrequency freq)
        {
            networkClient.sendAdminUpdateFrequency(type, freq);
        }

        public void pollAll()
        {
            networkClient.pollCmdNames();
            networkClient.pollDate();
            networkClient.pollClientInfos();
            networkClient.pollCompanyInfos();
            networkClient.pollCompanyStats();
            networkClient.pollCompanyEconomy();
        }

        public void chatPublic(string msg)
        {
            networkClient.sendAdminChat(enums.NetworkAction.NETWORK_ACTION_CHAT, enums.DestType.DESTTYPE_BROADCAST, 0, msg, 0);
        }

        public void onProtocol(Protocol protocol) { }

        public void onServerWelcome()
        {
            /* register for console updates */
            this.registerUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_CONSOLE, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            /* register for client information */
            this.registerUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            /* register for chat as it happens */
            this.registerUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_CHAT, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            this.registerUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_COMPANY_ECONOMY, AdminUpdateFrequency.ADMIN_FREQUENCY_WEEKLY);
            this.registerUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_COMPANY_STATS, AdminUpdateFrequency.ADMIN_FREQUENCY_WEEKLY);
            this.registerUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_CMD_LOGGING, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            this.registerUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_GAMESCRIPT, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            pollAll();
        }

    }
}