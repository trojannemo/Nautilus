namespace Nautilus
{
    partial class MoggMaker
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
            this.btnBegin = new System.Windows.Forms.Button();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstAudio = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSilent = new System.Windows.Forms.Button();
            this.chkEncrypt = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chkAnalyzer = new System.Windows.Forms.CheckBox();
            this.chkOverride = new System.Windows.Forms.CheckBox();
            this.cboFreq = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboQuality = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.picPin = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.SuspendLayout();
            // 
            // picWorking
            // 
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(247, 246);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 63;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // btnBegin
            // 
            this.btnBegin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))), ((int)(((byte)(51)))));
            this.btnBegin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBegin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBegin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBegin.ForeColor = System.Drawing.Color.Black;
            this.btnBegin.Location = new System.Drawing.Point(505, 206);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(97, 30);
            this.btnBegin.TabIndex = 62;
            this.btnBegin.Text = "&Make Mogg";
            this.btnBegin.UseVisualStyleBackColor = false;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // lstLog
            // 
            this.lstLog.BackColor = System.Drawing.Color.White;
            this.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLog.ContextMenuStrip = this.contextMenuStrip1;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Location = new System.Drawing.Point(12, 270);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(590, 210);
            this.lstLog.TabIndex = 61;
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
            // lstAudio
            // 
            this.lstAudio.AllowDrop = true;
            this.lstAudio.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lstAudio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAudio.FormattingEnabled = true;
            this.lstAudio.HorizontalScrollbar = true;
            this.lstAudio.Location = new System.Drawing.Point(12, 42);
            this.lstAudio.Name = "lstAudio";
            this.lstAudio.Size = new System.Drawing.Size(452, 158);
            this.lstAudio.TabIndex = 64;
            this.lstAudio.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstAudio_DragDrop);
            this.lstAudio.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstAudio_DragEnter);
            this.lstAudio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstAudio_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(529, 23);
            this.label1.TabIndex = 65;
            this.label1.Text = "Drag && Drop audio files below in the order you want them in the Mogg file";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))), ((int)(((byte)(51)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(12, 206);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 30);
            this.btnClear.TabIndex = 66;
            this.btnClear.Text = "&Clear Audio Files";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSilent
            // 
            this.btnSilent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))), ((int)(((byte)(51)))));
            this.btnSilent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSilent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSilent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSilent.ForeColor = System.Drawing.Color.Black;
            this.btnSilent.Location = new System.Drawing.Point(139, 206);
            this.btnSilent.Name = "btnSilent";
            this.btnSilent.Size = new System.Drawing.Size(120, 30);
            this.btnSilent.TabIndex = 67;
            this.btnSilent.Text = "Add &Silent Track";
            this.btnSilent.UseVisualStyleBackColor = false;
            this.btnSilent.Click += new System.EventHandler(this.btnSilent_Click);
            // 
            // chkEncrypt
            // 
            this.chkEncrypt.AutoSize = true;
            this.chkEncrypt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkEncrypt.Location = new System.Drawing.Point(6, 109);
            this.chkEncrypt.Name = "chkEncrypt";
            this.chkEncrypt.Size = new System.Drawing.Size(92, 17);
            this.chkEncrypt.TabIndex = 68;
            this.chkEncrypt.Text = "Encrypt Mogg";
            this.chkEncrypt.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // chkAnalyzer
            // 
            this.chkAnalyzer.AutoSize = true;
            this.chkAnalyzer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkAnalyzer.Location = new System.Drawing.Point(6, 133);
            this.chkAnalyzer.Name = "chkAnalyzer";
            this.chkAnalyzer.Size = new System.Drawing.Size(106, 17);
            this.chkAnalyzer.TabIndex = 69;
            this.chkAnalyzer.Text = "Send to Analyzer";
            this.chkAnalyzer.UseVisualStyleBackColor = true;
            // 
            // chkOverride
            // 
            this.chkOverride.AutoSize = true;
            this.chkOverride.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkOverride.Location = new System.Drawing.Point(6, 61);
            this.chkOverride.Name = "chkOverride";
            this.chkOverride.Size = new System.Drawing.Size(112, 17);
            this.chkOverride.TabIndex = 70;
            this.chkOverride.Text = "Override Freq (Hz)";
            this.chkOverride.UseVisualStyleBackColor = true;
            this.chkOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
            // 
            // cboFreq
            // 
            this.cboFreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFreq.Enabled = false;
            this.cboFreq.FormattingEnabled = true;
            this.cboFreq.Items.AddRange(new object[] {
            "16000",
            "22050",
            "32000",
            "44100",
            "48000"});
            this.cboFreq.Location = new System.Drawing.Point(6, 80);
            this.cboFreq.Name = "cboFreq";
            this.cboFreq.Size = new System.Drawing.Size(72, 21);
            this.cboFreq.TabIndex = 71;
            this.cboFreq.SelectedIndexChanged += new System.EventHandler(this.cboFreq_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboQuality);
            this.groupBox1.Controls.Add(this.chkAnalyzer);
            this.groupBox1.Controls.Add(this.cboFreq);
            this.groupBox1.Controls.Add(this.chkOverride);
            this.groupBox1.Controls.Add(this.chkEncrypt);
            this.groupBox1.Location = new System.Drawing.Point(470, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(132, 157);
            this.groupBox1.TabIndex = 72;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 73;
            this.label2.Text = "Encoding Quality:";
            // 
            // cboQuality
            // 
            this.cboQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuality.FormattingEnabled = true;
            this.cboQuality.Items.AddRange(new object[] {
            "-1",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cboQuality.Location = new System.Drawing.Point(6, 32);
            this.cboQuality.Name = "cboQuality";
            this.cboQuality.Size = new System.Drawing.Size(72, 21);
            this.cboQuality.TabIndex = 72;
            this.cboQuality.SelectedIndexChanged += new System.EventHandler(this.cboQuality_SelectedIndexChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(265, 223);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 13);
            this.lblInfo.TabIndex = 73;
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(582, 12);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 74;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // MoggMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(614, 492);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnSilent);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstAudio);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.btnBegin);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MoggMaker";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mogg Maker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MoggMaker_FormClosing);
            this.Shown += new System.EventHandler(this.MoggMaker_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.ListBox lstAudio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSilent;
        private System.Windows.Forms.CheckBox chkEncrypt;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportLogFileToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox chkAnalyzer;
        private System.Windows.Forms.CheckBox chkOverride;
        private System.Windows.Forms.ComboBox cboFreq;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ComboBox cboQuality;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picPin;
    }
}