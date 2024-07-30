namespace Nautilus
{
    partial class WiiConverter
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
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBegin = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picPin = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.extrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertVENUEFromRBN2ToRBN1 = new System.Windows.Forms.ToolStripMenuItem();
            this.grabNgidFromBINFile = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkCreatePreview = new System.Windows.Forms.ToolStripMenuItem();
            this.convertVenueDuringSongConversion = new System.Windows.Forms.ToolStripMenuItem();
            this.attemptToConvertPostprocEvents = new System.Windows.Forms.ToolStripMenuItem();
            this.previewClipMixingMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoDownmixPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.dualMonoChannels = new System.Windows.Forms.ToolStripMenuItem();
            this.autoDownmixDrums = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCrowdAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnFolder = new System.Windows.Forms.Button();
            this.chkRAR = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkEncrypt = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboRate = new System.Windows.Forms.ComboBox();
            this.numAttenuation = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numFadeOut = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numFadeIn = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkDummy = new System.Windows.Forms.CheckBox();
            this.picHelp = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSmartMerge = new System.Windows.Forms.Button();
            this.chkSmartDelete = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAttenuation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFadeOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFadeIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(474, 7);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "Refresh Folder";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Visible = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.BackColor = System.Drawing.Color.White;
            this.txtFolder.Location = new System.Drawing.Point(6, 40);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(568, 20);
            this.txtFolder.TabIndex = 13;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            this.txtFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.txtFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
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
            this.lstLog.Location = new System.Drawing.Point(12, 249);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(588, 223);
            this.lstLog.TabIndex = 12;
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
            this.btnBegin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBegin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            this.btnBegin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBegin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBegin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBegin.ForeColor = System.Drawing.Color.White;
            this.btnBegin.Location = new System.Drawing.Point(510, 155);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(64, 29);
            this.btnBegin.TabIndex = 19;
            this.btnBegin.Text = "&Begin";
            this.toolTip1.SetToolTip(this.btnBegin, "Click to begin");
            this.btnBegin.UseVisualStyleBackColor = false;
            this.btnBegin.Visible = false;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(587, 5);
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
            this.extrasToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(613, 24);
            this.menuStrip1.TabIndex = 35;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // extrasToolStripMenuItem
            // 
            this.extrasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertVENUEFromRBN2ToRBN1,
            this.grabNgidFromBINFile});
            this.extrasToolStripMenuItem.Name = "extrasToolStripMenuItem";
            this.extrasToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.extrasToolStripMenuItem.Text = "Extras";
            // 
            // convertVENUEFromRBN2ToRBN1
            // 
            this.convertVENUEFromRBN2ToRBN1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.convertVENUEFromRBN2ToRBN1.Name = "convertVENUEFromRBN2ToRBN1";
            this.convertVENUEFromRBN2ToRBN1.Size = new System.Drawing.Size(262, 22);
            this.convertVENUEFromRBN2ToRBN1.Text = "Convert VENUE from RBN2 to RBN1";
            this.convertVENUEFromRBN2ToRBN1.Click += new System.EventHandler(this.convertVENUEFromRBN2ToRBN1ToolStripMenuItem_Click);
            // 
            // grabNgidFromBINFile
            // 
            this.grabNgidFromBINFile.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.grabNgidFromBINFile.Name = "grabNgidFromBINFile";
            this.grabNgidFromBINFile.Size = new System.Drawing.Size(262, 22);
            this.grabNgidFromBINFile.Text = "Grab ng_id from BIN file";
            this.grabNgidFromBINFile.Click += new System.EventHandler(this.grabNgidFromBINFile_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkCreatePreview,
            this.convertVenueDuringSongConversion,
            this.attemptToConvertPostprocEvents,
            this.previewClipMixingMethodToolStripMenuItem,
            this.autoDownmixDrums,
            this.deleteCrowdAudio});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // chkCreatePreview
            // 
            this.chkCreatePreview.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.chkCreatePreview.Checked = true;
            this.chkCreatePreview.CheckOnClick = true;
            this.chkCreatePreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreatePreview.Name = "chkCreatePreview";
            this.chkCreatePreview.Size = new System.Drawing.Size(279, 22);
            this.chkCreatePreview.Text = "Auto create song preview audio";
            this.chkCreatePreview.CheckedChanged += new System.EventHandler(this.chkCreatePreview_CheckedChanged);
            // 
            // convertVenueDuringSongConversion
            // 
            this.convertVenueDuringSongConversion.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.convertVenueDuringSongConversion.Checked = true;
            this.convertVenueDuringSongConversion.CheckOnClick = true;
            this.convertVenueDuringSongConversion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.convertVenueDuringSongConversion.Name = "convertVenueDuringSongConversion";
            this.convertVenueDuringSongConversion.Size = new System.Drawing.Size(279, 22);
            this.convertVenueDuringSongConversion.Text = "Convert venue during song conversion";
            this.convertVenueDuringSongConversion.Click += new System.EventHandler(this.convertVenueDuringSongConversion_Click);
            // 
            // attemptToConvertPostprocEvents
            // 
            this.attemptToConvertPostprocEvents.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.attemptToConvertPostprocEvents.Checked = true;
            this.attemptToConvertPostprocEvents.CheckOnClick = true;
            this.attemptToConvertPostprocEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.attemptToConvertPostprocEvents.Name = "attemptToConvertPostprocEvents";
            this.attemptToConvertPostprocEvents.Size = new System.Drawing.Size(279, 22);
            this.attemptToConvertPostprocEvents.Text = "Attempt to convert post-proc events";
            // 
            // previewClipMixingMethodToolStripMenuItem
            // 
            this.previewClipMixingMethodToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.previewClipMixingMethodToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoDownmixPreview,
            this.dualMonoChannels});
            this.previewClipMixingMethodToolStripMenuItem.Name = "previewClipMixingMethodToolStripMenuItem";
            this.previewClipMixingMethodToolStripMenuItem.Size = new System.Drawing.Size(279, 22);
            this.previewClipMixingMethodToolStripMenuItem.Text = "Preview clip mixing method";
            this.previewClipMixingMethodToolStripMenuItem.Visible = false;
            // 
            // autoDownmixPreview
            // 
            this.autoDownmixPreview.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.autoDownmixPreview.Checked = true;
            this.autoDownmixPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoDownmixPreview.Name = "autoDownmixPreview";
            this.autoDownmixPreview.Size = new System.Drawing.Size(231, 22);
            this.autoDownmixPreview.Text = "Automatic downmix (default)";
            this.autoDownmixPreview.Click += new System.EventHandler(this.autodownmix_Click);
            // 
            // dualMonoChannels
            // 
            this.dualMonoChannels.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dualMonoChannels.Name = "dualMonoChannels";
            this.dualMonoChannels.Size = new System.Drawing.Size(231, 22);
            this.dualMonoChannels.Text = "Dual mono channels";
            this.dualMonoChannels.Click += new System.EventHandler(this.dualMonoChannels_Click);
            // 
            // autoDownmixDrums
            // 
            this.autoDownmixDrums.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.autoDownmixDrums.Checked = true;
            this.autoDownmixDrums.CheckOnClick = true;
            this.autoDownmixDrums.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoDownmixDrums.Name = "autoDownmixDrums";
            this.autoDownmixDrums.Size = new System.Drawing.Size(279, 22);
            this.autoDownmixDrums.Text = "Auto downmix drum stems to stereo";
            this.autoDownmixDrums.Click += new System.EventHandler(this.autoDownmixDrums_Click);
            // 
            // deleteCrowdAudio
            // 
            this.deleteCrowdAudio.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.deleteCrowdAudio.Checked = true;
            this.deleteCrowdAudio.CheckOnClick = true;
            this.deleteCrowdAudio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteCrowdAudio.Name = "deleteCrowdAudio";
            this.deleteCrowdAudio.Size = new System.Drawing.Size(279, 22);
            this.deleteCrowdAudio.Text = "Delete crowd audio track if present";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem1.Text = "&Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(6, 155);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 29);
            this.btnReset.TabIndex = 40;
            this.btnReset.Text = "&Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnFolder
            // 
            this.btnFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            this.btnFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFolder.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFolder.ForeColor = System.Drawing.Color.White;
            this.btnFolder.Location = new System.Drawing.Point(6, 7);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(134, 30);
            this.btnFolder.TabIndex = 14;
            this.btnFolder.Text = "Change &Input Folder";
            this.btnFolder.UseVisualStyleBackColor = false;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // chkRAR
            // 
            this.chkRAR.AutoSize = true;
            this.chkRAR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkRAR.Location = new System.Drawing.Point(6, 66);
            this.chkRAR.Name = "chkRAR";
            this.chkRAR.Size = new System.Drawing.Size(189, 17);
            this.chkRAR.TabIndex = 41;
            this.chkRAR.Text = "Create RAR archive for each song";
            this.chkRAR.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(588, 216);
            this.tabControl1.TabIndex = 47;
            this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.tabControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // tabPage1
            // 
            this.tabPage1.AllowDrop = true;
            this.tabPage1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPage1.Controls.Add(this.chkEncrypt);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.picWorking);
            this.tabPage1.Controls.Add(this.btnReset);
            this.tabPage1.Controls.Add(this.txtFolder);
            this.tabPage1.Controls.Add(this.btnFolder);
            this.tabPage1.Controls.Add(this.btnRefresh);
            this.tabPage1.Controls.Add(this.btnBegin);
            this.tabPage1.Controls.Add(this.chkRAR);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(580, 190);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Convert CON Files";
            this.tabPage1.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.tabPage1.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // chkEncrypt
            // 
            this.chkEncrypt.AutoSize = true;
            this.chkEncrypt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkEncrypt.Location = new System.Drawing.Point(432, 66);
            this.chkEncrypt.Name = "chkEncrypt";
            this.chkEncrypt.Size = new System.Drawing.Size(142, 17);
            this.chkEncrypt.TabIndex = 59;
            this.chkEncrypt.Text = "Encrypt audio (mogg) file";
            this.chkEncrypt.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboRate);
            this.groupBox1.Controls.Add(this.numAttenuation);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numFadeOut);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numFadeIn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numLength);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(568, 56);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Song preview clip (all times in seconds)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Rate:";
            // 
            // cboRate
            // 
            this.cboRate.FormattingEnabled = true;
            this.cboRate.Items.AddRange(new object[] {
            "22050 Hz",
            "32000 Hz",
            "44100 Hz",
            "48000 Hz"});
            this.cboRate.Location = new System.Drawing.Point(45, 26);
            this.cboRate.Name = "cboRate";
            this.cboRate.Size = new System.Drawing.Size(88, 21);
            this.cboRate.TabIndex = 9;
            this.cboRate.Text = "22050 Hz";
            // 
            // numAttenuation
            // 
            this.numAttenuation.DecimalPlaces = 1;
            this.numAttenuation.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numAttenuation.Location = new System.Drawing.Point(510, 27);
            this.numAttenuation.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAttenuation.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147418112});
            this.numAttenuation.Name = "numAttenuation";
            this.numAttenuation.Size = new System.Drawing.Size(49, 20);
            this.numAttenuation.TabIndex = 8;
            this.numAttenuation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(462, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Att. (dB):";
            // 
            // numFadeOut
            // 
            this.numFadeOut.DecimalPlaces = 1;
            this.numFadeOut.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numFadeOut.Location = new System.Drawing.Point(401, 27);
            this.numFadeOut.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numFadeOut.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFadeOut.Name = "numFadeOut";
            this.numFadeOut.Size = new System.Drawing.Size(49, 20);
            this.numFadeOut.TabIndex = 5;
            this.numFadeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numFadeOut.Value = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(347, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fade Out:";
            // 
            // numFadeIn
            // 
            this.numFadeIn.DecimalPlaces = 1;
            this.numFadeIn.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numFadeIn.Location = new System.Drawing.Point(287, 27);
            this.numFadeIn.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numFadeIn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFadeIn.Name = "numFadeIn";
            this.numFadeIn.Size = new System.Drawing.Size(49, 20);
            this.numFadeIn.TabIndex = 3;
            this.numFadeIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numFadeIn.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fade In:";
            // 
            // numLength
            // 
            this.numLength.Location = new System.Drawing.Point(189, 27);
            this.numLength.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numLength.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size(42, 20);
            this.numLength.TabIndex = 1;
            this.numLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numLength.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Duration:";
            // 
            // picWorking
            // 
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(235, 169);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 57;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.AllowDrop = true;
            this.tabPage2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPage2.Controls.Add(this.chkDummy);
            this.tabPage2.Controls.Add(this.picHelp);
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Controls.Add(this.btnSmartMerge);
            this.tabPage2.Controls.Add(this.chkSmartDelete);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(580, 190);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Smart Folder Merge";
            this.tabPage2.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.tabPage2.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // chkDummy
            // 
            this.chkDummy.AutoSize = true;
            this.chkDummy.Location = new System.Drawing.Point(197, 22);
            this.chkDummy.Name = "chkDummy";
            this.chkDummy.Size = new System.Drawing.Size(137, 17);
            this.chkDummy.TabIndex = 125;
            this.chkDummy.Text = "Use basic animation file";
            this.chkDummy.UseVisualStyleBackColor = true;
            // 
            // picHelp
            // 
            this.picHelp.BackColor = System.Drawing.Color.Transparent;
            this.picHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picHelp.Image = global::Nautilus.Properties.Resources.help1;
            this.picHelp.Location = new System.Drawing.Point(549, 6);
            this.picHelp.Name = "picHelp";
            this.picHelp.Size = new System.Drawing.Size(25, 25);
            this.picHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHelp.TabIndex = 124;
            this.picHelp.TabStop = false;
            this.picHelp.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picHelp_MouseClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Nautilus.Properties.Resources.dragdrop;
            this.pictureBox1.Location = new System.Drawing.Point(26, 58);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(529, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 45;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnSmartMerge
            // 
            this.btnSmartMerge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(129)))), ((int)(((byte)(216)))));
            this.btnSmartMerge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSmartMerge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSmartMerge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSmartMerge.ForeColor = System.Drawing.Color.White;
            this.btnSmartMerge.Location = new System.Drawing.Point(26, 15);
            this.btnSmartMerge.Name = "btnSmartMerge";
            this.btnSmartMerge.Size = new System.Drawing.Size(153, 30);
            this.btnSmartMerge.TabIndex = 44;
            this.btnSmartMerge.Text = "&Choose source folder";
            this.btnSmartMerge.UseVisualStyleBackColor = false;
            this.btnSmartMerge.Click += new System.EventHandler(this.btnSmartMerge_Click);
            this.btnSmartMerge.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnSmartMerge.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // chkSmartDelete
            // 
            this.chkSmartDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSmartDelete.AutoSize = true;
            this.chkSmartDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSmartDelete.Location = new System.Drawing.Point(348, 22);
            this.chkSmartDelete.Name = "chkSmartDelete";
            this.chkSmartDelete.Size = new System.Drawing.Size(190, 17);
            this.chkSmartDelete.TabIndex = 43;
            this.chkSmartDelete.Text = "Delete source folders after merging";
            this.chkSmartDelete.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // WiiConverter
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(613, 488);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lstLog);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "WiiConverter";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wii Converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WiiPrep_FormClosing);
            this.Shown += new System.EventHandler(this.WiiPrep_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.Resize += new System.EventHandler(this.WiiPrep_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAttenuation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFadeOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFadeIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportLogFileToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkRAR;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnSmartMerge;
        private System.Windows.Forms.CheckBox chkSmartDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox picHelp;
        private System.Windows.Forms.CheckBox chkDummy;
        private System.Windows.Forms.ToolStripMenuItem extrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertVENUEFromRBN2ToRBN1;
        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertVenueDuringSongConversion;
        private System.Windows.Forms.ToolStripMenuItem attemptToConvertPostprocEvents;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numFadeOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numFadeIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numAttenuation;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem previewClipMixingMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoDownmixPreview;
        private System.Windows.Forms.ToolStripMenuItem dualMonoChannels;
        private System.Windows.Forms.ToolStripMenuItem grabNgidFromBINFile;
        private System.Windows.Forms.CheckBox chkEncrypt;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.ToolStripMenuItem autoDownmixDrums;
        private System.Windows.Forms.ToolStripMenuItem deleteCrowdAudio;
        private System.Windows.Forms.ToolStripMenuItem chkCreatePreview;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboRate;
    }
}