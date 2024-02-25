namespace Nautilus
{
    partial class RBAConverter
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnFolder = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBegin = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picPin = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.lIVECONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LIVEtoCON = new System.Windows.Forms.ToolStripMenuItem();
            this.CONtoLIVE = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.artistSongTool = new System.Windows.Forms.ToolStripMenuItem();
            this.songTool = new System.Windows.Forms.ToolStripMenuItem();
            this.songByArtistTool = new System.Windows.Forms.ToolStripMenuItem();
            this.packageThumbnail = new System.Windows.Forms.ToolStripMenuItem();
            this.albumArtTool = new System.Windows.Forms.ToolStripMenuItem();
            this.rB3IconTool = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(488, 32);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(114, 30);
            this.btnRefresh.TabIndex = 18;
            this.btnRefresh.Text = "&Refresh Folder";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Visible = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnRefresh.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnFolder
            // 
            this.btnFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            this.btnFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFolder.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFolder.ForeColor = System.Drawing.Color.White;
            this.btnFolder.Location = new System.Drawing.Point(12, 32);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(137, 30);
            this.btnFolder.TabIndex = 17;
            this.btnFolder.Text = "Select &Input Folder";
            this.btnFolder.UseVisualStyleBackColor = false;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            this.btnFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // txtFolder
            // 
            this.txtFolder.BackColor = System.Drawing.Color.White;
            this.txtFolder.Location = new System.Drawing.Point(12, 68);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(590, 20);
            this.txtFolder.TabIndex = 16;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            this.txtFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.txtFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // lstLog
            // 
            this.lstLog.AllowDrop = true;
            this.lstLog.BackColor = System.Drawing.Color.White;
            this.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLog.ContextMenuStrip = this.contextMenuStrip1;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Location = new System.Drawing.Point(12, 94);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(590, 210);
            this.lstLog.TabIndex = 19;
            this.lstLog.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.lstLog.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
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
            // btnBegin
            // 
            this.btnBegin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            this.btnBegin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBegin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBegin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBegin.ForeColor = System.Drawing.Color.White;
            this.btnBegin.Location = new System.Drawing.Point(543, 310);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(59, 30);
            this.btnBegin.TabIndex = 20;
            this.btnBegin.Text = "&Begin";
            this.toolTip1.SetToolTip(this.btnBegin, "Click to begin");
            this.btnBegin.UseVisualStyleBackColor = false;
            this.btnBegin.Visible = false;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            this.btnBegin.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnBegin.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(588, 5);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 64;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.toolTip1.SetToolTip(this.picPin, "Click to pin on top");
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // picWorking
            // 
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(252, 47);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 58;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lIVECONToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(614, 24);
            this.menuStrip1.TabIndex = 59;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // lIVECONToolStripMenuItem
            // 
            this.lIVECONToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LIVEtoCON,
            this.CONtoLIVE});
            this.lIVECONToolStripMenuItem.Name = "lIVECONToolStripMenuItem";
            this.lIVECONToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.lIVECONToolStripMenuItem.Text = "LIVE <-> CON";
            // 
            // LIVEtoCON
            // 
            this.LIVEtoCON.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.LIVEtoCON.Name = "LIVEtoCON";
            this.LIVEtoCON.Size = new System.Drawing.Size(215, 22);
            this.LIVEtoCON.Text = "Batch convert LIVE to CON";
            this.LIVEtoCON.Click += new System.EventHandler(this.LIVEtoCON_Click);
            // 
            // CONtoLIVE
            // 
            this.CONtoLIVE.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.CONtoLIVE.Name = "CONtoLIVE";
            this.CONtoLIVE.Size = new System.Drawing.Size(215, 22);
            this.CONtoLIVE.Text = "Batch convert CON to LIVE";
            this.CONtoLIVE.Click += new System.EventHandler(this.CONtoLIVE_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayTitle,
            this.packageThumbnail});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // displayTitle
            // 
            this.displayTitle.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.displayTitle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.artistSongTool,
            this.songTool,
            this.songByArtistTool});
            this.displayTitle.Name = "displayTitle";
            this.displayTitle.Size = new System.Drawing.Size(178, 22);
            this.displayTitle.Text = "Display Title";
            // 
            // artistSongTool
            // 
            this.artistSongTool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.artistSongTool.Checked = true;
            this.artistSongTool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.artistSongTool.Name = "artistSongTool";
            this.artistSongTool.Size = new System.Drawing.Size(158, 22);
            this.artistSongTool.Text = "Artist - Song";
            this.artistSongTool.Click += new System.EventHandler(this.artistSongTool_Click);
            // 
            // songTool
            // 
            this.songTool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.songTool.Name = "songTool";
            this.songTool.Size = new System.Drawing.Size(158, 22);
            this.songTool.Text = "\"Song\"";
            this.songTool.Click += new System.EventHandler(this.songTool_Click);
            // 
            // songByArtistTool
            // 
            this.songByArtistTool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.songByArtistTool.Name = "songByArtistTool";
            this.songByArtistTool.Size = new System.Drawing.Size(158, 22);
            this.songByArtistTool.Text = "\"Song\" by Artist";
            this.songByArtistTool.Click += new System.EventHandler(this.songByArtistTool_Click);
            // 
            // packageThumbnail
            // 
            this.packageThumbnail.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.packageThumbnail.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.albumArtTool,
            this.rB3IconTool});
            this.packageThumbnail.Name = "packageThumbnail";
            this.packageThumbnail.Size = new System.Drawing.Size(178, 22);
            this.packageThumbnail.Text = "Package Thumbnail";
            // 
            // albumArtTool
            // 
            this.albumArtTool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.albumArtTool.Checked = true;
            this.albumArtTool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.albumArtTool.Name = "albumArtTool";
            this.albumArtTool.Size = new System.Drawing.Size(129, 22);
            this.albumArtTool.Text = "Album Art";
            this.albumArtTool.Click += new System.EventHandler(this.albumArtTool_Click);
            // 
            // rB3IconTool
            // 
            this.rB3IconTool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rB3IconTool.Name = "rB3IconTool";
            this.rB3IconTool.Size = new System.Drawing.Size(129, 22);
            this.rB3IconTool.Text = "RB3 Icon";
            this.rB3IconTool.Click += new System.EventHandler(this.rB3IconTool_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // RBAConverter
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(614, 349);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.btnBegin);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.txtFolder);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "RBAConverter";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CON/RBA Converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RBAConverter_FormClosing);
            this.Shown += new System.EventHandler(this.RBAConverter_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportLogFileToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayTitle;
        private System.Windows.Forms.ToolStripMenuItem artistSongTool;
        private System.Windows.Forms.ToolStripMenuItem songTool;
        private System.Windows.Forms.ToolStripMenuItem songByArtistTool;
        private System.Windows.Forms.ToolStripMenuItem packageThumbnail;
        private System.Windows.Forms.ToolStripMenuItem albumArtTool;
        private System.Windows.Forms.ToolStripMenuItem rB3IconTool;
        private System.Windows.Forms.ToolStripMenuItem lIVECONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LIVEtoCON;
        private System.Windows.Forms.ToolStripMenuItem CONtoLIVE;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.PictureBox picPin;
    }
}