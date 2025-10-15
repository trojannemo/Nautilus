namespace Nautilus
{
    partial class EventManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventManager));
            this.ShowOnForm = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowOnTitleBar = new System.Windows.Forms.ToolStripMenuItem();
            this.HeloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblClock = new System.Windows.Forms.Label();
            this.ClockOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.showEndTime = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEditPlay = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblNowPlayingBand = new System.Windows.Forms.Label();
            this.tmrClock = new System.Windows.Forms.Timer(this.components);
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picPin = new System.Windows.Forms.PictureBox();
            this.picDown = new System.Windows.Forms.PictureBox();
            this.picUp = new System.Windows.Forms.PictureBox();
            this.picRandom = new System.Windows.Forms.PictureBox();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.btnLoadSetlist = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblNowPlayingArtist = new System.Windows.Forms.Label();
            this.lblNowPlayingSong = new System.Windows.Forms.Label();
            this.grpNew = new System.Windows.Forms.GroupBox();
            this.cboAddBand = new System.Windows.Forms.ComboBox();
            this.btnSong = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblAddArtist = new System.Windows.Forms.Label();
            this.lblAddSong = new System.Windows.Forms.Label();
            this.lstSearch = new System.Windows.Forms.ListBox();
            this.txtAddArtist = new System.Windows.Forms.TextBox();
            this.txtAddSong = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblAddBand = new System.Windows.Forms.Label();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectSetlistFile = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSetlistDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.autosuggestSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.enableAutosuggest = new System.Windows.Forms.ToolStripMenuItem();
            this.editAutosuggestBands = new System.Windows.Forms.ToolStripMenuItem();
            this.editAutosuggestSongs = new System.Windows.Forms.ToolStripMenuItem();
            this.editAutosuggestArtists = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.lockSeverityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maximumSecurity = new System.Windows.Forms.ToolStripMenuItem();
            this.relaxedSecurity = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.resetSize = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpNext = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.lstNext = new System.Windows.Forms.ListBox();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelPrev = new System.Windows.Forms.Button();
            this.grpPrev = new System.Windows.Forms.GroupBox();
            this.chkAutoQueue = new System.Windows.Forms.CheckBox();
            this.btnEditPrev = new System.Windows.Forms.Button();
            this.btnDetails = new System.Windows.Forms.Button();
            this.lstPrevious = new System.Windows.Forms.ListBox();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.picLocked = new System.Windows.Forms.PictureBox();
            this.picUnlocked = new System.Windows.Forms.PictureBox();
            this.picMarquee = new System.Windows.Forms.PictureBox();
            this.lblEndTimeDesc = new System.Windows.Forms.Label();
            this.lblClockDesc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRandom)).BeginInit();
            this.grpNew.SuspendLayout();
            this.grpNext.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.grpPrev.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLocked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUnlocked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMarquee)).BeginInit();
            this.SuspendLayout();
            // 
            // ShowOnForm
            // 
            this.ShowOnForm.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ShowOnForm.Checked = true;
            this.ShowOnForm.CheckOnClick = true;
            this.ShowOnForm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowOnForm.Name = "ShowOnForm";
            this.ShowOnForm.Size = new System.Drawing.Size(208, 22);
            this.ShowOnForm.Text = "Show on form";
            this.ShowOnForm.Click += new System.EventHandler(this.ShowOnFormToolStripMenuItem_Click);
            // 
            // ShowOnTitleBar
            // 
            this.ShowOnTitleBar.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ShowOnTitleBar.CheckOnClick = true;
            this.ShowOnTitleBar.Name = "ShowOnTitleBar";
            this.ShowOnTitleBar.Size = new System.Drawing.Size(208, 22);
            this.ShowOnTitleBar.Text = "Show on title bar";
            this.ShowOnTitleBar.Click += new System.EventHandler(this.ShowOnTitleBarToolStripMenuItem_Click);
            // 
            // HeloToolStripMenuItem
            // 
            this.HeloToolStripMenuItem.Name = "HeloToolStripMenuItem";
            this.HeloToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HeloToolStripMenuItem.Text = "&Help";
            this.HeloToolStripMenuItem.Click += new System.EventHandler(this.HeloToolStripMenuItem_Click);
            // 
            // lblClock
            // 
            this.lblClock.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.lblClock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClock.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClock.ForeColor = System.Drawing.Color.White;
            this.lblClock.Location = new System.Drawing.Point(15, 63);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(218, 45);
            this.lblClock.TabIndex = 32;
            this.lblClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTip1.SetToolTip(this.lblClock, "Current Time");
            this.lblClock.VisibleChanged += new System.EventHandler(this.lblClock_VisibleChanged);
            this.lblClock.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblClock_MouseDown);
            this.lblClock.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblClock_MouseMove);
            this.lblClock.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblClock_MouseUp);
            // 
            // ClockOptionsToolStripMenuItem
            // 
            this.ClockOptionsToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClockOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowOnForm,
            this.ShowOnTitleBar,
            this.toolStripMenuItem1,
            this.showEndTime});
            this.ClockOptionsToolStripMenuItem.Name = "ClockOptionsToolStripMenuItem";
            this.ClockOptionsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.ClockOptionsToolStripMenuItem.Text = "Clock Options";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(205, 6);
            // 
            // showEndTime
            // 
            this.showEndTime.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.showEndTime.CheckOnClick = true;
            this.showEndTime.Name = "showEndTime";
            this.showEndTime.Size = new System.Drawing.Size(208, 22);
            this.showEndTime.Text = "Show estimated end time";
            // 
            // btnEditPlay
            // 
            this.btnEditPlay.BackColor = System.Drawing.Color.LightGray;
            this.btnEditPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditPlay.Enabled = false;
            this.btnEditPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditPlay.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditPlay.Location = new System.Drawing.Point(725, 305);
            this.btnEditPlay.Name = "btnEditPlay";
            this.btnEditPlay.Size = new System.Drawing.Size(103, 29);
            this.btnEditPlay.TabIndex = 30;
            this.btnEditPlay.Text = "Edit";
            this.btnEditPlay.UseVisualStyleBackColor = false;
            this.btnEditPlay.EnabledChanged += new System.EventHandler(this.btnEdit_EnabledChanged);
            this.btnEditPlay.Click += new System.EventHandler(this.btnEditPlay_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.BackColor = System.Drawing.Color.LightGray;
            this.btnSkip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSkip.Enabled = false;
            this.btnSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkip.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSkip.Location = new System.Drawing.Point(537, 305);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(103, 29);
            this.btnSkip.TabIndex = 27;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = false;
            this.btnSkip.EnabledChanged += new System.EventHandler(this.btnSkip_EnabledChanged);
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.LightGray;
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(908, 305);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(103, 29);
            this.btnStop.TabIndex = 26;
            this.btnStop.Text = "St&op";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.EnabledChanged += new System.EventHandler(this.btnDelete_EnabledChanged);
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblNowPlayingBand
            // 
            this.lblNowPlayingBand.BackColor = System.Drawing.Color.Transparent;
            this.lblNowPlayingBand.CausesValidation = false;
            this.lblNowPlayingBand.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNowPlayingBand.ForeColor = System.Drawing.Color.Black;
            this.lblNowPlayingBand.Location = new System.Drawing.Point(383, 98);
            this.lblNowPlayingBand.Name = "lblNowPlayingBand";
            this.lblNowPlayingBand.Size = new System.Drawing.Size(590, 60);
            this.lblNowPlayingBand.TabIndex = 22;
            this.lblNowPlayingBand.Text = "PERFORMER";
            this.lblNowPlayingBand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNowPlayingBand.UseMnemonic = false;
            // 
            // tmrClock
            // 
            this.tmrClock.Enabled = true;
            this.tmrClock.Interval = 1000;
            this.tmrClock.Tick += new System.EventHandler(this.tmrClock_Tick);
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(1252, 11);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 65;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.ToolTip1.SetToolTip(this.picPin, "Click to pin on top");
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // picDown
            // 
            this.picDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDown.Image = global::Nautilus.Properties.Resources.down;
            this.picDown.Location = new System.Drawing.Point(297, 73);
            this.picDown.Name = "picDown";
            this.picDown.Size = new System.Drawing.Size(27, 37);
            this.picDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDown.TabIndex = 9;
            this.picDown.TabStop = false;
            this.ToolTip1.SetToolTip(this.picDown, "Click to move down");
            this.picDown.Visible = false;
            this.picDown.Click += new System.EventHandler(this.picDown_Click);
            // 
            // picUp
            // 
            this.picUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picUp.Image = global::Nautilus.Properties.Resources.up;
            this.picUp.Location = new System.Drawing.Point(297, 30);
            this.picUp.Name = "picUp";
            this.picUp.Size = new System.Drawing.Size(27, 37);
            this.picUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUp.TabIndex = 8;
            this.picUp.TabStop = false;
            this.ToolTip1.SetToolTip(this.picUp, "Click to move up");
            this.picUp.Visible = false;
            this.picUp.Click += new System.EventHandler(this.picUp_Click);
            // 
            // picRandom
            // 
            this.picRandom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picRandom.BackColor = System.Drawing.Color.Transparent;
            this.picRandom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picRandom.Location = new System.Drawing.Point(285, 265);
            this.picRandom.Name = "picRandom";
            this.picRandom.Size = new System.Drawing.Size(30, 30);
            this.picRandom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRandom.TabIndex = 51;
            this.picRandom.TabStop = false;
            this.ToolTip1.SetToolTip(this.picRandom, "Pick a random winner");
            this.picRandom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picRandom_MouseClick);
            // 
            // lblEndTime
            // 
            this.lblEndTime.BackColor = System.Drawing.Color.RosyBrown;
            this.lblEndTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEndTime.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndTime.ForeColor = System.Drawing.Color.White;
            this.lblEndTime.Location = new System.Drawing.Point(15, 162);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(218, 45);
            this.lblEndTime.TabIndex = 66;
            this.lblEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTip1.SetToolTip(this.lblEndTime, "Estimated End Time");
            this.lblEndTime.Visible = false;
            this.lblEndTime.VisibleChanged += new System.EventHandler(this.lblEndTime_VisibleChanged);
            this.lblEndTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblClock_MouseDown);
            this.lblEndTime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblClock_MouseMove);
            this.lblEndTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblClock_MouseUp);
            // 
            // btnLoadSetlist
            // 
            this.btnLoadSetlist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSetlist.BackColor = System.Drawing.Color.White;
            this.btnLoadSetlist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadSetlist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadSetlist.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadSetlist.Location = new System.Drawing.Point(283, 115);
            this.btnLoadSetlist.Name = "btnLoadSetlist";
            this.btnLoadSetlist.Size = new System.Drawing.Size(30, 29);
            this.btnLoadSetlist.TabIndex = 18;
            this.btnLoadSetlist.TabStop = false;
            this.btnLoadSetlist.Text = "...";
            this.ToolTip1.SetToolTip(this.btnLoadSetlist, "Click to load Setlist file");
            this.btnLoadSetlist.UseVisualStyleBackColor = false;
            this.btnLoadSetlist.Click += new System.EventHandler(this.btnLoadSetlist_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LightGray;
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Enabled = false;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(349, 305);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 29);
            this.btnStart.TabIndex = 25;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.EnabledChanged += new System.EventHandler(this.btnPlay_EnabledChanged);
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblNowPlayingArtist
            // 
            this.lblNowPlayingArtist.BackColor = System.Drawing.Color.Transparent;
            this.lblNowPlayingArtist.CausesValidation = false;
            this.lblNowPlayingArtist.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNowPlayingArtist.ForeColor = System.Drawing.Color.Black;
            this.lblNowPlayingArtist.Location = new System.Drawing.Point(383, 207);
            this.lblNowPlayingArtist.Name = "lblNowPlayingArtist";
            this.lblNowPlayingArtist.Size = new System.Drawing.Size(590, 40);
            this.lblNowPlayingArtist.TabIndex = 24;
            this.lblNowPlayingArtist.Text = "ARTIST";
            this.lblNowPlayingArtist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNowPlayingArtist.UseMnemonic = false;
            // 
            // lblNowPlayingSong
            // 
            this.lblNowPlayingSong.BackColor = System.Drawing.Color.Transparent;
            this.lblNowPlayingSong.CausesValidation = false;
            this.lblNowPlayingSong.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNowPlayingSong.ForeColor = System.Drawing.Color.Black;
            this.lblNowPlayingSong.Location = new System.Drawing.Point(383, 149);
            this.lblNowPlayingSong.Name = "lblNowPlayingSong";
            this.lblNowPlayingSong.Size = new System.Drawing.Size(590, 58);
            this.lblNowPlayingSong.TabIndex = 23;
            this.lblNowPlayingSong.Text = "SONG TITLE";
            this.lblNowPlayingSong.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNowPlayingSong.UseMnemonic = false;
            // 
            // grpNew
            // 
            this.grpNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grpNew.BackColor = System.Drawing.Color.Transparent;
            this.grpNew.Controls.Add(this.cboAddBand);
            this.grpNew.Controls.Add(this.btnLoadSetlist);
            this.grpNew.Controls.Add(this.btnSong);
            this.grpNew.Controls.Add(this.txtSearch);
            this.grpNew.Controls.Add(this.lblAddArtist);
            this.grpNew.Controls.Add(this.lblAddSong);
            this.grpNew.Controls.Add(this.lstSearch);
            this.grpNew.Controls.Add(this.txtAddArtist);
            this.grpNew.Controls.Add(this.txtAddSong);
            this.grpNew.Controls.Add(this.btnAdd);
            this.grpNew.Controls.Add(this.lblAddBand);
            this.grpNew.ForeColor = System.Drawing.Color.Black;
            this.grpNew.Location = new System.Drawing.Point(6, 340);
            this.grpNew.Name = "grpNew";
            this.grpNew.Size = new System.Drawing.Size(330, 336);
            this.grpNew.TabIndex = 17;
            this.grpNew.TabStop = false;
            this.grpNew.Text = "New Act";
            // 
            // cboAddBand
            // 
            this.cboAddBand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAddBand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAddBand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cboAddBand.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAddBand.FormattingEnabled = true;
            this.cboAddBand.Location = new System.Drawing.Point(13, 51);
            this.cboAddBand.Name = "cboAddBand";
            this.cboAddBand.Size = new System.Drawing.Size(300, 29);
            this.cboAddBand.Sorted = true;
            this.cboAddBand.TabIndex = 0;
            this.cboAddBand.TextChanged += new System.EventHandler(this.cboAddBand_TextChanged);
            this.cboAddBand.Click += new System.EventHandler(this.cboAddBand_Click);
            this.cboAddBand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cboAddBand_KeyUp);
            // 
            // btnSong
            // 
            this.btnSong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSong.BackColor = System.Drawing.Color.LightGray;
            this.btnSong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSong.Enabled = false;
            this.btnSong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSong.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSong.Location = new System.Drawing.Point(15, 300);
            this.btnSong.Name = "btnSong";
            this.btnSong.Size = new System.Drawing.Size(103, 29);
            this.btnSong.TabIndex = 17;
            this.btnSong.Text = "Song Details";
            this.btnSong.UseVisualStyleBackColor = false;
            this.btnSong.EnabledChanged += new System.EventHandler(this.btnSong_EnabledChanged);
            this.btnSong.Click += new System.EventHandler(this.btnSong_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Enabled = false;
            this.txtSearch.ForeColor = System.Drawing.Color.Black;
            this.txtSearch.Location = new System.Drawing.Point(15, 90);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.Text = "Click to search Setlist...";
            this.txtSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSearch_MouseClick);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // lblAddArtist
            // 
            this.lblAddArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddArtist.BackColor = System.Drawing.Color.Transparent;
            this.lblAddArtist.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddArtist.Location = new System.Drawing.Point(15, 243);
            this.lblAddArtist.Name = "lblAddArtist";
            this.lblAddArtist.Size = new System.Drawing.Size(300, 21);
            this.lblAddArtist.TabIndex = 14;
            this.lblAddArtist.Text = "Artist";
            this.lblAddArtist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAddSong
            // 
            this.lblAddSong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddSong.BackColor = System.Drawing.Color.Transparent;
            this.lblAddSong.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddSong.Location = new System.Drawing.Point(15, 185);
            this.lblAddSong.Name = "lblAddSong";
            this.lblAddSong.Size = new System.Drawing.Size(300, 21);
            this.lblAddSong.TabIndex = 13;
            this.lblAddSong.Text = "Song Title";
            this.lblAddSong.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstSearch
            // 
            this.lstSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSearch.Enabled = false;
            this.lstSearch.FormattingEnabled = true;
            this.lstSearch.Location = new System.Drawing.Point(15, 113);
            this.lstSearch.Name = "lstSearch";
            this.lstSearch.Size = new System.Drawing.Size(300, 69);
            this.lstSearch.Sorted = true;
            this.lstSearch.TabIndex = 2;
            this.lstSearch.TabStop = false;
            this.lstSearch.SelectedIndexChanged += new System.EventHandler(this.lstSearch_SelectedIndexChanged);
            this.lstSearch.EnabledChanged += new System.EventHandler(this.lstSearch_EnabledChanged);
            this.lstSearch.DoubleClick += new System.EventHandler(this.lstSearch_DoubleClick);
            // 
            // txtAddArtist
            // 
            this.txtAddArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtAddArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtAddArtist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddArtist.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddArtist.ForeColor = System.Drawing.Color.Black;
            this.txtAddArtist.Location = new System.Drawing.Point(15, 265);
            this.txtAddArtist.Name = "txtAddArtist";
            this.txtAddArtist.Size = new System.Drawing.Size(300, 29);
            this.txtAddArtist.TabIndex = 3;
            this.txtAddArtist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAddArtist.Click += new System.EventHandler(this.txtAddArtist_Click);
            this.txtAddArtist.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtAddSong_KeyUp);
            // 
            // txtAddSong
            // 
            this.txtAddSong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddSong.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtAddSong.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtAddSong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddSong.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddSong.ForeColor = System.Drawing.Color.Black;
            this.txtAddSong.Location = new System.Drawing.Point(15, 207);
            this.txtAddSong.Name = "txtAddSong";
            this.txtAddSong.Size = new System.Drawing.Size(300, 29);
            this.txtAddSong.TabIndex = 2;
            this.txtAddSong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAddSong.Click += new System.EventHandler(this.txtAddSong_Click);
            this.txtAddSong.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtAddSong_KeyUp);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.LightGray;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(186, 300);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(129, 29);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add to &Queue >>>";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.EnabledChanged += new System.EventHandler(this.btnAdd_EnabledChanged);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblAddBand
            // 
            this.lblAddBand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddBand.BackColor = System.Drawing.Color.Transparent;
            this.lblAddBand.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddBand.Location = new System.Drawing.Point(15, 29);
            this.lblAddBand.Name = "lblAddBand";
            this.lblAddBand.Size = new System.Drawing.Size(300, 21);
            this.lblAddBand.TabIndex = 2;
            this.lblAddBand.Text = "Performer";
            this.lblAddBand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewLogsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.selectSetlistFile,
            this.viewSetlistDetails,
            this.toolStripMenuItem3,
            this.autosuggestSettings,
            this.toolStripMenuItem4,
            this.lockSeverityToolStripMenuItem,
            this.toolStripMenuItem5,
            this.ClockOptionsToolStripMenuItem,
            this.resetSize});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.ToolsToolStripMenuItem.Text = "&Options";
            // 
            // ViewLogsToolStripMenuItem
            // 
            this.ViewLogsToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ViewLogsToolStripMenuItem.Name = "ViewLogsToolStripMenuItem";
            this.ViewLogsToolStripMenuItem.ShowShortcutKeys = false;
            this.ViewLogsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.ViewLogsToolStripMenuItem.Text = "View &Logs";
            this.ViewLogsToolStripMenuItem.Click += new System.EventHandler(this.ViewLogsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(187, 6);
            // 
            // selectSetlistFile
            // 
            this.selectSetlistFile.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.selectSetlistFile.Name = "selectSetlistFile";
            this.selectSetlistFile.Size = new System.Drawing.Size(190, 22);
            this.selectSetlistFile.Text = "Select Setlist file";
            this.selectSetlistFile.Click += new System.EventHandler(this.selectSetlistFile_Click);
            // 
            // viewSetlistDetails
            // 
            this.viewSetlistDetails.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.viewSetlistDetails.Name = "viewSetlistDetails";
            this.viewSetlistDetails.Size = new System.Drawing.Size(190, 22);
            this.viewSetlistDetails.Text = "View Setlist details";
            this.viewSetlistDetails.Visible = false;
            this.viewSetlistDetails.Click += new System.EventHandler(this.viewSetlistDetails_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(187, 6);
            // 
            // autosuggestSettings
            // 
            this.autosuggestSettings.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.autosuggestSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableAutosuggest,
            this.editAutosuggestBands,
            this.editAutosuggestSongs,
            this.editAutosuggestArtists});
            this.autosuggestSettings.Name = "autosuggestSettings";
            this.autosuggestSettings.Size = new System.Drawing.Size(190, 22);
            this.autosuggestSettings.Text = "Auto-suggest settings";
            // 
            // enableAutosuggest
            // 
            this.enableAutosuggest.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.enableAutosuggest.Checked = true;
            this.enableAutosuggest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableAutosuggest.Name = "enableAutosuggest";
            this.enableAutosuggest.Size = new System.Drawing.Size(205, 22);
            this.enableAutosuggest.Text = "Enable auto-suggest";
            this.enableAutosuggest.Click += new System.EventHandler(this.enableAutosuggest_Click);
            // 
            // editAutosuggestBands
            // 
            this.editAutosuggestBands.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.editAutosuggestBands.Name = "editAutosuggestBands";
            this.editAutosuggestBands.Size = new System.Drawing.Size(205, 22);
            this.editAutosuggestBands.Text = "Edit Auto-suggest Bands";
            this.editAutosuggestBands.Click += new System.EventHandler(this.editAutosuggestBands_Click);
            // 
            // editAutosuggestSongs
            // 
            this.editAutosuggestSongs.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.editAutosuggestSongs.Name = "editAutosuggestSongs";
            this.editAutosuggestSongs.Size = new System.Drawing.Size(205, 22);
            this.editAutosuggestSongs.Text = "Edit Auto-suggest Songs";
            this.editAutosuggestSongs.Click += new System.EventHandler(this.editAutosuggestSongs_Click);
            // 
            // editAutosuggestArtists
            // 
            this.editAutosuggestArtists.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.editAutosuggestArtists.Name = "editAutosuggestArtists";
            this.editAutosuggestArtists.Size = new System.Drawing.Size(205, 22);
            this.editAutosuggestArtists.Text = "Edit Auto-suggest Artists";
            this.editAutosuggestArtists.Click += new System.EventHandler(this.editAutosuggestArtists_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(187, 6);
            // 
            // lockSeverityToolStripMenuItem
            // 
            this.lockSeverityToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lockSeverityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.maximumSecurity,
            this.relaxedSecurity});
            this.lockSeverityToolStripMenuItem.Name = "lockSeverityToolStripMenuItem";
            this.lockSeverityToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.lockSeverityToolStripMenuItem.Text = "Lock Severity";
            // 
            // maximumSecurity
            // 
            this.maximumSecurity.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.maximumSecurity.Checked = true;
            this.maximumSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.maximumSecurity.Name = "maximumSecurity";
            this.maximumSecurity.Size = new System.Drawing.Size(174, 22);
            this.maximumSecurity.Text = "Maximum Security";
            this.maximumSecurity.Click += new System.EventHandler(this.maximumSecurity_Click);
            // 
            // relaxedSecurity
            // 
            this.relaxedSecurity.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.relaxedSecurity.Name = "relaxedSecurity";
            this.relaxedSecurity.Size = new System.Drawing.Size(174, 22);
            this.relaxedSecurity.Text = "Relaxed Security";
            this.relaxedSecurity.Click += new System.EventHandler(this.relaxedSecurity_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(187, 6);
            // 
            // resetSize
            // 
            this.resetSize.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.resetSize.Name = "resetSize";
            this.resetSize.Size = new System.Drawing.Size(190, 22);
            this.resetSize.Text = "Reset size to default";
            this.resetSize.Click += new System.EventHandler(this.resetSize_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.ExitToolStripMenuItem.Text = "Save and E&xit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // grpNext
            // 
            this.grpNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grpNext.BackColor = System.Drawing.Color.Transparent;
            this.grpNext.Controls.Add(this.picDown);
            this.grpNext.Controls.Add(this.picUp);
            this.grpNext.Controls.Add(this.btnDelete);
            this.grpNext.Controls.Add(this.btnEdit);
            this.grpNext.Controls.Add(this.btnPlay);
            this.grpNext.Controls.Add(this.lstNext);
            this.grpNext.ForeColor = System.Drawing.Color.Black;
            this.grpNext.Location = new System.Drawing.Point(342, 340);
            this.grpNext.Name = "grpNext";
            this.grpNext.Size = new System.Drawing.Size(330, 337);
            this.grpNext.TabIndex = 18;
            this.grpNext.TabStop = false;
            this.grpNext.Text = "Coming Up";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.LightGray;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(237, 300);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 29);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.EnabledChanged += new System.EventHandler(this.btnDelete_EnabledChanged);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.LightGray;
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Enabled = false;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(125, 300);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 29);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.EnabledChanged += new System.EventHandler(this.btnEdit_EnabledChanged);
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlay.BackColor = System.Drawing.Color.LightGray;
            this.btnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlay.Enabled = false;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(15, 300);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(80, 29);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "&Play Now";
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.EnabledChanged += new System.EventHandler(this.btnPlay_EnabledChanged);
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lstNext
            // 
            this.lstNext.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstNext.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstNext.ForeColor = System.Drawing.Color.Black;
            this.lstNext.FormattingEnabled = true;
            this.lstNext.HorizontalScrollbar = true;
            this.lstNext.ItemHeight = 19;
            this.lstNext.Location = new System.Drawing.Point(15, 29);
            this.lstNext.Name = "lstNext";
            this.lstNext.Size = new System.Drawing.Size(276, 251);
            this.lstNext.TabIndex = 0;
            this.lstNext.SelectedIndexChanged += new System.EventHandler(this.lstNext_SelectedIndexChanged);
            this.lstNext.DoubleClick += new System.EventHandler(this.lstNext_DoubleClick);
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.ToolsToolStripMenuItem,
            this.HeloToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(1284, 24);
            this.MenuStrip1.TabIndex = 20;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "&File";
            // 
            // btnDelPrev
            // 
            this.btnDelPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelPrev.BackColor = System.Drawing.Color.LightGray;
            this.btnDelPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelPrev.Enabled = false;
            this.btnDelPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelPrev.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelPrev.Location = new System.Drawing.Point(235, 300);
            this.btnDelPrev.Name = "btnDelPrev";
            this.btnDelPrev.Size = new System.Drawing.Size(80, 29);
            this.btnDelPrev.TabIndex = 9;
            this.btnDelPrev.Text = "Delete";
            this.btnDelPrev.UseVisualStyleBackColor = false;
            this.btnDelPrev.EnabledChanged += new System.EventHandler(this.btnDelete_EnabledChanged);
            this.btnDelPrev.Click += new System.EventHandler(this.btnDelPrev_Click);
            // 
            // grpPrev
            // 
            this.grpPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grpPrev.BackColor = System.Drawing.Color.Transparent;
            this.grpPrev.Controls.Add(this.picRandom);
            this.grpPrev.Controls.Add(this.chkAutoQueue);
            this.grpPrev.Controls.Add(this.btnDelPrev);
            this.grpPrev.Controls.Add(this.btnEditPrev);
            this.grpPrev.Controls.Add(this.btnDetails);
            this.grpPrev.Controls.Add(this.lstPrevious);
            this.grpPrev.ForeColor = System.Drawing.Color.Black;
            this.grpPrev.Location = new System.Drawing.Point(675, 340);
            this.grpPrev.Name = "grpPrev";
            this.grpPrev.Size = new System.Drawing.Size(330, 337);
            this.grpPrev.TabIndex = 19;
            this.grpPrev.TabStop = false;
            this.grpPrev.Text = "Previous Acts";
            // 
            // chkAutoQueue
            // 
            this.chkAutoQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAutoQueue.AutoSize = true;
            this.chkAutoQueue.Checked = true;
            this.chkAutoQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoQueue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkAutoQueue.Location = new System.Drawing.Point(15, 271);
            this.chkAutoQueue.Name = "chkAutoQueue";
            this.chkAutoQueue.Size = new System.Drawing.Size(235, 17);
            this.chkAutoQueue.TabIndex = 10;
            this.chkAutoQueue.Text = "Queue automatically after each performance";
            this.chkAutoQueue.UseVisualStyleBackColor = true;
            // 
            // btnEditPrev
            // 
            this.btnEditPrev.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEditPrev.BackColor = System.Drawing.Color.LightGray;
            this.btnEditPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditPrev.Enabled = false;
            this.btnEditPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditPrev.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditPrev.Location = new System.Drawing.Point(137, 300);
            this.btnEditPrev.Name = "btnEditPrev";
            this.btnEditPrev.Size = new System.Drawing.Size(80, 29);
            this.btnEditPrev.TabIndex = 8;
            this.btnEditPrev.Text = "Edit";
            this.btnEditPrev.UseVisualStyleBackColor = false;
            this.btnEditPrev.EnabledChanged += new System.EventHandler(this.btnEdit_EnabledChanged);
            this.btnEditPrev.Click += new System.EventHandler(this.btnEditPrev_Click);
            // 
            // btnDetails
            // 
            this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDetails.BackColor = System.Drawing.Color.LightGray;
            this.btnDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDetails.Enabled = false;
            this.btnDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDetails.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetails.Location = new System.Drawing.Point(15, 300);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(103, 29);
            this.btnDetails.TabIndex = 6;
            this.btnDetails.Text = "&View Details";
            this.btnDetails.UseVisualStyleBackColor = false;
            this.btnDetails.EnabledChanged += new System.EventHandler(this.btnDetails_EnabledChanged);
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // lstPrevious
            // 
            this.lstPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPrevious.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPrevious.ForeColor = System.Drawing.Color.Black;
            this.lstPrevious.FormattingEnabled = true;
            this.lstPrevious.HorizontalScrollbar = true;
            this.lstPrevious.ItemHeight = 19;
            this.lstPrevious.Location = new System.Drawing.Point(15, 29);
            this.lstPrevious.Name = "lstPrevious";
            this.lstPrevious.Size = new System.Drawing.Size(300, 232);
            this.lstPrevious.TabIndex = 1;
            this.lstPrevious.SelectedIndexChanged += new System.EventHandler(this.lstPrevious_SelectedIndexChanged);
            this.lstPrevious.DoubleClick += new System.EventHandler(this.lstPrevious_DoubleClick);
            // 
            // picWorking
            // 
            this.picWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(-64, 27);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 64;
            this.picWorking.TabStop = false;
            // 
            // picLocked
            // 
            this.picLocked.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLocked.Image = ((System.Drawing.Image)(resources.GetObject("picLocked.Image")));
            this.picLocked.Location = new System.Drawing.Point(1084, 11);
            this.picLocked.Name = "picLocked";
            this.picLocked.Size = new System.Drawing.Size(75, 75);
            this.picLocked.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLocked.TabIndex = 29;
            this.picLocked.TabStop = false;
            this.picLocked.Visible = false;
            this.picLocked.Click += new System.EventHandler(this.picLocked_Click);
            // 
            // picUnlocked
            // 
            this.picUnlocked.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picUnlocked.Image = ((System.Drawing.Image)(resources.GetObject("picUnlocked.Image")));
            this.picUnlocked.Location = new System.Drawing.Point(1165, 11);
            this.picUnlocked.Name = "picUnlocked";
            this.picUnlocked.Size = new System.Drawing.Size(75, 75);
            this.picUnlocked.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUnlocked.TabIndex = 28;
            this.picUnlocked.TabStop = false;
            this.picUnlocked.Click += new System.EventHandler(this.picUnlocked_Click);
            // 
            // picMarquee
            // 
            this.picMarquee.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picMarquee.Location = new System.Drawing.Point(331, 11);
            this.picMarquee.Name = "picMarquee";
            this.picMarquee.Size = new System.Drawing.Size(694, 289);
            this.picMarquee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMarquee.TabIndex = 21;
            this.picMarquee.TabStop = false;
            // 
            // lblEndTimeDesc
            // 
            this.lblEndTimeDesc.AutoSize = true;
            this.lblEndTimeDesc.BackColor = System.Drawing.Color.RosyBrown;
            this.lblEndTimeDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEndTimeDesc.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndTimeDesc.ForeColor = System.Drawing.Color.White;
            this.lblEndTimeDesc.Location = new System.Drawing.Point(58, 139);
            this.lblEndTimeDesc.Name = "lblEndTimeDesc";
            this.lblEndTimeDesc.Size = new System.Drawing.Size(134, 24);
            this.lblEndTimeDesc.TabIndex = 67;
            this.lblEndTimeDesc.Text = "Est. End Time:";
            this.lblEndTimeDesc.Visible = false;
            // 
            // lblClockDesc
            // 
            this.lblClockDesc.AutoSize = true;
            this.lblClockDesc.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.lblClockDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClockDesc.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClockDesc.ForeColor = System.Drawing.Color.White;
            this.lblClockDesc.Location = new System.Drawing.Point(60, 40);
            this.lblClockDesc.Name = "lblClockDesc";
            this.lblClockDesc.Size = new System.Drawing.Size(129, 24);
            this.lblClockDesc.TabIndex = 68;
            this.lblClockDesc.Text = "Current Time:";
            // 
            // EventManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1284, 681);
            this.Controls.Add(this.lblClockDesc);
            this.Controls.Add(this.lblEndTimeDesc);
            this.Controls.Add(this.lblEndTime);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.lblClock);
            this.Controls.Add(this.btnEditPlay);
            this.Controls.Add(this.picLocked);
            this.Controls.Add(this.picUnlocked);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblNowPlayingBand);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblNowPlayingArtist);
            this.Controls.Add(this.lblNowPlayingSong);
            this.Controls.Add(this.grpNew);
            this.Controls.Add(this.grpNext);
            this.Controls.Add(this.grpPrev);
            this.Controls.Add(this.picMarquee);
            this.Controls.Add(this.MenuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "EventManager";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Event Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EventManager_FormClosing);
            this.Load += new System.EventHandler(this.EventManager_Load);
            this.Resize += new System.EventHandler(this.EventManager_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRandom)).EndInit();
            this.grpNew.ResumeLayout(false);
            this.grpNew.PerformLayout();
            this.grpNext.ResumeLayout(false);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.grpPrev.ResumeLayout(false);
            this.grpPrev.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLocked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUnlocked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMarquee)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStripMenuItem ShowOnForm;
        internal System.Windows.Forms.ToolStripMenuItem ShowOnTitleBar;
        internal System.Windows.Forms.ToolStripMenuItem HeloToolStripMenuItem;
        internal System.Windows.Forms.Label lblClock;
        internal System.Windows.Forms.ToolTip ToolTip1;
        internal System.Windows.Forms.ToolStripMenuItem ClockOptionsToolStripMenuItem;
        internal System.Windows.Forms.Button btnEditPlay;
        internal System.Windows.Forms.PictureBox picLocked;
        internal System.Windows.Forms.PictureBox picUnlocked;
        internal System.Windows.Forms.Button btnSkip;
        internal System.Windows.Forms.Button btnStop;
        internal System.Windows.Forms.Label lblNowPlayingBand;
        internal System.Windows.Forms.Timer tmrClock;
        internal System.Windows.Forms.PictureBox picDown;
        internal System.Windows.Forms.PictureBox picUp;
        internal System.Windows.Forms.Button btnStart;
        internal System.Windows.Forms.Label lblNowPlayingArtist;
        internal System.Windows.Forms.Label lblNowPlayingSong;
        internal System.Windows.Forms.GroupBox grpNew;
        internal System.Windows.Forms.Label lblAddArtist;
        internal System.Windows.Forms.Label lblAddSong;
        internal System.Windows.Forms.TextBox txtAddArtist;
        internal System.Windows.Forms.TextBox txtAddSong;
        internal System.Windows.Forms.Button btnAdd;
        internal System.Windows.Forms.Label lblAddBand;
        internal System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        internal System.Windows.Forms.GroupBox grpNext;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Button btnEdit;
        internal System.Windows.Forms.Button btnPlay;
        internal System.Windows.Forms.ListBox lstNext;
        internal System.Windows.Forms.MenuStrip MenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        internal System.Windows.Forms.Button btnDelPrev;
        internal System.Windows.Forms.GroupBox grpPrev;
        internal System.Windows.Forms.Button btnEditPrev;
        internal System.Windows.Forms.Button btnDetails;
        internal System.Windows.Forms.ListBox lstPrevious;
        internal System.Windows.Forms.PictureBox picMarquee;
        private System.Windows.Forms.ToolStripMenuItem selectSetlistFile;
        internal System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ListBox lstSearch;
        private System.Windows.Forms.PictureBox picWorking;
        internal System.Windows.Forms.Button btnSong;
        private System.Windows.Forms.ToolStripMenuItem resetSize;
        private System.Windows.Forms.ToolStripMenuItem viewSetlistDetails;
        internal System.Windows.Forms.ToolStripMenuItem ViewLogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem autosuggestSettings;
        private System.Windows.Forms.ToolStripMenuItem enableAutosuggest;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem editAutosuggestBands;
        private System.Windows.Forms.ToolStripMenuItem editAutosuggestSongs;
        private System.Windows.Forms.ToolStripMenuItem editAutosuggestArtists;
        private System.Windows.Forms.CheckBox chkAutoQueue;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.PictureBox picRandom;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showEndTime;
        internal System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.Label lblEndTimeDesc;
        private System.Windows.Forms.Label lblClockDesc;
        internal System.Windows.Forms.Button btnLoadSetlist;
        private System.Windows.Forms.ComboBox cboAddBand;
        private System.Windows.Forms.ToolStripMenuItem lockSeverityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maximumSecurity;
        private System.Windows.Forms.ToolStripMenuItem relaxedSecurity;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;

    }
}