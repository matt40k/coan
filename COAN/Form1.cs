using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using NLog;

namespace COAN
{
    public partial class Form1 : Form
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Config _config;
        private DataTable ClientDataTable;
        private readonly NetworkClient networkClient = new NetworkClient();

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

            ClientDataTable = CreateClientDataTable;
            // AddExampleClient();
        }

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
                networkClient.OnChat += new NetworkClient.onChat(NetworkClient_OnChat);
                networkClient.OnClientInfo += new NetworkClient.onClientInfo(NetworkClient_OnClientInfo);
                networkClient.OnServerWelcome += new NetworkClient.onWelcome(OnServerWelcome);
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

        public void NetworkClient_OnChat(NetworkAction action, DestType dest, long clientId, string message, long data)
        {
            if (textBox2.InvokeRequired)
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

        public void NetworkClient_OnClientInfo(Client client)
        {
            // Create row from the client message
            var newrow = CreateClientDataTable.NewRow();
            newrow["ID"] = (string)client.clientId.ToString();
            newrow["Name"] = (string)client.name;
            newrow["Company"] = (string)client.companyId.ToString();
            newrow["IP"] = (string)client.address;

            CreateClientDataTable.Rows.Add(newrow);
            this.clientsDg.DataSource = CreateClientDataTable;
        }

        public void Chat(string msg)
        {
            textBox2.AppendText("YOU said: " + msg);
            textBox2.AppendText(Environment.NewLine);
            networkClient.ChatPublic(msg);
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                buttonSendMessage_Click(sender, e);
            }
        }


        public void RegisterUpdateFrequency(AdminUpdateType type, AdminUpdateFrequency freq)
        {
            logger.Log(LogLevel.Trace, "registerUpdateFrequency");
            networkClient.SendAdminUpdateFrequency(type, freq);
        }
        
        public void PollAll()
        {
            logger.Log(LogLevel.Trace, "pollAll");
            networkClient.PollCmdNames();
            networkClient.PollDate();
            networkClient.PollClientInfos();
            networkClient.PollCompanyInfos();
            networkClient.PollCompanyStats();
            networkClient.PollCompanyEconomy();
        }
        
        public void OnServerWelcome()
        {
            /* register for console updates */
            this.RegisterUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_CONSOLE, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            /* register for client information */
            this.RegisterUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            /* register for chat as it happens */
            this.RegisterUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_CHAT, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            this.RegisterUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_COMPANY_ECONOMY, AdminUpdateFrequency.ADMIN_FREQUENCY_WEEKLY);
            this.RegisterUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_COMPANY_STATS, AdminUpdateFrequency.ADMIN_FREQUENCY_WEEKLY);
            this.RegisterUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_CMD_LOGGING, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            this.RegisterUpdateFrequency(AdminUpdateType.ADMIN_UPDATE_GAMESCRIPT, AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC);
            PollAll();
        }

        public DataTable CreateClientDataTable
        {
            get
            {
                // Create DataTable
                var _clientdt = new DataTable();
                _clientdt.Columns.Add(new DataColumn("ID", typeof(string)));
                _clientdt.Columns.Add(new DataColumn("Name", typeof(string)));
                _clientdt.Columns.Add(new DataColumn("Company", typeof(string)));
                _clientdt.Columns.Add(new DataColumn("IP", typeof(string)));
                return _clientdt;
            }
        }

        private void AddExampleClient()
        {
            CreateClientDataTable.Rows.Clear();
            var newrow = CreateClientDataTable.NewRow();
            newrow["ID"] = "-1";
            newrow["Name"] = "Example";
            newrow["Company"] = "Dummy Inc";
            newrow["IP"] = "192.168.0.100";
            CreateClientDataTable.Rows.Add(newrow);

            this.clientsDg.DataSource = CreateClientDataTable;
        }

        private void buttonLogs_Click(object sender, EventArgs e)
        {
            try
            {
                Process prc = new Process();
                prc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\OpenTTD\\coan";
                prc.Start();
            }
            catch (Exception openLog_Ex)
            {
                logger.Log(LogLevel.Error, openLog_Ex.Message);
            }
        }
    }
}
