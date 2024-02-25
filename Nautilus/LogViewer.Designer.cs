namespace Nautilus
{
    partial class LogViewer
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportLogTool = new System.Windows.Forms.ToolStripMenuItem();
            this.exitTool = new System.Windows.Forms.ToolStripMenuItem();
            this.lstLogs = new System.Windows.Forms.ListView();
            this.colNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDidPlay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAdded = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPlayed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStopped = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSkipped = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeleted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cboLogs = new System.Windows.Forms.ComboBox();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.lblStats = new System.Windows.Forms.Label();
            this.picPin = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(722, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportLogTool,
            this.exitTool});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportLogTool
            // 
            this.exportLogTool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exportLogTool.Enabled = false;
            this.exportLogTool.Name = "exportLogTool";
            this.exportLogTool.Size = new System.Drawing.Size(169, 22);
            this.exportLogTool.Text = "Export Log to CSV";
            this.exportLogTool.Click += new System.EventHandler(this.exportLogTool_Click);
            // 
            // exitTool
            // 
            this.exitTool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exitTool.Name = "exitTool";
            this.exitTool.Size = new System.Drawing.Size(169, 22);
            this.exitTool.Text = "Exit";
            this.exitTool.Click += new System.EventHandler(this.exitTool_Click);
            // 
            // lstLogs
            // 
            this.lstLogs.AllowColumnReorder = true;
            this.lstLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNumber,
            this.colBand,
            this.colSong,
            this.colArtist,
            this.colDidPlay,
            this.colAdded,
            this.colPlayed,
            this.colStopped,
            this.colSkipped,
            this.colDeleted});
            this.lstLogs.FullRowSelect = true;
            this.lstLogs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstLogs.HideSelection = false;
            this.lstLogs.Location = new System.Drawing.Point(12, 54);
            this.lstLogs.MultiSelect = false;
            this.lstLogs.Name = "lstLogs";
            this.lstLogs.Size = new System.Drawing.Size(698, 377);
            this.lstLogs.TabIndex = 1;
            this.lstLogs.UseCompatibleStateImageBehavior = false;
            this.lstLogs.View = System.Windows.Forms.View.Details;
            // 
            // colNumber
            // 
            this.colNumber.Text = "#";
            this.colNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colNumber.Width = 25;
            // 
            // colBand
            // 
            this.colBand.Text = "Band";
            this.colBand.Width = 190;
            // 
            // colSong
            // 
            this.colSong.Text = "Song";
            this.colSong.Width = 190;
            // 
            // colArtist
            // 
            this.colArtist.Text = "Artist";
            this.colArtist.Width = 190;
            // 
            // colDidPlay
            // 
            this.colDidPlay.Text = "Performed?";
            this.colDidPlay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDidPlay.Width = 70;
            // 
            // colAdded
            // 
            this.colAdded.Text = "Time Added";
            this.colAdded.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colAdded.Width = 100;
            // 
            // colPlayed
            // 
            this.colPlayed.Text = "Start Time";
            this.colPlayed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colPlayed.Width = 100;
            // 
            // colStopped
            // 
            this.colStopped.Text = "Stop Time";
            this.colStopped.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colStopped.Width = 100;
            // 
            // colSkipped
            // 
            this.colSkipped.Text = "Skipped?";
            this.colSkipped.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colDeleted
            // 
            this.colDeleted.Text = "Deleted?";
            this.colDeleted.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cboLogs
            // 
            this.cboLogs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboLogs.FormattingEnabled = true;
            this.cboLogs.Location = new System.Drawing.Point(12, 27);
            this.cboLogs.Name = "cboLogs";
            this.cboLogs.Size = new System.Drawing.Size(196, 21);
            this.cboLogs.TabIndex = 3;
            this.cboLogs.Text = "Click to select a log file ...";
            this.cboLogs.SelectedIndexChanged += new System.EventHandler(this.cboLogs_SelectedIndexChanged);
            // 
            // picWorking
            // 
            this.picWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(308, 8);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 65;
            this.picWorking.TabStop = false;
            // 
            // lblStats
            // 
            this.lblStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStats.Location = new System.Drawing.Point(221, 27);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(488, 21);
            this.lblStats.TabIndex = 66;
            this.lblStats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(695, 6);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 67;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(722, 443);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.cboLogs);
            this.Controls.Add(this.lstLogs);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "LogViewer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log Viewer";
            this.Shown += new System.EventHandler(this.LogViewer_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportLogTool;
        private System.Windows.Forms.ToolStripMenuItem exitTool;
        private System.Windows.Forms.ListView lstLogs;
        private System.Windows.Forms.ComboBox cboLogs;
        private System.Windows.Forms.ColumnHeader colBand;
        private System.Windows.Forms.ColumnHeader colSong;
        private System.Windows.Forms.ColumnHeader colArtist;
        private System.Windows.Forms.ColumnHeader colDidPlay;
        private System.Windows.Forms.ColumnHeader colAdded;
        private System.Windows.Forms.ColumnHeader colPlayed;
        private System.Windows.Forms.ColumnHeader colStopped;
        private System.Windows.Forms.ColumnHeader colSkipped;
        private System.Windows.Forms.ColumnHeader colDeleted;
        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.ColumnHeader colNumber;
        private System.Windows.Forms.PictureBox picPin;
    }
}