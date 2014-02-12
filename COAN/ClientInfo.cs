using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COAN
{
    public partial class ClientInfo : UserControl
    {
        public string IP
        {
            get { return wLblIP.Text; }
            set { wLblIP.Text = value; }
        }

        public string Company
        {
            get { return wLblComp.Text; }
            set { wLblComp.Text = value; }
        }

        public string Name
        {
            get { return wLblName.Text; }
            set { wLblName.Text = value; }
        }


        public ClientInfo()
        {
            InitializeComponent();
        }
    }
}
