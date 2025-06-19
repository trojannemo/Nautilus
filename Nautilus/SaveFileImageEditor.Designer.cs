namespace Nautilus
{
    partial class SaveFileImageEditor
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
            this.picArt = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replaceArtImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picCharacter = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replaceCharacterImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCompositeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboArt = new System.Windows.Forms.ComboBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSaveGameFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveChangesToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offsetFix = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportArt = new System.Windows.Forms.Button();
            this.btnReplaceArt = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboBackgrounds = new System.Windows.Forms.ComboBox();
            this.txtBand = new System.Windows.Forms.TextBox();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnExportChar = new System.Windows.Forms.Button();
            this.btnReplaceChar = new System.Windows.Forms.Button();
            this.cboCharacter = new System.Windows.Forms.ComboBox();
            this.picPin = new System.Windows.Forms.PictureBox();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblConsole = new System.Windows.Forms.Label();
            this.lblCharacter = new System.Windows.Forms.Label();
            this.lblArt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picArt)).BeginInit();
            this.contextMenuStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            this.SuspendLayout();
            // 
            // picArt
            // 
            this.picArt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picArt.ContextMenuStrip = this.contextMenuStrip3;
            this.picArt.Location = new System.Drawing.Point(274, 206);
            this.picArt.Name = "picArt";
            this.picArt.Size = new System.Drawing.Size(256, 256);
            this.picArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picArt.TabIndex = 1;
            this.picArt.TabStop = false;
            this.picArt.DragDrop += new System.Windows.Forms.DragEventHandler(this.picArt_DragDrop);
            this.picArt.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceArtImageToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.ShowImageMargin = false;
            this.contextMenuStrip3.Size = new System.Drawing.Size(144, 26);
            this.contextMenuStrip3.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip3_Opening);
            // 
            // replaceArtImageToolStripMenuItem
            // 
            this.replaceArtImageToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.replaceArtImageToolStripMenuItem.Enabled = false;
            this.replaceArtImageToolStripMenuItem.Name = "replaceArtImageToolStripMenuItem";
            this.replaceArtImageToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.replaceArtImageToolStripMenuItem.Text = "Replace art image";
            this.replaceArtImageToolStripMenuItem.Click += new System.EventHandler(this.replaceArtImageToolStripMenuItem_Click);
            // 
            // picCharacter
            // 
            this.picCharacter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCharacter.ContextMenuStrip = this.contextMenuStrip2;
            this.picCharacter.Location = new System.Drawing.Point(12, 27);
            this.picCharacter.Name = "picCharacter";
            this.picCharacter.Size = new System.Drawing.Size(256, 512);
            this.picCharacter.TabIndex = 0;
            this.picCharacter.TabStop = false;
            this.picCharacter.DragDrop += new System.Windows.Forms.DragEventHandler(this.picCharacter_DragDrop);
            this.picCharacter.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceCharacterImageToolStripMenuItem,
            this.exportCompositeImageToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.ShowImageMargin = false;
            this.contextMenuStrip2.Size = new System.Drawing.Size(179, 48);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // replaceCharacterImageToolStripMenuItem
            // 
            this.replaceCharacterImageToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.replaceCharacterImageToolStripMenuItem.Enabled = false;
            this.replaceCharacterImageToolStripMenuItem.Name = "replaceCharacterImageToolStripMenuItem";
            this.replaceCharacterImageToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.replaceCharacterImageToolStripMenuItem.Text = "Replace character image";
            this.replaceCharacterImageToolStripMenuItem.Click += new System.EventHandler(this.replaceCharacterImageToolStripMenuItem_Click);
            // 
            // exportCompositeImageToolStripMenuItem
            // 
            this.exportCompositeImageToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exportCompositeImageToolStripMenuItem.Name = "exportCompositeImageToolStripMenuItem";
            this.exportCompositeImageToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.exportCompositeImageToolStripMenuItem.Text = "Export composite image";
            this.exportCompositeImageToolStripMenuItem.Click += new System.EventHandler(this.exportCompositeImageToolStripMenuItem_Click);
            // 
            // cboArt
            // 
            this.cboArt.Enabled = false;
            this.cboArt.FormattingEnabled = true;
            this.cboArt.Location = new System.Drawing.Point(274, 178);
            this.cboArt.Name = "cboArt";
            this.cboArt.Size = new System.Drawing.Size(256, 21);
            this.cboArt.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cboArt, "Choose art image");
            this.cboArt.SelectedIndexChanged += new System.EventHandler(this.cboArt_SelectedIndexChanged);
            // 
            // lstLog
            // 
            this.lstLog.AllowDrop = true;
            this.lstLog.BackColor = System.Drawing.Color.White;
            this.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLog.ContextMenuStrip = this.contextMenuStrip1;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Location = new System.Drawing.Point(12, 545);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(518, 132);
            this.lstLog.TabIndex = 4;
            this.toolTip1.SetToolTip(this.lstLog, "This is the program log. Right click to export");
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
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(542, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSaveGameFileToolStripMenuItem,
            this.exportAllImagesToolStripMenuItem,
            this.saveChangesToFileToolStripMenuItem,
            this.closeFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openSaveGameFileToolStripMenuItem
            // 
            this.openSaveGameFileToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.openSaveGameFileToolStripMenuItem.Name = "openSaveGameFileToolStripMenuItem";
            this.openSaveGameFileToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.openSaveGameFileToolStripMenuItem.Text = "&Open file";
            this.openSaveGameFileToolStripMenuItem.Click += new System.EventHandler(this.openSaveGameFileToolStripMenuItem_Click);
            // 
            // exportAllImagesToolStripMenuItem
            // 
            this.exportAllImagesToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exportAllImagesToolStripMenuItem.Enabled = false;
            this.exportAllImagesToolStripMenuItem.Name = "exportAllImagesToolStripMenuItem";
            this.exportAllImagesToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.exportAllImagesToolStripMenuItem.Text = "Export all images";
            this.exportAllImagesToolStripMenuItem.Click += new System.EventHandler(this.exportAllImagesToolStripMenuItem_Click);
            // 
            // saveChangesToFileToolStripMenuItem
            // 
            this.saveChangesToFileToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.saveChangesToFileToolStripMenuItem.Name = "saveChangesToFileToolStripMenuItem";
            this.saveChangesToFileToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.saveChangesToFileToolStripMenuItem.Text = "&Save changes to file";
            this.saveChangesToFileToolStripMenuItem.Click += new System.EventHandler(this.saveChangesToFileToolStripMenuItem_Click);
            // 
            // closeFileToolStripMenuItem
            // 
            this.closeFileToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.closeFileToolStripMenuItem.Enabled = false;
            this.closeFileToolStripMenuItem.Name = "closeFileToolStripMenuItem";
            this.closeFileToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.closeFileToolStripMenuItem.Text = "&Close file";
            this.closeFileToolStripMenuItem.Click += new System.EventHandler(this.closeFileToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.offsetFix});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Visible = false;
            // 
            // offsetFix
            // 
            this.offsetFix.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.offsetFix.CheckOnClick = true;
            this.offsetFix.Name = "offsetFix";
            this.offsetFix.Size = new System.Drawing.Size(194, 22);
            this.offsetFix.Text = "ScientistsSay Offset Fix";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // btnExportArt
            // 
            this.btnExportArt.AllowDrop = true;
            this.btnExportArt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(84)))), ((int)(((byte)(86)))));
            this.btnExportArt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportArt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExportArt.Enabled = false;
            this.btnExportArt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportArt.ForeColor = System.Drawing.Color.White;
            this.btnExportArt.Location = new System.Drawing.Point(274, 468);
            this.btnExportArt.Name = "btnExportArt";
            this.btnExportArt.Size = new System.Drawing.Size(67, 30);
            this.btnExportArt.TabIndex = 11;
            this.btnExportArt.Text = "Export";
            this.toolTip1.SetToolTip(this.btnExportArt, "Export current art image");
            this.btnExportArt.UseVisualStyleBackColor = false;
            this.btnExportArt.Click += new System.EventHandler(this.btnExportArt_Click);
            // 
            // btnReplaceArt
            // 
            this.btnReplaceArt.AllowDrop = true;
            this.btnReplaceArt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(84)))), ((int)(((byte)(86)))));
            this.btnReplaceArt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReplaceArt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReplaceArt.Enabled = false;
            this.btnReplaceArt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplaceArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReplaceArt.ForeColor = System.Drawing.Color.White;
            this.btnReplaceArt.Location = new System.Drawing.Point(463, 468);
            this.btnReplaceArt.Name = "btnReplaceArt";
            this.btnReplaceArt.Size = new System.Drawing.Size(67, 30);
            this.btnReplaceArt.TabIndex = 10;
            this.btnReplaceArt.Text = "Replace";
            this.toolTip1.SetToolTip(this.btnReplaceArt, "Replace current art image");
            this.btnReplaceArt.UseVisualStyleBackColor = false;
            this.btnReplaceArt.Click += new System.EventHandler(this.btnReplaceArt_Click);
            // 
            // cboBackgrounds
            // 
            this.cboBackgrounds.FormattingEnabled = true;
            this.cboBackgrounds.Items.AddRange(new object[] {
            "No background"});
            this.cboBackgrounds.Location = new System.Drawing.Point(274, 518);
            this.cboBackgrounds.Name = "cboBackgrounds";
            this.cboBackgrounds.Size = new System.Drawing.Size(256, 21);
            this.cboBackgrounds.TabIndex = 12;
            this.toolTip1.SetToolTip(this.cboBackgrounds, "Choose a background image for your character");
            this.cboBackgrounds.SelectedIndexChanged += new System.EventHandler(this.cboBackgrounds_SelectedIndexChanged);
            // 
            // txtBand
            // 
            this.txtBand.Enabled = false;
            this.txtBand.Location = new System.Drawing.Point(274, 139);
            this.txtBand.MaxLength = 31;
            this.txtBand.Name = "txtBand";
            this.txtBand.Size = new System.Drawing.Size(256, 20);
            this.txtBand.TabIndex = 69;
            this.toolTip1.SetToolTip(this.txtBand, "Click to change your band\'s name");
            this.txtBand.TextChanged += new System.EventHandler(this.txtBand_TextChanged);
            // 
            // btnRename
            // 
            this.btnRename.AllowDrop = true;
            this.btnRename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(84)))), ((int)(((byte)(86)))));
            this.btnRename.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRename.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRename.Enabled = false;
            this.btnRename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRename.ForeColor = System.Drawing.Color.White;
            this.btnRename.Location = new System.Drawing.Point(21, 64);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(67, 30);
            this.btnRename.TabIndex = 73;
            this.btnRename.Text = "Rename";
            this.toolTip1.SetToolTip(this.btnRename, "Rename current character");
            this.btnRename.UseVisualStyleBackColor = false;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnExportChar
            // 
            this.btnExportChar.AllowDrop = true;
            this.btnExportChar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(84)))), ((int)(((byte)(86)))));
            this.btnExportChar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportChar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExportChar.Enabled = false;
            this.btnExportChar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportChar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportChar.ForeColor = System.Drawing.Color.White;
            this.btnExportChar.Location = new System.Drawing.Point(107, 64);
            this.btnExportChar.Name = "btnExportChar";
            this.btnExportChar.Size = new System.Drawing.Size(67, 30);
            this.btnExportChar.TabIndex = 72;
            this.btnExportChar.Text = "Export";
            this.toolTip1.SetToolTip(this.btnExportChar, "Export current character image");
            this.btnExportChar.UseVisualStyleBackColor = false;
            this.btnExportChar.Click += new System.EventHandler(this.btnExportChar_Click);
            // 
            // btnReplaceChar
            // 
            this.btnReplaceChar.AllowDrop = true;
            this.btnReplaceChar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(84)))), ((int)(((byte)(86)))));
            this.btnReplaceChar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReplaceChar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReplaceChar.Enabled = false;
            this.btnReplaceChar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplaceChar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReplaceChar.ForeColor = System.Drawing.Color.White;
            this.btnReplaceChar.Location = new System.Drawing.Point(192, 64);
            this.btnReplaceChar.Name = "btnReplaceChar";
            this.btnReplaceChar.Size = new System.Drawing.Size(67, 30);
            this.btnReplaceChar.TabIndex = 71;
            this.btnReplaceChar.Text = "Replace";
            this.toolTip1.SetToolTip(this.btnReplaceChar, "Replace current character image");
            this.btnReplaceChar.UseVisualStyleBackColor = false;
            this.btnReplaceChar.EnabledChanged += new System.EventHandler(this.btnReplaceChar_EnabledChanged);
            this.btnReplaceChar.Click += new System.EventHandler(this.btnReplaceChar_Click);
            // 
            // cboCharacter
            // 
            this.cboCharacter.Enabled = false;
            this.cboCharacter.FormattingEnabled = true;
            this.cboCharacter.Location = new System.Drawing.Point(21, 37);
            this.cboCharacter.Name = "cboCharacter";
            this.cboCharacter.Size = new System.Drawing.Size(238, 21);
            this.cboCharacter.TabIndex = 70;
            this.toolTip1.SetToolTip(this.cboCharacter, "Choose character image");
            this.cboCharacter.SelectedIndexChanged += new System.EventHandler(this.cboCharacter_SelectedIndexChanged);
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(515, 6);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 82;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.toolTip1.SetToolTip(this.picPin, "Click to pin on top");
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // picWorking
            // 
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(230, 6);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 66;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(274, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 68;
            this.label1.Text = "Band Name (click to change):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(274, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 74;
            this.label4.Text = "File name:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(274, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 75;
            this.label5.Text = "Console:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(274, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 76;
            this.label6.Text = "Character image count:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(274, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 77;
            this.label7.Text = "Art image count:";
            // 
            // lblFileName
            // 
            this.lblFileName.Location = new System.Drawing.Point(349, 37);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(181, 13);
            this.lblFileName.TabIndex = 78;
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblConsole
            // 
            this.lblConsole.Location = new System.Drawing.Point(349, 54);
            this.lblConsole.Name = "lblConsole";
            this.lblConsole.Size = new System.Drawing.Size(181, 13);
            this.lblConsole.TabIndex = 79;
            this.lblConsole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCharacter
            // 
            this.lblCharacter.Location = new System.Drawing.Point(419, 73);
            this.lblCharacter.Name = "lblCharacter";
            this.lblCharacter.Size = new System.Drawing.Size(111, 13);
            this.lblCharacter.TabIndex = 80;
            this.lblCharacter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblArt
            // 
            this.lblArt.Location = new System.Drawing.Point(419, 88);
            this.lblArt.Name = "lblArt";
            this.lblArt.Size = new System.Drawing.Size(111, 13);
            this.lblArt.TabIndex = 81;
            this.lblArt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SaveFileImageEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(542, 688);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.lblArt);
            this.Controls.Add(this.lblConsole);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnExportChar);
            this.Controls.Add(this.btnReplaceChar);
            this.Controls.Add(this.cboCharacter);
            this.Controls.Add(this.txtBand);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.cboBackgrounds);
            this.Controls.Add(this.btnExportArt);
            this.Controls.Add(this.btnReplaceArt);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.cboArt);
            this.Controls.Add(this.picArt);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lblCharacter);
            this.Controls.Add(this.picCharacter);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "SaveFileImageEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save File Image Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveFileImageEditor_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.picArt)).EndInit();
            this.contextMenuStrip3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picCharacter;
        private System.Windows.Forms.PictureBox picArt;
        private System.Windows.Forms.ComboBox cboArt;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSaveGameFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveChangesToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllImagesToolStripMenuItem;
        private System.Windows.Forms.Button btnExportArt;
        private System.Windows.Forms.Button btnReplaceArt;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem replaceArtImageToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem replaceCharacterImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCompositeImageToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem closeFileToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboBackgrounds;
        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBand;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnExportChar;
        private System.Windows.Forms.Button btnReplaceChar;
        private System.Windows.Forms.ComboBox cboCharacter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblConsole;
        private System.Windows.Forms.Label lblCharacter;
        private System.Windows.Forms.Label lblArt;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offsetFix;
    }
}