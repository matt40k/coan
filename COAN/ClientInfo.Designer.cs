namespace COAN
{
    partial class ClientInfo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wLblName = new System.Windows.Forms.Label();
            this.wLblID = new System.Windows.Forms.Label();
            this.wLblComp = new System.Windows.Forms.Label();
            this.wLblIP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // wLblName
            // 
            this.wLblName.AutoSize = true;
            this.wLblName.Location = new System.Drawing.Point(90, 14);
            this.wLblName.Name = "wLblName";
            this.wLblName.Size = new System.Drawing.Size(45, 17);
            this.wLblName.TabIndex = 0;
            this.wLblName.Text = "Name";
            // 
            // wLblID
            // 
            this.wLblID.AutoSize = true;
            this.wLblID.Location = new System.Drawing.Point(26, 14);
            this.wLblID.Name = "wLblID";
            this.wLblID.Size = new System.Drawing.Size(21, 17);
            this.wLblID.TabIndex = 1;
            this.wLblID.Text = "ID";
            // 
            // wLblComp
            // 
            this.wLblComp.AutoSize = true;
            this.wLblComp.Location = new System.Drawing.Point(206, 14);
            this.wLblComp.Name = "wLblComp";
            this.wLblComp.Size = new System.Drawing.Size(67, 17);
            this.wLblComp.TabIndex = 2;
            this.wLblComp.Text = "Company";
            // 
            // wLblIP
            // 
            this.wLblIP.AutoSize = true;
            this.wLblIP.Location = new System.Drawing.Point(350, 14);
            this.wLblIP.Name = "wLblIP";
            this.wLblIP.Size = new System.Drawing.Size(20, 17);
            this.wLblIP.TabIndex = 3;
            this.wLblIP.Text = "IP";
            // 
            // ClientInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wLblIP);
            this.Controls.Add(this.wLblComp);
            this.Controls.Add(this.wLblID);
            this.Controls.Add(this.wLblName);
            this.Name = "ClientInfo";
            this.Size = new System.Drawing.Size(547, 43);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label wLblName;
        private System.Windows.Forms.Label wLblID;
        private System.Windows.Forms.Label wLblComp;
        private System.Windows.Forms.Label wLblIP;
    }
}
