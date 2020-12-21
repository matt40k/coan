using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace COAN
{
    public partial class Form1 : Form
    {
        readonly NetworkClient networkClient = new NetworkClient();
        public Dictionary<long, Client> clientPool = new Dictionary<long, Client>();

        private void button1_Click(object sender, EventArgs e)
        {
            if (networkClient.IsConnected)
            {
                networkClient.Disconnect();
            }
            else
            {
                networkClient.Connect(wTextHost.Text, int.Parse(wTextPort.Text), wTextPassword.Text);
                networkClient.OnChat += new NetworkClient.onChat(networkClient_OnChat);
                networkClient.OnServerWelcome += new NetworkClient.onWelcome(onServerWelcome);
            }
            if (networkClient.IsConnected)
                button1.Text = "Disconnect";
            else
                button1.Text = "Connect";
        }

        void networkClient_OnChat(NetworkAction action, DestType dest, long clientId, string message, long data)
        {
            if (textBox2.InvokeRequired == true)
            {
                textBox2.Invoke(
                    (MethodInvoker)delegate
                    {
                        textBox2.AppendText(Convert.ToString(clientPool[clientId].name));
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

        private void button2_Click(object sender, EventArgs e)
        {
            Chat(textBox1.Text);
            textBox1.Text = "";
        }

        public Form1()
        {
            InitializeComponent();

            labelTitle.Text = Info.Title;
            labelDescription.Text = Info.Description;
            labelVersion.Text = string.Format("Version: {0}", Info.Version);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                button2_Click(sender, e);
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
