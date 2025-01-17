namespace Nautilus
{
    partial class BatchExtractor
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
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnDeselect = new System.Windows.Forms.Button();
            this.btnConverter = new System.Windows.Forms.Button();
            this.picPin = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.renamingOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.songIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.artistSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.artistTheSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.songArtistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.songArtistTheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSpacesFromFileName = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceSpacesWithUnderscores = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.appendsongsToFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.removekeepFromPNGXBOXFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.organizeFilesByType = new System.Windows.Forms.ToolStripMenuItem();
            this.chkDTA = new System.Windows.Forms.CheckBox();
            this.chkMIDI = new System.Windows.Forms.CheckBox();
            this.chkMOGG = new System.Windows.Forms.CheckBox();
            this.chkMILO = new System.Windows.Forms.CheckBox();
            this.FileExtractor = new System.ComponentModel.BackgroundWorker();
            this.chkPNG = new System.Windows.Forms.CheckBox();
            this.chkThumbs = new System.Windows.Forms.CheckBox();
            this.folderScanner = new System.ComponentModel.BackgroundWorker();
            this.yARGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToYARG = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.AllowDrop = true;
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.Black;
            this.btnRefresh.Location = new System.Drawing.Point(488, 32);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(114, 30);
            this.btnRefresh.TabIndex = 18;
            this.btnRefresh.Text = "&Refresh Folder";
            this.toolTip1.SetToolTip(this.btnRefresh, "Click to rescan the input folder");
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Visible = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnRefresh.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnFolder
            // 
            this.btnFolder.AllowDrop = true;
            this.btnFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFolder.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFolder.ForeColor = System.Drawing.Color.Black;
            this.btnFolder.Location = new System.Drawing.Point(12, 32);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(137, 30);
            this.btnFolder.TabIndex = 17;
            this.btnFolder.Text = "Select &Input Folder";
            this.toolTip1.SetToolTip(this.btnFolder, "Click to select the folder where your files are");
            this.btnFolder.UseVisualStyleBackColor = false;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            this.btnFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // txtFolder
            // 
            this.txtFolder.AllowDrop = true;
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
            this.lstLog.Size = new System.Drawing.Size(590, 340);
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
            this.btnBegin.AllowDrop = true;
            this.btnBegin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnBegin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBegin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBegin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBegin.ForeColor = System.Drawing.Color.Black;
            this.btnBegin.Location = new System.Drawing.Point(517, 470);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(85, 30);
            this.btnBegin.TabIndex = 20;
            this.btnBegin.Text = "&Extract";
            this.toolTip1.SetToolTip(this.btnBegin, "Click to begin extracting files");
            this.btnBegin.UseVisualStyleBackColor = false;
            this.btnBegin.Visible = false;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            this.btnBegin.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnBegin.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // picWorking
            // 
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(252, 47);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 59;
            this.picWorking.TabStop = false;
            this.toolTip1.SetToolTip(this.picWorking, "Working...");
            this.picWorking.Visible = false;
            // 
            // btnSelect
            // 
            this.btnSelect.AllowDrop = true;
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.ForeColor = System.Drawing.Color.Black;
            this.btnSelect.Location = new System.Drawing.Point(12, 470);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(85, 30);
            this.btnSelect.TabIndex = 62;
            this.btnSelect.Text = "Select All";
            this.toolTip1.SetToolTip(this.btnSelect, "Click to select all file types");
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnDeselect
            // 
            this.btnDeselect.AllowDrop = true;
            this.btnDeselect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnDeselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeselect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeselect.ForeColor = System.Drawing.Color.Black;
            this.btnDeselect.Location = new System.Drawing.Point(115, 470);
            this.btnDeselect.Name = "btnDeselect";
            this.btnDeselect.Size = new System.Drawing.Size(85, 30);
            this.btnDeselect.TabIndex = 63;
            this.btnDeselect.Text = "Deselect All";
            this.toolTip1.SetToolTip(this.btnDeselect, "Click to deselect all file types");
            this.btnDeselect.UseVisualStyleBackColor = false;
            this.btnDeselect.Click += new System.EventHandler(this.btnDeselect_Click);
            // 
            // btnConverter
            // 
            this.btnConverter.AllowDrop = true;
            this.btnConverter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnConverter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConverter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConverter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConverter.ForeColor = System.Drawing.Color.Black;
            this.btnConverter.Location = new System.Drawing.Point(343, 470);
            this.btnConverter.Name = "btnConverter";
            this.btnConverter.Size = new System.Drawing.Size(157, 30);
            this.btnConverter.TabIndex = 64;
            this.btnConverter.Text = "&Advanced Art Converter";
            this.toolTip1.SetToolTip(this.btnConverter, "Click to open Advanced Art Converter");
            this.btnConverter.UseVisualStyleBackColor = false;
            this.btnConverter.Visible = false;
            this.btnConverter.Click += new System.EventHandler(this.btnConverter_Click);
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(589, 5);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 65;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.toolTip1.SetToolTip(this.picPin, "Click to pin on top");
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renamingOptionsToolStripMenuItem,
            this.yARGToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(614, 24);
            this.menuStrip1.TabIndex = 26;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // renamingOptionsToolStripMenuItem
            // 
            this.renamingOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameFilesToolStripMenuItem,
            this.removeSpacesFromFileName,
            this.replaceSpacesWithUnderscores,
            this.toolStripMenuItem1,
            this.appendsongsToFiles,
            this.removekeepFromPNGXBOXFiles,
            this.toolStripMenuItem2,
            this.organizeFilesByType});
            this.renamingOptionsToolStripMenuItem.Name = "renamingOptionsToolStripMenuItem";
            this.renamingOptionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.renamingOptionsToolStripMenuItem.Text = "&Options";
            // 
            // renameFilesToolStripMenuItem
            // 
            this.renameFilesToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.renameFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.songIDToolStripMenuItem,
            this.artistSongToolStripMenuItem,
            this.artistTheSongToolStripMenuItem,
            this.songArtistToolStripMenuItem,
            this.songArtistTheToolStripMenuItem});
            this.renameFilesToolStripMenuItem.Name = "renameFilesToolStripMenuItem";
            this.renameFilesToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.renameFilesToolStripMenuItem.Text = "Naming format";
            // 
            // songIDToolStripMenuItem
            // 
            this.songIDToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.songIDToolStripMenuItem.CheckOnClick = true;
            this.songIDToolStripMenuItem.Name = "songIDToolStripMenuItem";
            this.songIDToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.songIDToolStripMenuItem.Text = "InternalName";
            this.songIDToolStripMenuItem.Click += new System.EventHandler(this.songIDToolStripMenuItem_Click);
            // 
            // artistSongToolStripMenuItem
            // 
            this.artistSongToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.artistSongToolStripMenuItem.Checked = true;
            this.artistSongToolStripMenuItem.CheckOnClick = true;
            this.artistSongToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.artistSongToolStripMenuItem.Name = "artistSongToolStripMenuItem";
            this.artistSongToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.artistSongToolStripMenuItem.Text = "The Artist - Song";
            this.artistSongToolStripMenuItem.Click += new System.EventHandler(this.artistSongToolStripMenuItem_Click);
            // 
            // artistTheSongToolStripMenuItem
            // 
            this.artistTheSongToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.artistTheSongToolStripMenuItem.CheckOnClick = true;
            this.artistTheSongToolStripMenuItem.Name = "artistTheSongToolStripMenuItem";
            this.artistTheSongToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.artistTheSongToolStripMenuItem.Text = "Artist, The - Song";
            this.artistTheSongToolStripMenuItem.Click += new System.EventHandler(this.artistTheSongToolStripMenuItem_Click);
            // 
            // songArtistToolStripMenuItem
            // 
            this.songArtistToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.songArtistToolStripMenuItem.CheckOnClick = true;
            this.songArtistToolStripMenuItem.Name = "songArtistToolStripMenuItem";
            this.songArtistToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.songArtistToolStripMenuItem.Text = "Song - The Artist";
            this.songArtistToolStripMenuItem.Click += new System.EventHandler(this.songArtistToolStripMenuItem_Click);
            // 
            // songArtistTheToolStripMenuItem
            // 
            this.songArtistTheToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.songArtistTheToolStripMenuItem.CheckOnClick = true;
            this.songArtistTheToolStripMenuItem.Name = "songArtistTheToolStripMenuItem";
            this.songArtistTheToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.songArtistTheToolStripMenuItem.Text = "Song - Artist, The";
            this.songArtistTheToolStripMenuItem.Click += new System.EventHandler(this.songArtistTheToolStripMenuItem_Click);
            // 
            // removeSpacesFromFileName
            // 
            this.removeSpacesFromFileName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.removeSpacesFromFileName.CheckOnClick = true;
            this.removeSpacesFromFileName.Name = "removeSpacesFromFileName";
            this.removeSpacesFromFileName.Size = new System.Drawing.Size(267, 22);
            this.removeSpacesFromFileName.Text = "Remove spaces from file names";
            this.removeSpacesFromFileName.Click += new System.EventHandler(this.removeSpacesFromFileNameToolStripMenuItem_Click);
            // 
            // replaceSpacesWithUnderscores
            // 
            this.replaceSpacesWithUnderscores.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.replaceSpacesWithUnderscores.CheckOnClick = true;
            this.replaceSpacesWithUnderscores.Name = "replaceSpacesWithUnderscores";
            this.replaceSpacesWithUnderscores.Size = new System.Drawing.Size(267, 22);
            this.replaceSpacesWithUnderscores.Text = "Replace spaces with underscores";
            this.replaceSpacesWithUnderscores.Click += new System.EventHandler(this.replaceSpacesWithUnderscoresToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(264, 6);
            // 
            // appendsongsToFiles
            // 
            this.appendsongsToFiles.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.appendsongsToFiles.CheckOnClick = true;
            this.appendsongsToFiles.Name = "appendsongsToFiles";
            this.appendsongsToFiles.Size = new System.Drawing.Size(267, 22);
            this.appendsongsToFiles.Text = "Append \'_songs\' to .dta files";
            // 
            // removekeepFromPNGXBOXFiles
            // 
            this.removekeepFromPNGXBOXFiles.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.removekeepFromPNGXBOXFiles.Checked = true;
            this.removekeepFromPNGXBOXFiles.CheckOnClick = true;
            this.removekeepFromPNGXBOXFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removekeepFromPNGXBOXFiles.Name = "removekeepFromPNGXBOXFiles";
            this.removekeepFromPNGXBOXFiles.Size = new System.Drawing.Size(267, 22);
            this.removekeepFromPNGXBOXFiles.Text = "Remove \'_keep\' from .png_xbox files";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(264, 6);
            // 
            // organizeFilesByType
            // 
            this.organizeFilesByType.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.organizeFilesByType.Checked = true;
            this.organizeFilesByType.CheckOnClick = true;
            this.organizeFilesByType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.organizeFilesByType.Name = "organizeFilesByType";
            this.organizeFilesByType.Size = new System.Drawing.Size(267, 22);
            this.organizeFilesByType.Text = "Organize files by type";
            // 
            // chkDTA
            // 
            this.chkDTA.AutoSize = true;
            this.chkDTA.BackColor = System.Drawing.Color.Transparent;
            this.chkDTA.Checked = true;
            this.chkDTA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDTA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDTA.Location = new System.Drawing.Point(12, 443);
            this.chkDTA.Name = "chkDTA";
            this.chkDTA.Size = new System.Drawing.Size(72, 17);
            this.chkDTA.TabIndex = 27;
            this.chkDTA.Text = "DTA Files";
            this.chkDTA.UseVisualStyleBackColor = false;
            this.chkDTA.CheckedChanged += new System.EventHandler(this.chkDTA_CheckedChanged);
            // 
            // chkMIDI
            // 
            this.chkMIDI.AutoSize = true;
            this.chkMIDI.BackColor = System.Drawing.Color.Transparent;
            this.chkMIDI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkMIDI.Location = new System.Drawing.Point(198, 443);
            this.chkMIDI.Name = "chkMIDI";
            this.chkMIDI.Size = new System.Drawing.Size(73, 17);
            this.chkMIDI.TabIndex = 28;
            this.chkMIDI.Text = "MIDI Files";
            this.chkMIDI.UseVisualStyleBackColor = false;
            this.chkMIDI.CheckedChanged += new System.EventHandler(this.chkDTA_CheckedChanged);
            // 
            // chkMOGG
            // 
            this.chkMOGG.AutoSize = true;
            this.chkMOGG.BackColor = System.Drawing.Color.Transparent;
            this.chkMOGG.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkMOGG.Location = new System.Drawing.Point(274, 443);
            this.chkMOGG.Name = "chkMOGG";
            this.chkMOGG.Size = new System.Drawing.Size(83, 17);
            this.chkMOGG.TabIndex = 29;
            this.chkMOGG.Text = "MOGG Files";
            this.chkMOGG.UseVisualStyleBackColor = false;
            this.chkMOGG.CheckedChanged += new System.EventHandler(this.chkDTA_CheckedChanged);
            // 
            // chkMILO
            // 
            this.chkMILO.AutoSize = true;
            this.chkMILO.BackColor = System.Drawing.Color.Transparent;
            this.chkMILO.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkMILO.Location = new System.Drawing.Point(360, 443);
            this.chkMILO.Name = "chkMILO";
            this.chkMILO.Size = new System.Drawing.Size(111, 17);
            this.chkMILO.TabIndex = 30;
            this.chkMILO.Text = "MILO_XBOX Files";
            this.chkMILO.UseVisualStyleBackColor = false;
            this.chkMILO.CheckedChanged += new System.EventHandler(this.chkDTA_CheckedChanged);
            // 
            // FileExtractor
            // 
            this.FileExtractor.WorkerReportsProgress = true;
            this.FileExtractor.WorkerSupportsCancellation = true;
            this.FileExtractor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.FileExtractor_DoWork);
            this.FileExtractor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.FileExtractor_RunWorkerCompleted);
            // 
            // chkPNG
            // 
            this.chkPNG.AutoSize = true;
            this.chkPNG.BackColor = System.Drawing.Color.Transparent;
            this.chkPNG.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkPNG.Location = new System.Drawing.Point(87, 443);
            this.chkPNG.Name = "chkPNG";
            this.chkPNG.Size = new System.Drawing.Size(108, 17);
            this.chkPNG.TabIndex = 60;
            this.chkPNG.Text = "PNG_XBOX Files";
            this.chkPNG.UseVisualStyleBackColor = false;
            this.chkPNG.CheckedChanged += new System.EventHandler(this.chkDTA_CheckedChanged);
            // 
            // chkThumbs
            // 
            this.chkThumbs.AutoSize = true;
            this.chkThumbs.BackColor = System.Drawing.Color.Transparent;
            this.chkThumbs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkThumbs.Location = new System.Drawing.Point(474, 443);
            this.chkThumbs.Name = "chkThumbs";
            this.chkThumbs.Size = new System.Drawing.Size(126, 17);
            this.chkThumbs.TabIndex = 61;
            this.chkThumbs.Text = "Package Thumbnails";
            this.chkThumbs.UseVisualStyleBackColor = false;
            this.chkThumbs.CheckedChanged += new System.EventHandler(this.chkDTA_CheckedChanged);
            // 
            // folderScanner
            // 
            this.folderScanner.WorkerReportsProgress = true;
            this.folderScanner.WorkerSupportsCancellation = true;
            this.folderScanner.DoWork += new System.ComponentModel.DoWorkEventHandler(this.folderScanner_DoWork);
            this.folderScanner.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.folderScanner_RunWorkerCompleted);
            // 
            // yARGToolStripMenuItem
            // 
            this.yARGToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToYARG});
            this.yARGToolStripMenuItem.Name = "yARGToolStripMenuItem";
            this.yARGToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.yARGToolStripMenuItem.Text = "YARG";
            // 
            // extractToYARG
            // 
            this.extractToYARG.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.extractToYARG.CheckOnClick = true;
            this.extractToYARG.Name = "extractToYARG";
            this.extractToYARG.Size = new System.Drawing.Size(236, 22);
            this.extractToYARG.Text = "Extract to YARG exCON format";
            this.extractToYARG.Click += new System.EventHandler(this.extractToYARG_Click);
            // 
            // BatchExtractor
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(614, 511);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.btnConverter);
            this.Controls.Add(this.btnDeselect);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.chkThumbs);
            this.Controls.Add(this.chkPNG);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.chkMILO);
            this.Controls.Add(this.chkMOGG);
            this.Controls.Add(this.chkMIDI);
            this.Controls.Add(this.chkDTA);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnBegin);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.txtFolder);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BatchExtractor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batch Extractor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BatchExtractor_FormClosing);
            this.Shown += new System.EventHandler(this.BatchExtractor_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem renamingOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem artistSongToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem artistTheSongToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem songArtistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem songArtistTheToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceSpacesWithUnderscores;
        private System.Windows.Forms.ToolStripMenuItem songIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSpacesFromFileName;
        private System.Windows.Forms.ToolStripMenuItem appendsongsToFiles;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportLogFileToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkDTA;
        private System.Windows.Forms.CheckBox chkMIDI;
        private System.Windows.Forms.CheckBox chkMOGG;
        private System.Windows.Forms.CheckBox chkMILO;
        private System.Windows.Forms.PictureBox picWorking;
        private System.ComponentModel.BackgroundWorker FileExtractor;
        private System.Windows.Forms.CheckBox chkPNG;
        private System.Windows.Forms.CheckBox chkThumbs;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDeselect;
        private System.Windows.Forms.ToolStripMenuItem removekeepFromPNGXBOXFiles;
        private System.Windows.Forms.ToolStripMenuItem organizeFilesByType;
        private System.Windows.Forms.Button btnConverter;
        private System.ComponentModel.BackgroundWorker folderScanner;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.ToolStripMenuItem yARGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractToYARG;
    }
}