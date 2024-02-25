namespace Nautilus
{
    partial class FileIndexer
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearIndexedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findSongsWithoutWipeproofIDs = new System.Windows.Forms.ToolStripMenuItem();
            this.findSongsWithIDConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.findSongsWithDuplicateNames = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doubleclickToOpenInVisualizer = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstFolders = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkSubDirs = new System.Windows.Forms.CheckBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.lstSongs = new System.Windows.Forms.ListView();
            this.colSong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSongID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.onlyShowOtherSongs = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDisplayedSongs = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToJson = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveSelectedFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SendToCONExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.SendToVisualizer = new System.Windows.Forms.ToolStripMenuItem();
            this.SendToMIDICleaner = new System.Windows.Forms.ToolStripMenuItem();
            this.SendToSongAnalyzer = new System.Windows.Forms.ToolStripMenuItem();
            this.SendToAudioAnalyzer = new System.Windows.Forms.ToolStripMenuItem();
            this.SendToQuickPackEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.indexingWorker = new System.ComponentModel.BackgroundWorker();
            this.lblWorking = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.radioSongs = new System.Windows.Forms.RadioButton();
            this.radioPackages = new System.Windows.Forms.RadioButton();
            this.picPin = new System.Windows.Forms.PictureBox();
            this.filteringWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(616, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearIndexedFilesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // clearIndexedFilesToolStripMenuItem
            // 
            this.clearIndexedFilesToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.clearIndexedFilesToolStripMenuItem.Name = "clearIndexedFilesToolStripMenuItem";
            this.clearIndexedFilesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.clearIndexedFilesToolStripMenuItem.Text = "Clear indexed files";
            this.clearIndexedFilesToolStripMenuItem.Click += new System.EventHandler(this.clearIndexedFilesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findSongsWithoutWipeproofIDs,
            this.findSongsWithIDConflicts,
            this.findSongsWithDuplicateNames});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // findSongsWithoutWipeproofIDs
            // 
            this.findSongsWithoutWipeproofIDs.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.findSongsWithoutWipeproofIDs.Name = "findSongsWithoutWipeproofIDs";
            this.findSongsWithoutWipeproofIDs.Size = new System.Drawing.Size(256, 22);
            this.findSongsWithoutWipeproofIDs.Text = "Find songs without wipe-proof IDs";
            this.findSongsWithoutWipeproofIDs.Click += new System.EventHandler(this.findSongsWithoutWipeproofIDs_Click);
            // 
            // findSongsWithIDConflicts
            // 
            this.findSongsWithIDConflicts.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.findSongsWithIDConflicts.Name = "findSongsWithIDConflicts";
            this.findSongsWithIDConflicts.Size = new System.Drawing.Size(256, 22);
            this.findSongsWithIDConflicts.Text = "Find songs with ID conflicts";
            this.findSongsWithIDConflicts.Click += new System.EventHandler(this.findSongsWithIDConflicts_Click);
            // 
            // findSongsWithDuplicateNames
            // 
            this.findSongsWithDuplicateNames.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.findSongsWithDuplicateNames.Name = "findSongsWithDuplicateNames";
            this.findSongsWithDuplicateNames.Size = new System.Drawing.Size(256, 22);
            this.findSongsWithDuplicateNames.Text = "Find songs with duplicate names";
            this.findSongsWithDuplicateNames.Click += new System.EventHandler(this.findSongsWithDuplicateNames_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doubleclickToOpenInVisualizer});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // doubleclickToOpenInVisualizer
            // 
            this.doubleclickToOpenInVisualizer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.doubleclickToOpenInVisualizer.Checked = true;
            this.doubleclickToOpenInVisualizer.CheckOnClick = true;
            this.doubleclickToOpenInVisualizer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doubleclickToOpenInVisualizer.Name = "doubleclickToOpenInVisualizer";
            this.doubleclickToOpenInVisualizer.Size = new System.Drawing.Size(250, 22);
            this.doubleclickToOpenInVisualizer.Text = "Double-click to open in Visualizer";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // lstFolders
            // 
            this.lstFolders.AllowDrop = true;
            this.lstFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFolders.FormattingEnabled = true;
            this.lstFolders.Location = new System.Drawing.Point(12, 54);
            this.lstFolders.Name = "lstFolders";
            this.lstFolders.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstFolders.Size = new System.Drawing.Size(592, 134);
            this.lstFolders.Sorted = true;
            this.lstFolders.TabIndex = 1;
            this.lstFolders.SelectedIndexChanged += new System.EventHandler(this.lstFolders_SelectedIndexChanged);
            this.lstFolders.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.lstFolders.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Indexed Folders:";
            this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.label1.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnNew
            // 
            this.btnNew.AllowDrop = true;
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.Color.Black;
            this.btnNew.Location = new System.Drawing.Point(275, 25);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(109, 23);
            this.btnNew.TabIndex = 23;
            this.btnNew.Text = "Add New Folder";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            this.btnNew.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnNew.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnDelete
            // 
            this.btnDelete.AllowDrop = true;
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(390, 25);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(117, 23);
            this.btnDelete.TabIndex = 24;
            this.btnDelete.Text = "Remove Selected";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnDelete.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnDelete.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnClear
            // 
            this.btnClear.AllowDrop = true;
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Enabled = false;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(513, 25);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(91, 23);
            this.btnClear.TabIndex = 25;
            this.btnClear.Text = "Clear Folders";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnClear.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // chkSubDirs
            // 
            this.chkSubDirs.AllowDrop = true;
            this.chkSubDirs.AutoSize = true;
            this.chkSubDirs.Checked = true;
            this.chkSubDirs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubDirs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSubDirs.Location = new System.Drawing.Point(113, 201);
            this.chkSubDirs.Name = "chkSubDirs";
            this.chkSubDirs.Size = new System.Drawing.Size(128, 17);
            this.chkSubDirs.TabIndex = 26;
            this.chkSubDirs.Text = "Search subdirectories";
            this.chkSubDirs.UseVisualStyleBackColor = true;
            this.chkSubDirs.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.chkSubDirs.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnBuild
            // 
            this.btnBuild.AllowDrop = true;
            this.btnBuild.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnBuild.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuild.Enabled = false;
            this.btnBuild.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuild.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuild.ForeColor = System.Drawing.Color.Black;
            this.btnBuild.Location = new System.Drawing.Point(12, 194);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(95, 23);
            this.btnBuild.TabIndex = 27;
            this.btnBuild.Text = "Build Index";
            this.btnBuild.UseVisualStyleBackColor = false;
            this.btnBuild.EnabledChanged += new System.EventHandler(this.btnBuild_EnabledChanged);
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            this.btnBuild.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnBuild.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // lstSongs
            // 
            this.lstSongs.AllowDrop = true;
            this.lstSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSong,
            this.colSongID,
            this.colLocation});
            this.lstSongs.ContextMenuStrip = this.contextMenuStrip1;
            this.lstSongs.Enabled = false;
            this.lstSongs.FullRowSelect = true;
            this.lstSongs.HideSelection = false;
            this.lstSongs.Location = new System.Drawing.Point(12, 224);
            this.lstSongs.Name = "lstSongs";
            this.lstSongs.Size = new System.Drawing.Size(592, 220);
            this.lstSongs.TabIndex = 64;
            this.lstSongs.UseCompatibleStateImageBehavior = false;
            this.lstSongs.View = System.Windows.Forms.View.Details;
            this.lstSongs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstSongs_ColumnClick);
            this.lstSongs.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.lstSongs.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.lstSongs.DoubleClick += new System.EventHandler(this.lstSongs_DoubleClick);
            // 
            // colSong
            // 
            this.colSong.Text = "Song Name";
            this.colSong.Width = 250;
            // 
            // colSongID
            // 
            this.colSongID.Text = "Song ID";
            this.colSongID.Width = 100;
            // 
            // colLocation
            // 
            this.colLocation.Text = "File Location";
            this.colLocation.Width = 220;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolder,
            this.onlyShowOtherSongs,
            this.exportDisplayedSongs,
            this.exportToJson,
            this.deleteSelectedFileToolStripMenuItem,
            this.moveSelectedFiles,
            this.sendToMenu});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(255, 158);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // openFolder
            // 
            this.openFolder.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.openFolder.Name = "openFolder";
            this.openFolder.Size = new System.Drawing.Size(254, 22);
            this.openFolder.Text = "Open selected file in Windows Explorer";
            this.openFolder.Click += new System.EventHandler(this.openFolderThatContainsFileToolStripMenuItem_Click);
            // 
            // onlyShowOtherSongs
            // 
            this.onlyShowOtherSongs.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.onlyShowOtherSongs.Name = "onlyShowOtherSongs";
            this.onlyShowOtherSongs.Size = new System.Drawing.Size(254, 22);
            this.onlyShowOtherSongs.Text = "Only show songs found in this file";
            this.onlyShowOtherSongs.Click += new System.EventHandler(this.onlyShowOtherSongs_Click);
            // 
            // exportDisplayedSongs
            // 
            this.exportDisplayedSongs.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exportDisplayedSongs.Name = "exportDisplayedSongs";
            this.exportDisplayedSongs.Size = new System.Drawing.Size(254, 22);
            this.exportDisplayedSongs.Text = "Export displayed songs to CSV";
            this.exportDisplayedSongs.Click += new System.EventHandler(this.exportDisplayedSongs_Click);
            // 
            // exportToJson
            // 
            this.exportToJson.Name = "exportToJson";
            this.exportToJson.Size = new System.Drawing.Size(254, 22);
            this.exportToJson.Text = "Export displayed songs to Json";
            this.exportToJson.Click += new System.EventHandler(this.exportToJson_Click);
            // 
            // deleteSelectedFileToolStripMenuItem
            // 
            this.deleteSelectedFileToolStripMenuItem.Name = "deleteSelectedFileToolStripMenuItem";
            this.deleteSelectedFileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteSelectedFileToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.deleteSelectedFileToolStripMenuItem.Text = "Delete selected file(s)";
            this.deleteSelectedFileToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedFileToolStripMenuItem_Click);
            // 
            // moveSelectedFiles
            // 
            this.moveSelectedFiles.Name = "moveSelectedFiles";
            this.moveSelectedFiles.Size = new System.Drawing.Size(254, 22);
            this.moveSelectedFiles.Text = "Move selected file(s)";
            this.moveSelectedFiles.Click += new System.EventHandler(this.moveSelectedFiles_Click);
            // 
            // sendToMenu
            // 
            this.sendToMenu.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.sendToMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SendToCONExplorer,
            this.SendToVisualizer,
            this.SendToMIDICleaner,
            this.SendToSongAnalyzer,
            this.SendToAudioAnalyzer,
            this.SendToQuickPackEditor});
            this.sendToMenu.Name = "sendToMenu";
            this.sendToMenu.Size = new System.Drawing.Size(254, 22);
            this.sendToMenu.Text = "Send selected file to...";
            // 
            // SendToCONExplorer
            // 
            this.SendToCONExplorer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.SendToCONExplorer.Name = "SendToCONExplorer";
            this.SendToCONExplorer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.SendToCONExplorer.Size = new System.Drawing.Size(210, 22);
            this.SendToCONExplorer.Text = "CON Explorer";
            this.SendToCONExplorer.Click += new System.EventHandler(this.SendToCONExplorer_Click);
            // 
            // SendToVisualizer
            // 
            this.SendToVisualizer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.SendToVisualizer.Name = "SendToVisualizer";
            this.SendToVisualizer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.SendToVisualizer.Size = new System.Drawing.Size(210, 22);
            this.SendToVisualizer.Text = "Visualizer";
            this.SendToVisualizer.Click += new System.EventHandler(this.SendToVisualizer_Click);
            // 
            // SendToMIDICleaner
            // 
            this.SendToMIDICleaner.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.SendToMIDICleaner.Name = "SendToMIDICleaner";
            this.SendToMIDICleaner.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.SendToMIDICleaner.Size = new System.Drawing.Size(210, 22);
            this.SendToMIDICleaner.Text = "MIDI Cleaner";
            this.SendToMIDICleaner.Click += new System.EventHandler(this.SendToMIDICleaner_Click);
            // 
            // SendToSongAnalyzer
            // 
            this.SendToSongAnalyzer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.SendToSongAnalyzer.Name = "SendToSongAnalyzer";
            this.SendToSongAnalyzer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SendToSongAnalyzer.Size = new System.Drawing.Size(210, 22);
            this.SendToSongAnalyzer.Text = "Song Analyzer";
            this.SendToSongAnalyzer.Click += new System.EventHandler(this.SendToSongAnalyzer_Click);
            // 
            // SendToAudioAnalyzer
            // 
            this.SendToAudioAnalyzer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.SendToAudioAnalyzer.Name = "SendToAudioAnalyzer";
            this.SendToAudioAnalyzer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.SendToAudioAnalyzer.Size = new System.Drawing.Size(210, 22);
            this.SendToAudioAnalyzer.Text = "Audio Analyzer";
            this.SendToAudioAnalyzer.Click += new System.EventHandler(this.SendToAudioAnalyzer_Click);
            // 
            // SendToQuickPackEditor
            // 
            this.SendToQuickPackEditor.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.SendToQuickPackEditor.Name = "SendToQuickPackEditor";
            this.SendToQuickPackEditor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.SendToQuickPackEditor.Size = new System.Drawing.Size(210, 22);
            this.SendToQuickPackEditor.Text = "Quick Pack Editor";
            this.SendToQuickPackEditor.Click += new System.EventHandler(this.SendToQuickPackEditor_Click);
            // 
            // indexingWorker
            // 
            this.indexingWorker.WorkerReportsProgress = true;
            this.indexingWorker.WorkerSupportsCancellation = true;
            this.indexingWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.indexingWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // lblWorking
            // 
            this.lblWorking.AllowDrop = true;
            this.lblWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWorking.Location = new System.Drawing.Point(275, 198);
            this.lblWorking.Name = "lblWorking";
            this.lblWorking.Size = new System.Drawing.Size(329, 21);
            this.lblWorking.TabIndex = 66;
            this.lblWorking.Text = "Working...";
            this.lblWorking.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblWorking.Visible = false;
            this.lblWorking.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.lblWorking.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // txtSearch
            // 
            this.txtSearch.AllowDrop = true;
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Enabled = false;
            this.txtSearch.ForeColor = System.Drawing.Color.LightGray;
            this.txtSearch.Location = new System.Drawing.Point(12, 452);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(354, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Text = "Type to search...";
            this.txtSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSearch_MouseClick);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.txtSearch.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.AllowDrop = true;
            this.btnClearSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(211)))), ((int)(((byte)(0)))));
            this.btnClearSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearSearch.Enabled = false;
            this.btnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSearch.ForeColor = System.Drawing.Color.Black;
            this.btnClearSearch.Location = new System.Drawing.Point(512, 450);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(92, 23);
            this.btnClearSearch.TabIndex = 68;
            this.btnClearSearch.Text = "Clear Search";
            this.btnClearSearch.UseVisualStyleBackColor = false;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            this.btnClearSearch.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.btnClearSearch.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // picWorking
            // 
            this.picWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(256, 5);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 63;
            this.picWorking.TabStop = false;
            // 
            // radioSongs
            // 
            this.radioSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radioSongs.AutoSize = true;
            this.radioSongs.Checked = true;
            this.radioSongs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioSongs.Enabled = false;
            this.radioSongs.Location = new System.Drawing.Point(373, 454);
            this.radioSongs.Name = "radioSongs";
            this.radioSongs.Size = new System.Drawing.Size(55, 17);
            this.radioSongs.TabIndex = 69;
            this.radioSongs.TabStop = true;
            this.radioSongs.Text = "Songs";
            this.radioSongs.UseVisualStyleBackColor = true;
            this.radioSongs.CheckedChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // radioPackages
            // 
            this.radioPackages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radioPackages.AutoSize = true;
            this.radioPackages.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioPackages.Enabled = false;
            this.radioPackages.Location = new System.Drawing.Point(434, 454);
            this.radioPackages.Name = "radioPackages";
            this.radioPackages.Size = new System.Drawing.Size(73, 17);
            this.radioPackages.TabIndex = 70;
            this.radioPackages.Text = "Packages";
            this.radioPackages.UseVisualStyleBackColor = true;
            this.radioPackages.CheckedChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(592, 3);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 71;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // filteringWorker
            // 
            this.filteringWorker.WorkerReportsProgress = true;
            this.filteringWorker.WorkerSupportsCancellation = true;
            this.filteringWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.filteringWorker_DoWork);
            this.filteringWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.filteringWorker_RunWorkerCompleted);
            // 
            // FileIndexer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(616, 481);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.radioPackages);
            this.Controls.Add(this.radioSongs);
            this.Controls.Add(this.btnClearSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblWorking);
            this.Controls.Add(this.lstSongs);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.chkSubDirs);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstFolders);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FileIndexer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Indexer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileIndexer_FormClosing);
            this.Shown += new System.EventHandler(this.FileIndexer_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.Resize += new System.EventHandler(this.FileIndexer_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ListBox lstFolders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkSubDirs;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.ListView lstSongs;
        private System.Windows.Forms.ColumnHeader colSong;
        private System.Windows.Forms.ColumnHeader colLocation;
        private System.ComponentModel.BackgroundWorker indexingWorker;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openFolder;
        private System.Windows.Forms.Label lblWorking;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.ToolStripMenuItem clearIndexedFilesToolStripMenuItem;
        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.ToolStripMenuItem onlyShowOtherSongs;
        private System.Windows.Forms.RadioButton radioSongs;
        private System.Windows.Forms.RadioButton radioPackages;
        private System.Windows.Forms.ToolStripMenuItem exportDisplayedSongs;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.ToolStripMenuItem sendToMenu;
        private System.Windows.Forms.ToolStripMenuItem SendToCONExplorer;
        private System.Windows.Forms.ToolStripMenuItem SendToVisualizer;
        private System.Windows.Forms.ToolStripMenuItem SendToMIDICleaner;
        private System.Windows.Forms.ToolStripMenuItem SendToSongAnalyzer;
        private System.Windows.Forms.ToolStripMenuItem SendToAudioAnalyzer;
        private System.Windows.Forms.ToolStripMenuItem SendToQuickPackEditor;
        private System.Windows.Forms.ColumnHeader colSongID;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findSongsWithoutWipeproofIDs;
        private System.ComponentModel.BackgroundWorker filteringWorker;
        private System.Windows.Forms.ToolStripMenuItem findSongsWithIDConflicts;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findSongsWithDuplicateNames;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doubleclickToOpenInVisualizer;
        private System.Windows.Forms.ToolStripMenuItem moveSelectedFiles;
        private System.Windows.Forms.ToolStripMenuItem exportToJson;
    }
}