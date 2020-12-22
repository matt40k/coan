using System;
using System.Windows.Forms;
using NLog;

namespace COAN
{
    public partial class Form1 : Form
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        private Config _config;
        readonly NetworkClient networkClient = new NetworkClient();

        private void button1_Click(object sender, EventArgs e)
        {
            if (networkClient.IsConnected)
            {
                logger.Log(LogLevel.Trace, "IsConnected - User clicked disconnect");
                networkClient.Disconnect();
            }
            else
            {
                logger.Log(LogLevel.Trace, "IsNotConnected - User clicked connect");
                networkClient.Connect(wTextHost.Text, int.Parse(wTextPort.Text), wTextPassword.Text);
                networkClient.OnChat += new NetworkClient.onChat(networkClient_OnChat);
                networkClient.OnServerWelcome += new NetworkClient.onWelcome(onServerWelcome);
            }
            if (networkClient.IsConnected)
            {
                button1.Text = "Disconnect";
                buttonSendMessage.Enabled = true;
            }
            else
            {
                button1.Text = "Connect";
                buttonSendMessage.Enabled = false;
            }
        }

        void networkClient_OnChat(NetworkAction action, DestType dest, long clientId, string message, long data)
        {
            if (textBox2.InvokeRequired == true)
            {
                textBox2.Invoke(
                    (MethodInvoker)delegate
                    {
                        textBox2.AppendText(Convert.ToString(networkClient.GetClient(clientId).name));
                        textBox2.AppendText(" said: " + message);
                        textBox2.AppendText(Environment.NewLine);
                    });
            }
        }

        public void Chat(string msg)
        {
            textBox2.AppendText("YOU said: " + msg);
            textBox2.AppendText(Environment.NewLine);
            networkClient.chatPublic(msg);
        }

        private void buttonSendMessage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textChatToSend.Text))
            {
                logger.Log(LogLevel.Trace, string.Format("User click send message - {0}", textChatToSend.Text));
                Chat(textChatToSend.Text);
                textChatToSend.Text = "";
            }
        }

        public Form1()
        {
            InitializeComponent();

            labelTitle.Text = Info.Title;
            labelDescription.Text = Info.Description;
            labelVersion.Text = string.Format("Version: {0}", Info.Version);

            _config = new Config();
            var defaultHost = _config.GetDefaultHost;
            var defaultPort = _config.GetDefaultPort.ToString();
            var defaultPassword = _config.GetDefaultPassword;

            if (!string.IsNullOrWhiteSpace(defaultHost))
                wTextHost.Text = defaultHost;
            if (!string.IsNullOrWhiteSpace(defaultPort))
                wTextPort.Text = defaultPort;
            if (!string.IsNullOrWhiteSpace(defaultPassword))
                wTextPassword.Text = defaultPassword;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                buttonSendMessage_Click(sender, e);
            }
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
