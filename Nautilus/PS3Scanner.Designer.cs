namespace Nautilus
{
    partial class PS3Scanner
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.chkListing = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numWait = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numTries = new System.Windows.Forms.NumericUpDown();
            this.radioPAL = new System.Windows.Forms.RadioButton();
            this.radioNTSC = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numTimeOut = new System.Windows.Forms.NumericUpDown();
            this.ipAddress = new IPAddressControlLib.IPAddressControl();
            this.btnFind = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkDetailed = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeOut)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picWorking
            // 
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(277, 242);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 59;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(393, 39);
            this.label1.TabIndex = 60;
            this.label1.Text = "Enter your PS3\'s IP address and port number, then click Begin to search for songs" +
    "\r\n\r\nTHIS MIGHT TAKE A LONG TIME. HAVE PATIENCE :-)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "PS3 IP Address:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "PS3 FTP Port Number:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(171, 39);
            this.numPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(120, 20);
            this.numPort.TabIndex = 1;
            this.toolTip1.SetToolTip(this.numPort, "Enter your PS3\'s FTP Port Number here");
            this.numPort.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.chkListing);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.numWait);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.numTries);
            this.panel1.Controls.Add(this.radioPAL);
            this.panel1.Controls.Add(this.radioNTSC);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.numTimeOut);
            this.panel1.Controls.Add(this.ipAddress);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.numPort);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(15, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 161);
            this.panel1.TabIndex = 64;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(327, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 39);
            this.label8.TabIndex = 75;
            this.label8.Text = "Create\r\nDTA\r\nListing";
            // 
            // chkListing
            // 
            this.chkListing.AutoSize = true;
            this.chkListing.Checked = true;
            this.chkListing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkListing.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkListing.Location = new System.Drawing.Point(340, 94);
            this.chkListing.Name = "chkListing";
            this.chkListing.Size = new System.Drawing.Size(15, 14);
            this.chkListing.TabIndex = 74;
            this.chkListing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkListing, "Check to create a separate CSV file with the location of all your songs.dta files" +
        "");
            this.chkListing.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 13);
            this.label7.TabIndex = 73;
            this.label7.Text = "Wait time between tries (ms):";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numWait
            // 
            this.numWait.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numWait.Location = new System.Drawing.Point(171, 130);
            this.numWait.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWait.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numWait.Name = "numWait";
            this.numWait.Size = new System.Drawing.Size(120, 20);
            this.numWait.TabIndex = 4;
            this.toolTip1.SetToolTip(this.numWait, "Enter the time to wait between tries in milliseconds");
            this.numWait.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 71;
            this.label6.Text = "Maximum amount of tries:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTries
            // 
            this.numTries.Location = new System.Drawing.Point(171, 99);
            this.numTries.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTries.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTries.Name = "numTries";
            this.numTries.Size = new System.Drawing.Size(120, 20);
            this.numTries.TabIndex = 3;
            this.toolTip1.SetToolTip(this.numTries, "Enter the maximum amount of tries before skipping a file");
            this.numTries.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // radioPAL
            // 
            this.radioPAL.AutoSize = true;
            this.radioPAL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioPAL.Location = new System.Drawing.Point(320, 54);
            this.radioPAL.Name = "radioPAL";
            this.radioPAL.Size = new System.Drawing.Size(45, 17);
            this.radioPAL.TabIndex = 6;
            this.radioPAL.Text = "PAL";
            this.toolTip1.SetToolTip(this.radioPAL, "Select this if you\'re using a PAL console");
            this.radioPAL.UseVisualStyleBackColor = true;
            // 
            // radioNTSC
            // 
            this.radioNTSC.AutoSize = true;
            this.radioNTSC.Checked = true;
            this.radioNTSC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioNTSC.Location = new System.Drawing.Point(320, 32);
            this.radioNTSC.Name = "radioNTSC";
            this.radioNTSC.Size = new System.Drawing.Size(54, 17);
            this.radioNTSC.TabIndex = 5;
            this.radioNTSC.TabStop = true;
            this.radioNTSC.Text = "NTSC";
            this.toolTip1.SetToolTip(this.radioNTSC, "Select this if you\'re using a NTSC console");
            this.radioNTSC.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(324, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 67;
            this.label5.Text = "Region";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Connection Timeout (ms):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTimeOut
            // 
            this.numTimeOut.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTimeOut.Location = new System.Drawing.Point(171, 69);
            this.numTimeOut.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numTimeOut.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numTimeOut.Name = "numTimeOut";
            this.numTimeOut.Size = new System.Drawing.Size(120, 20);
            this.numTimeOut.TabIndex = 2;
            this.toolTip1.SetToolTip(this.numTimeOut, "Enter the connection timeout in milliseconds");
            this.numTimeOut.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // ipAddress
            // 
            this.ipAddress.AllowInternalTab = false;
            this.ipAddress.AutoHeight = true;
            this.ipAddress.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddress.Location = new System.Drawing.Point(171, 9);
            this.ipAddress.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.ReadOnly = false;
            this.ipAddress.Size = new System.Drawing.Size(120, 20);
            this.ipAddress.TabIndex = 0;
            this.ipAddress.Text = "192.168.0.1";
            this.toolTip1.SetToolTip(this.ipAddress, "Enter your PS3\'s IP Address here");
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.Aquamarine;
            this.btnFind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Location = new System.Drawing.Point(15, 238);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(100, 23);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Begin scanning...";
            this.toolTip1.SetToolTip(this.btnFind, "Click to start searching...");
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // chkDetailed
            // 
            this.chkDetailed.AutoSize = true;
            this.chkDetailed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDetailed.Location = new System.Drawing.Point(121, 244);
            this.chkDetailed.Name = "chkDetailed";
            this.chkDetailed.Size = new System.Drawing.Size(102, 17);
            this.chkDetailed.TabIndex = 68;
            this.chkDetailed.Text = "Detailed logging";
            this.toolTip1.SetToolTip(this.chkDetailed, "Check to have a much more detailed (but long!) log");
            this.chkDetailed.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // lstLog
            // 
            this.lstLog.AllowDrop = true;
            this.lstLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLog.BackColor = System.Drawing.Color.White;
            this.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLog.ContextMenuStrip = this.contextMenuStrip1;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Items.AddRange(new object[] {
            "Welcome to PS3 Scanner"});
            this.lstLog.Location = new System.Drawing.Point(15, 271);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(390, 119);
            this.lstLog.TabIndex = 67;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportLogFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 26);
            // 
            // exportLogFileToolStripMenuItem
            // 
            this.exportLogFileToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exportLogFileToolStripMenuItem.Name = "exportLogFileToolStripMenuItem";
            this.exportLogFileToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.exportLogFileToolStripMenuItem.Text = "Export log file";
            this.exportLogFileToolStripMenuItem.Click += new System.EventHandler(this.exportLogFileToolStripMenuItem_Click);
            // 
            // PS3Scanner
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(420, 405);
            this.Controls.Add(this.chkDetailed);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picWorking);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PS3Scanner";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PS3 Scanner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PS3Scanner_FormClosing);
            this.Shown += new System.EventHandler(this.PS3Scanner_Shown);
            this.Resize += new System.EventHandler(this.PS3Scanner_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeOut)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private IPAddressControlLib.IPAddressControl ipAddress;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportLogFileToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown numTimeOut;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioPAL;
        private System.Windows.Forms.RadioButton radioNTSC;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numTries;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numWait;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkListing;
        private System.Windows.Forms.CheckBox chkDetailed;
    }
}