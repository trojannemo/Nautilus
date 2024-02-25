namespace Nautilus
{
    partial class AudioAnalyzer
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
            this.uploadToImgur = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.labelAudioChannels = new System.Windows.Forms.ToolStripMenuItem();
            this.outlineAudioTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.highQualityDrawing = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblFileInfo = new System.Windows.Forms.Label();
            this.panelWave = new System.Windows.Forms.Panel();
            this.lblFileName = new System.Windows.Forms.Label();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.picPin = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.panelBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(763, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadToImgur,
            this.saveToFile,
            this.toolStripMenuItem2,
            this.exitToolStrip});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // uploadToImgur
            // 
            this.uploadToImgur.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.uploadToImgur.Name = "uploadToImgur";
            this.uploadToImgur.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.uploadToImgur.Size = new System.Drawing.Size(180, 22);
            this.uploadToImgur.Text = "Upload to Imgur";
            this.uploadToImgur.Click += new System.EventHandler(this.uploadToImgurToolStripMenuItem_Click);
            // 
            // saveToFile
            // 
            this.saveToFile.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.saveToFile.Name = "saveToFile";
            this.saveToFile.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.saveToFile.Size = new System.Drawing.Size(180, 22);
            this.saveToFile.Text = "Save to file";
            this.saveToFile.Click += new System.EventHandler(this.saveToFile_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStrip
            // 
            this.exitToolStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.exitToolStrip.Name = "exitToolStrip";
            this.exitToolStrip.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.exitToolStrip.Size = new System.Drawing.Size(180, 22);
            this.exitToolStrip.Text = "E&xit";
            this.exitToolStrip.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLegend,
            this.labelAudioChannels,
            this.outlineAudioTracks,
            this.highQualityDrawing});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // showLegend
            // 
            this.showLegend.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.showLegend.Checked = true;
            this.showLegend.CheckOnClick = true;
            this.showLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showLegend.Name = "showLegend";
            this.showLegend.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.showLegend.Size = new System.Drawing.Size(230, 22);
            this.showLegend.Text = "Show legend";
            this.showLegend.Click += new System.EventHandler(this.showLegend_Click);
            // 
            // labelAudioChannels
            // 
            this.labelAudioChannels.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelAudioChannels.Checked = true;
            this.labelAudioChannels.CheckOnClick = true;
            this.labelAudioChannels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.labelAudioChannels.Name = "labelAudioChannels";
            this.labelAudioChannels.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.labelAudioChannels.Size = new System.Drawing.Size(230, 22);
            this.labelAudioChannels.Text = "Label audio channels";
            this.labelAudioChannels.Click += new System.EventHandler(this.labelEachAudioChannel_Click);
            // 
            // outlineAudioTracks
            // 
            this.outlineAudioTracks.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.outlineAudioTracks.Checked = true;
            this.outlineAudioTracks.CheckOnClick = true;
            this.outlineAudioTracks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.outlineAudioTracks.Name = "outlineAudioTracks";
            this.outlineAudioTracks.Size = new System.Drawing.Size(230, 22);
            this.outlineAudioTracks.Text = "Outline audio tracks";
            this.outlineAudioTracks.Click += new System.EventHandler(this.showLegend_Click);
            // 
            // highQualityDrawing
            // 
            this.highQualityDrawing.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.highQualityDrawing.CheckOnClick = true;
            this.highQualityDrawing.Name = "highQualityDrawing";
            this.highQualityDrawing.Size = new System.Drawing.Size(230, 22);
            this.highQualityDrawing.Text = "High quality drawing (slower)";
            this.highQualityDrawing.Click += new System.EventHandler(this.showLegend_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // panelBackground
            // 
            this.panelBackground.AllowDrop = true;
            this.panelBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.panelBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBackground.Controls.Add(this.lblStart);
            this.panelBackground.Controls.Add(this.lblLength);
            this.panelBackground.Controls.Add(this.lblFileInfo);
            this.panelBackground.Controls.Add(this.panelWave);
            this.panelBackground.Controls.Add(this.lblFileName);
            this.panelBackground.Location = new System.Drawing.Point(12, 27);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(740, 652);
            this.panelBackground.TabIndex = 1;
            this.panelBackground.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.panelBackground.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // lblStart
            // 
            this.lblStart.AllowDrop = true;
            this.lblStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.lblStart.ForeColor = System.Drawing.Color.White;
            this.lblStart.Location = new System.Drawing.Point(11, 612);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(30, 16);
            this.lblStart.TabIndex = 7;
            // 
            // lblLength
            // 
            this.lblLength.AllowDrop = true;
            this.lblLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.lblLength.ForeColor = System.Drawing.Color.White;
            this.lblLength.Location = new System.Drawing.Point(678, 612);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(50, 16);
            this.lblLength.TabIndex = 8;
            this.lblLength.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFileInfo
            // 
            this.lblFileInfo.AllowDrop = true;
            this.lblFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.lblFileInfo.ForeColor = System.Drawing.Color.White;
            this.lblFileInfo.Location = new System.Drawing.Point(10, 619);
            this.lblFileInfo.Name = "lblFileInfo";
            this.lblFileInfo.Size = new System.Drawing.Size(718, 24);
            this.lblFileInfo.TabIndex = 6;
            this.lblFileInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblFileInfo.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.lblFileInfo.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // panelWave
            // 
            this.panelWave.AllowDrop = true;
            this.panelWave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWave.BackColor = System.Drawing.Color.Silver;
            this.panelWave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelWave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelWave.Location = new System.Drawing.Point(10, 42);
            this.panelWave.Name = "panelWave";
            this.panelWave.Size = new System.Drawing.Size(718, 568);
            this.panelWave.TabIndex = 5;
            this.panelWave.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.panelWave.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // lblFileName
            // 
            this.lblFileName.AllowDrop = true;
            this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.lblFileName.ForeColor = System.Drawing.Color.White;
            this.lblFileName.Location = new System.Drawing.Point(10, 9);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(718, 24);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblFileName.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.lblFileName.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // picWorking
            // 
            this.picWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(320, 6);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 58;
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
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(738, 4);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 64;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // AudioAnalyzer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(763, 691);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.panelBackground);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AudioAnalyzer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Analyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AudioAnalyzer_FormClosing);
            this.Shown += new System.EventHandler(this.AudioAnalyzer_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.Resize += new System.EventHandler(this.AudioAnalyzer_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Panel panelWave;
        private System.Windows.Forms.Label lblFileInfo;
        private System.Windows.Forms.PictureBox picWorking;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem uploadToImgur;
        private System.Windows.Forms.ToolStripMenuItem exitToolStrip;
        private System.Windows.Forms.ToolStripMenuItem saveToFile;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelAudioChannels;
        //private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.ToolStripMenuItem showLegend;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem highQualityDrawing;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.ToolStripMenuItem outlineAudioTracks;
    }
}