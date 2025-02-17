namespace Nautilus
{
    partial class MIDICleaner
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
            this.lstLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBegin = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.numVelocity = new System.Windows.Forms.NumericUpDown();
            this.chkVelocity = new System.Windows.Forms.CheckBox();
            this.chkLength = new System.Windows.Forms.CheckBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.cboLength = new System.Windows.Forms.ComboBox();
            this.picPin = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailedLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportUsingMeasureBeatTicksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportUsingLocationInSecondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakDownIssueCountByTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.dONOTFixCapitalizationErrorsHarm1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dONOTFixCapitalizationErrorsHarm2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreMIDINotes0And1 = new System.Windows.Forms.ToolStripMenuItem();
            this.guitarHeroDrumsSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveGreenTomMarkers = new System.Windows.Forms.ToolStripMenuItem();
            this.doNotDelete2xBassPedalNotes = new System.Windows.Forms.ToolStripMenuItem();
            this.separate2xBassPedalNotes = new System.Windows.Forms.ToolStripMenuItem();
            this.leaveSysEx = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.createBEATTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteVENUETracks = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLighting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.backUpFileWhenCleaning = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVelocity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            this.SuspendLayout();
            // 
            // lstLog
            // 
            this.lstLog.AllowDrop = true;
            this.lstLog.BackColor = System.Drawing.Color.White;
            this.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLog.ContextMenuStrip = this.contextMenuStrip1;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Location = new System.Drawing.Point(12, 28);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(590, 340);
            this.lstLog.TabIndex = 19;
            this.toolTip1.SetToolTip(this.lstLog, "This is the application log. Right click to export");
            this.lstLog.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.lstLog.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportLogFileToolStripMenuItem,
            this.clearLogToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 48);
            // 
            // exportLogFileToolStripMenuItem
            // 
            this.exportLogFileToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exportLogFileToolStripMenuItem.Name = "exportLogFileToolStripMenuItem";
            this.exportLogFileToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.exportLogFileToolStripMenuItem.Text = "Export log file";
            this.exportLogFileToolStripMenuItem.Click += new System.EventHandler(this.exportLogFileToolStripMenuItem_Click);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.clearLogToolStripMenuItem.Text = "Clear log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // btnBegin
            // 
            this.btnBegin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(84)))), ((int)(((byte)(86)))));
            this.btnBegin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBegin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBegin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBegin.ForeColor = System.Drawing.Color.White;
            this.btnBegin.Location = new System.Drawing.Point(543, 375);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(59, 30);
            this.btnBegin.TabIndex = 20;
            this.btnBegin.Text = "&Begin";
            this.toolTip1.SetToolTip(this.btnBegin, "Click here to begin cleaning the MIDI file(s)");
            this.btnBegin.UseVisualStyleBackColor = false;
            this.btnBegin.Visible = false;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            this.btnBegin.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnBegin.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // numVelocity
            // 
            this.numVelocity.AllowDrop = true;
            this.numVelocity.Location = new System.Drawing.Point(237, 382);
            this.numVelocity.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numVelocity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVelocity.Name = "numVelocity";
            this.numVelocity.Size = new System.Drawing.Size(44, 20);
            this.numVelocity.TabIndex = 27;
            this.numVelocity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numVelocity, "This is the velocity that will be applied to the notes if this feature is enabled" +
        "");
            this.numVelocity.Value = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.numVelocity.ValueChanged += new System.EventHandler(this.SaveOptions);
            this.numVelocity.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.numVelocity.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // chkVelocity
            // 
            this.chkVelocity.AllowDrop = true;
            this.chkVelocity.AutoSize = true;
            this.chkVelocity.BackColor = System.Drawing.Color.Transparent;
            this.chkVelocity.Checked = true;
            this.chkVelocity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVelocity.Location = new System.Drawing.Point(132, 383);
            this.chkVelocity.Name = "chkVelocity";
            this.chkVelocity.Size = new System.Drawing.Size(105, 17);
            this.chkVelocity.TabIndex = 28;
            this.chkVelocity.Text = "Override velocity";
            this.toolTip1.SetToolTip(this.chkVelocity, "Leave this checked if you want to change the note velocity for all notes in the M" +
        "IDI file");
            this.chkVelocity.UseVisualStyleBackColor = false;
            this.chkVelocity.CheckedChanged += new System.EventHandler(this.chkVelocity_CheckedChanged);
            this.chkVelocity.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.chkVelocity.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // chkLength
            // 
            this.chkLength.AllowDrop = true;
            this.chkLength.AutoSize = true;
            this.chkLength.BackColor = System.Drawing.Color.Transparent;
            this.chkLength.Checked = true;
            this.chkLength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLength.Location = new System.Drawing.Point(319, 383);
            this.chkLength.Name = "chkLength";
            this.chkLength.Size = new System.Drawing.Size(117, 17);
            this.chkLength.TabIndex = 30;
            this.chkLength.Text = "Zero-length note fix";
            this.toolTip1.SetToolTip(this.chkLength, "Leave this enabled to resize FoF style notes to something more practical in REAPE" +
        "R");
            this.chkLength.UseVisualStyleBackColor = false;
            this.chkLength.CheckedChanged += new System.EventHandler(this.chkLength_CheckedChanged);
            this.chkLength.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.chkLength.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnOpen
            // 
            this.btnOpen.AllowDrop = true;
            this.btnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(84)))), ((int)(((byte)(86)))));
            this.btnOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpen.ForeColor = System.Drawing.Color.White;
            this.btnOpen.Location = new System.Drawing.Point(12, 375);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(79, 30);
            this.btnOpen.TabIndex = 31;
            this.btnOpen.Text = "Bro&wse...";
            this.toolTip1.SetToolTip(this.btnOpen, "Click here to browse for the MIDI file(s)");
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            this.btnOpen.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnOpen.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // cboLength
            // 
            this.cboLength.AllowDrop = true;
            this.cboLength.FormattingEnabled = true;
            this.cboLength.Items.AddRange(new object[] {
            "1/16",
            "1/32",
            "1/64"});
            this.cboLength.Location = new System.Drawing.Point(439, 380);
            this.cboLength.Name = "cboLength";
            this.cboLength.Size = new System.Drawing.Size(51, 21);
            this.cboLength.TabIndex = 32;
            this.toolTip1.SetToolTip(this.cboLength, "This is the duration the resized notes will be given if this feature is enabled");
            this.cboLength.SelectedIndexChanged += new System.EventHandler(this.SaveOptions);
            this.cboLength.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.cboLength.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // picPin
            // 
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(589, 4);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 64;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.toolTip1.SetToolTip(this.picPin, "Click to pin on top");
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(614, 24);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailedLoggingToolStripMenuItem,
            this.toolStripMenuItem1,
            this.dONOTFixCapitalizationErrorsHarm1,
            this.dONOTFixCapitalizationErrorsHarm2,
            this.ignoreMIDINotes0And1,
            this.guitarHeroDrumsSettingsToolStripMenuItem,
            this.leaveSysEx,
            this.toolStripMenuItem2,
            this.createBEATTracks,
            this.deleteVENUETracks,
            this.removeLighting,
            this.toolStripMenuItem3,
            this.backUpFileWhenCleaning});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // detailedLoggingToolStripMenuItem
            // 
            this.detailedLoggingToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.detailedLoggingToolStripMenuItem.CheckOnClick = true;
            this.detailedLoggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportUsingMeasureBeatTicksToolStripMenuItem,
            this.reportUsingLocationInSecondsToolStripMenuItem,
            this.breakDownIssueCountByTrackToolStripMenuItem});
            this.detailedLoggingToolStripMenuItem.Name = "detailedLoggingToolStripMenuItem";
            this.detailedLoggingToolStripMenuItem.Size = new System.Drawing.Size(333, 22);
            this.detailedLoggingToolStripMenuItem.Text = "Detailed logging";
            this.detailedLoggingToolStripMenuItem.Click += new System.EventHandler(this.detailedLoggingToolStripMenuItem_Click);
            // 
            // reportUsingMeasureBeatTicksToolStripMenuItem
            // 
            this.reportUsingMeasureBeatTicksToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.reportUsingMeasureBeatTicksToolStripMenuItem.Checked = true;
            this.reportUsingMeasureBeatTicksToolStripMenuItem.CheckOnClick = true;
            this.reportUsingMeasureBeatTicksToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reportUsingMeasureBeatTicksToolStripMenuItem.Enabled = false;
            this.reportUsingMeasureBeatTicksToolStripMenuItem.Name = "reportUsingMeasureBeatTicksToolStripMenuItem";
            this.reportUsingMeasureBeatTicksToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.reportUsingMeasureBeatTicksToolStripMenuItem.Text = "Report using [Measure:Beat:Ticks]";
            this.reportUsingMeasureBeatTicksToolStripMenuItem.Click += new System.EventHandler(this.reportUsingMeasureBeatTicksToolStripMenuItem_Click);
            // 
            // reportUsingLocationInSecondsToolStripMenuItem
            // 
            this.reportUsingLocationInSecondsToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.reportUsingLocationInSecondsToolStripMenuItem.CheckOnClick = true;
            this.reportUsingLocationInSecondsToolStripMenuItem.Enabled = false;
            this.reportUsingLocationInSecondsToolStripMenuItem.Name = "reportUsingLocationInSecondsToolStripMenuItem";
            this.reportUsingLocationInSecondsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.reportUsingLocationInSecondsToolStripMenuItem.Text = "Report using location in seconds";
            this.reportUsingLocationInSecondsToolStripMenuItem.Click += new System.EventHandler(this.reportUsingLocationInSecondsToolStripMenuItem_Click);
            // 
            // breakDownIssueCountByTrackToolStripMenuItem
            // 
            this.breakDownIssueCountByTrackToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.breakDownIssueCountByTrackToolStripMenuItem.Checked = true;
            this.breakDownIssueCountByTrackToolStripMenuItem.CheckOnClick = true;
            this.breakDownIssueCountByTrackToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.breakDownIssueCountByTrackToolStripMenuItem.Enabled = false;
            this.breakDownIssueCountByTrackToolStripMenuItem.Name = "breakDownIssueCountByTrackToolStripMenuItem";
            this.breakDownIssueCountByTrackToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.breakDownIssueCountByTrackToolStripMenuItem.Text = "Break down issue count by track";
            this.breakDownIssueCountByTrackToolStripMenuItem.Click += new System.EventHandler(this.SaveOptions);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(330, 6);
            // 
            // dONOTFixCapitalizationErrorsHarm1
            // 
            this.dONOTFixCapitalizationErrorsHarm1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dONOTFixCapitalizationErrorsHarm1.CheckOnClick = true;
            this.dONOTFixCapitalizationErrorsHarm1.Name = "dONOTFixCapitalizationErrorsHarm1";
            this.dONOTFixCapitalizationErrorsHarm1.Size = new System.Drawing.Size(333, 22);
            this.dONOTFixCapitalizationErrorsHarm1.Text = "Do not fix capitalization errors - VOCALS/HARM1";
            this.dONOTFixCapitalizationErrorsHarm1.Click += new System.EventHandler(this.SaveOptions);
            // 
            // dONOTFixCapitalizationErrorsHarm2
            // 
            this.dONOTFixCapitalizationErrorsHarm2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dONOTFixCapitalizationErrorsHarm2.Checked = true;
            this.dONOTFixCapitalizationErrorsHarm2.CheckOnClick = true;
            this.dONOTFixCapitalizationErrorsHarm2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dONOTFixCapitalizationErrorsHarm2.Name = "dONOTFixCapitalizationErrorsHarm2";
            this.dONOTFixCapitalizationErrorsHarm2.Size = new System.Drawing.Size(333, 22);
            this.dONOTFixCapitalizationErrorsHarm2.Text = "Do not fix capitalization errors - HARM2";
            this.dONOTFixCapitalizationErrorsHarm2.Click += new System.EventHandler(this.SaveOptions);
            // 
            // ignoreMIDINotes0And1
            // 
            this.ignoreMIDINotes0And1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ignoreMIDINotes0And1.CheckOnClick = true;
            this.ignoreMIDINotes0And1.Name = "ignoreMIDINotes0And1";
            this.ignoreMIDINotes0And1.Size = new System.Drawing.Size(333, 22);
            this.ignoreMIDINotes0And1.Text = "Allow MIDI notes 0 and 1 on Vocals/Harmonies";
            this.ignoreMIDINotes0And1.Click += new System.EventHandler(this.SaveOptions);
            // 
            // guitarHeroDrumsSettingsToolStripMenuItem
            // 
            this.guitarHeroDrumsSettingsToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.guitarHeroDrumsSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveGreenTomMarkers,
            this.doNotDelete2xBassPedalNotes,
            this.separate2xBassPedalNotes});
            this.guitarHeroDrumsSettingsToolStripMenuItem.Name = "guitarHeroDrumsSettingsToolStripMenuItem";
            this.guitarHeroDrumsSettingsToolStripMenuItem.Size = new System.Drawing.Size(333, 22);
            this.guitarHeroDrumsSettingsToolStripMenuItem.Text = "Guitar Hero drums settings";
            // 
            // moveGreenTomMarkers
            // 
            this.moveGreenTomMarkers.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.moveGreenTomMarkers.Checked = true;
            this.moveGreenTomMarkers.CheckOnClick = true;
            this.moveGreenTomMarkers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.moveGreenTomMarkers.Name = "moveGreenTomMarkers";
            this.moveGreenTomMarkers.Size = new System.Drawing.Size(339, 22);
            this.moveGreenTomMarkers.Text = "Move green tom markers to RB3 position";
            this.moveGreenTomMarkers.Click += new System.EventHandler(this.SaveOptions);
            // 
            // doNotDelete2xBassPedalNotes
            // 
            this.doNotDelete2xBassPedalNotes.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.doNotDelete2xBassPedalNotes.CheckOnClick = true;
            this.doNotDelete2xBassPedalNotes.Name = "doNotDelete2xBassPedalNotes";
            this.doNotDelete2xBassPedalNotes.Size = new System.Drawing.Size(339, 22);
            this.doNotDelete2xBassPedalNotes.Text = "Do not delete Expert+ notes";
            this.doNotDelete2xBassPedalNotes.Click += new System.EventHandler(this.doNotDelete2xBassPedalNotes_Click);
            // 
            // separate2xBassPedalNotes
            // 
            this.separate2xBassPedalNotes.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.separate2xBassPedalNotes.Checked = true;
            this.separate2xBassPedalNotes.CheckOnClick = true;
            this.separate2xBassPedalNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.separate2xBassPedalNotes.Name = "separate2xBassPedalNotes";
            this.separate2xBassPedalNotes.Size = new System.Drawing.Size(339, 22);
            this.separate2xBassPedalNotes.Text = "Separate Expert+ notes into PART DRUMS_2X chart";
            this.separate2xBassPedalNotes.Click += new System.EventHandler(this.separate2xBassPedalNotes_Click);
            // 
            // leaveSysEx
            // 
            this.leaveSysEx.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.leaveSysEx.CheckOnClick = true;
            this.leaveSysEx.Name = "leaveSysEx";
            this.leaveSysEx.Size = new System.Drawing.Size(333, 22);
            this.leaveSysEx.Text = "Do not remove SysEx events";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(330, 6);
            // 
            // createBEATTracks
            // 
            this.createBEATTracks.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.createBEATTracks.Checked = true;
            this.createBEATTracks.CheckOnClick = true;
            this.createBEATTracks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.createBEATTracks.Name = "createBEATTracks";
            this.createBEATTracks.Size = new System.Drawing.Size(333, 22);
            this.createBEATTracks.Text = "Create BEAT tracks";
            this.createBEATTracks.Click += new System.EventHandler(this.SaveOptions);
            // 
            // deleteVENUETracks
            // 
            this.deleteVENUETracks.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.deleteVENUETracks.CheckOnClick = true;
            this.deleteVENUETracks.Name = "deleteVENUETracks";
            this.deleteVENUETracks.Size = new System.Drawing.Size(333, 22);
            this.deleteVENUETracks.Text = "Delete VENUE tracks";
            this.deleteVENUETracks.Click += new System.EventHandler(this.SaveOptions);
            // 
            // removeLighting
            // 
            this.removeLighting.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.removeLighting.Checked = true;
            this.removeLighting.CheckOnClick = true;
            this.removeLighting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeLighting.Name = "removeLighting";
            this.removeLighting.Size = new System.Drawing.Size(333, 22);
            this.removeLighting.Text = "Remove [lighting()] from VENUE  track";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(330, 6);
            // 
            // backUpFileWhenCleaning
            // 
            this.backUpFileWhenCleaning.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.backUpFileWhenCleaning.Checked = true;
            this.backUpFileWhenCleaning.CheckOnClick = true;
            this.backUpFileWhenCleaning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backUpFileWhenCleaning.Name = "backUpFileWhenCleaning";
            this.backUpFileWhenCleaning.Size = new System.Drawing.Size(333, 22);
            this.backUpFileWhenCleaning.Text = "Back up CON file when cleaning";
            this.backUpFileWhenCleaning.Click += new System.EventHandler(this.SaveOptions);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // picWorking
            // 
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(246, 7);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 57;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // MIDICleaner
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(614, 415);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.cboLength);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.chkLength);
            this.Controls.Add(this.chkVelocity);
            this.Controls.Add(this.numVelocity);
            this.Controls.Add(this.btnBegin);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MIDICleaner";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MIDI Cleaner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MIDICleaner_FormClosing);
            this.Shown += new System.EventHandler(this.MIDICleaner_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numVelocity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteVENUETracks;
        private System.Windows.Forms.NumericUpDown numVelocity;
        private System.Windows.Forms.CheckBox chkVelocity;
        private System.Windows.Forms.CheckBox chkLength;
        private System.Windows.Forms.ToolStripMenuItem detailedLoggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoreMIDINotes0And1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboLength;
        private System.Windows.Forms.ToolStripMenuItem dONOTFixCapitalizationErrorsHarm1;
        private System.Windows.Forms.ToolStripMenuItem guitarHeroDrumsSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveGreenTomMarkers;
        private System.Windows.Forms.ToolStripMenuItem doNotDelete2xBassPedalNotes;
        private System.Windows.Forms.ToolStripMenuItem separate2xBassPedalNotes;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem reportUsingMeasureBeatTicksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportUsingLocationInSecondsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createBEATTracks;
        private System.Windows.Forms.ToolStripMenuItem breakDownIssueCountByTrackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dONOTFixCapitalizationErrorsHarm2;
        private System.Windows.Forms.PictureBox picWorking;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem backUpFileWhenCleaning;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.ToolStripMenuItem leaveSysEx;
        private System.Windows.Forms.ToolStripMenuItem removeLighting;
    }
}