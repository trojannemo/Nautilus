namespace Nautilus
{
    partial class RBAEditor
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picPin = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnDTA = new System.Windows.Forms.Button();
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendToVisualizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithNotepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMogg = new System.Windows.Forms.Button();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendToAudioAnalyzerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToSongAnalyzerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnArt = new System.Windows.Forms.Button();
            this.contextMenuStrip5 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendToVisualizerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMilo = new System.Windows.Forms.Button();
            this.btnWeights = new System.Windows.Forms.Button();
            this.btnBackend = new System.Windows.Forms.Button();
            this.contextMenuStrip6 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openInNotepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMIDI = new System.Windows.Forms.Button();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendToSongAnalyzerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.contextMenuStrip4.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip5.SuspendLayout();
            this.contextMenuStrip6.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
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
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(588, 3);
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
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(614, 24);
            this.menuStrip1.TabIndex = 59;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnDTA
            // 
            this.btnDTA.AllowDrop = true;
            this.btnDTA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDTA.ContextMenuStrip = this.contextMenuStrip4;
            this.btnDTA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDTA.Enabled = false;
            this.btnDTA.Location = new System.Drawing.Point(12, 27);
            this.btnDTA.Name = "btnDTA";
            this.btnDTA.Size = new System.Drawing.Size(75, 48);
            this.btnDTA.TabIndex = 65;
            this.btnDTA.Text = "DTA";
            this.btnDTA.UseVisualStyleBackColor = false;
            this.btnDTA.Click += new System.EventHandler(this.btnDTA_Click);
            this.btnDTA.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnDTA_DragDrop);
            this.btnDTA.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToVisualizerToolStripMenuItem,
            this.openWithNotepadToolStripMenuItem});
            this.contextMenuStrip4.Name = "contextMenuStrip4";
            this.contextMenuStrip4.Size = new System.Drawing.Size(179, 48);
            // 
            // sendToVisualizerToolStripMenuItem
            // 
            this.sendToVisualizerToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.sendToVisualizerToolStripMenuItem.Name = "sendToVisualizerToolStripMenuItem";
            this.sendToVisualizerToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.sendToVisualizerToolStripMenuItem.Text = "Send to Visualizer";
            this.sendToVisualizerToolStripMenuItem.Click += new System.EventHandler(this.sendToVisualizerToolStripMenuItem_Click);
            // 
            // openWithNotepadToolStripMenuItem
            // 
            this.openWithNotepadToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.openWithNotepadToolStripMenuItem.Name = "openWithNotepadToolStripMenuItem";
            this.openWithNotepadToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.openWithNotepadToolStripMenuItem.Text = "Open with Notepad";
            this.openWithNotepadToolStripMenuItem.Click += new System.EventHandler(this.openWithNotepadToolStripMenuItem_Click);
            // 
            // btnMogg
            // 
            this.btnMogg.AllowDrop = true;
            this.btnMogg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMogg.ContextMenuStrip = this.contextMenuStrip2;
            this.btnMogg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMogg.Enabled = false;
            this.btnMogg.Location = new System.Drawing.Point(270, 27);
            this.btnMogg.Name = "btnMogg";
            this.btnMogg.Size = new System.Drawing.Size(75, 48);
            this.btnMogg.TabIndex = 66;
            this.btnMogg.Text = "MOGG";
            this.btnMogg.UseVisualStyleBackColor = false;
            this.btnMogg.Click += new System.EventHandler(this.btnMogg_Click);
            this.btnMogg.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnMogg_DragDrop);
            this.btnMogg.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToAudioAnalyzerToolStripMenuItem,
            this.sendToSongAnalyzerToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(198, 48);
            // 
            // sendToAudioAnalyzerToolStripMenuItem
            // 
            this.sendToAudioAnalyzerToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.sendToAudioAnalyzerToolStripMenuItem.Name = "sendToAudioAnalyzerToolStripMenuItem";
            this.sendToAudioAnalyzerToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.sendToAudioAnalyzerToolStripMenuItem.Text = "Send to Audio Analyzer";
            this.sendToAudioAnalyzerToolStripMenuItem.Click += new System.EventHandler(this.sendToAudioAnalyzerToolStripMenuItem_Click);
            // 
            // sendToSongAnalyzerToolStripMenuItem
            // 
            this.sendToSongAnalyzerToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.sendToSongAnalyzerToolStripMenuItem.Name = "sendToSongAnalyzerToolStripMenuItem";
            this.sendToSongAnalyzerToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.sendToSongAnalyzerToolStripMenuItem.Text = "Send to File Analyzer";
            this.sendToSongAnalyzerToolStripMenuItem.Click += new System.EventHandler(this.sendToSongAnalyzerToolStripMenuItem_Click);
            // 
            // btnArt
            // 
            this.btnArt.AllowDrop = true;
            this.btnArt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnArt.ContextMenuStrip = this.contextMenuStrip5;
            this.btnArt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnArt.Enabled = false;
            this.btnArt.Location = new System.Drawing.Point(97, 27);
            this.btnArt.Name = "btnArt";
            this.btnArt.Size = new System.Drawing.Size(75, 48);
            this.btnArt.TabIndex = 67;
            this.btnArt.Text = "Album Art";
            this.btnArt.UseVisualStyleBackColor = false;
            this.btnArt.Click += new System.EventHandler(this.btnArt_Click);
            this.btnArt.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnArt_DragDrop);
            this.btnArt.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // contextMenuStrip5
            // 
            this.contextMenuStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToVisualizerToolStripMenuItem1});
            this.contextMenuStrip5.Name = "contextMenuStrip5";
            this.contextMenuStrip5.Size = new System.Drawing.Size(167, 26);
            // 
            // sendToVisualizerToolStripMenuItem1
            // 
            this.sendToVisualizerToolStripMenuItem1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.sendToVisualizerToolStripMenuItem1.Name = "sendToVisualizerToolStripMenuItem1";
            this.sendToVisualizerToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.sendToVisualizerToolStripMenuItem1.Text = "Send to Visualizer";
            this.sendToVisualizerToolStripMenuItem1.Click += new System.EventHandler(this.sendToVisualizerToolStripMenuItem1_Click);
            // 
            // btnMilo
            // 
            this.btnMilo.AllowDrop = true;
            this.btnMilo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMilo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMilo.Enabled = false;
            this.btnMilo.Location = new System.Drawing.Point(356, 27);
            this.btnMilo.Name = "btnMilo";
            this.btnMilo.Size = new System.Drawing.Size(75, 48);
            this.btnMilo.TabIndex = 68;
            this.btnMilo.Text = "Milo";
            this.btnMilo.UseVisualStyleBackColor = false;
            this.btnMilo.Click += new System.EventHandler(this.btnMilo_Click);
            this.btnMilo.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnMilo_DragDrop);
            this.btnMilo.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnWeights
            // 
            this.btnWeights.AllowDrop = true;
            this.btnWeights.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnWeights.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWeights.Enabled = false;
            this.btnWeights.Location = new System.Drawing.Point(441, 27);
            this.btnWeights.Name = "btnWeights";
            this.btnWeights.Size = new System.Drawing.Size(75, 48);
            this.btnWeights.TabIndex = 69;
            this.btnWeights.Text = "Weights";
            this.btnWeights.UseVisualStyleBackColor = false;
            this.btnWeights.Click += new System.EventHandler(this.btnWeights_Click);
            this.btnWeights.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnWeights_DragDrop);
            this.btnWeights.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // btnBackend
            // 
            this.btnBackend.AllowDrop = true;
            this.btnBackend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBackend.ContextMenuStrip = this.contextMenuStrip6;
            this.btnBackend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackend.Enabled = false;
            this.btnBackend.Location = new System.Drawing.Point(527, 27);
            this.btnBackend.Name = "btnBackend";
            this.btnBackend.Size = new System.Drawing.Size(75, 48);
            this.btnBackend.TabIndex = 70;
            this.btnBackend.Text = "Backend";
            this.btnBackend.UseVisualStyleBackColor = false;
            this.btnBackend.Click += new System.EventHandler(this.btnBackend_Click);
            this.btnBackend.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnBackend_DragDrop);
            this.btnBackend.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // contextMenuStrip6
            // 
            this.contextMenuStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInNotepadToolStripMenuItem});
            this.contextMenuStrip6.Name = "contextMenuStrip6";
            this.contextMenuStrip6.Size = new System.Drawing.Size(166, 26);
            // 
            // openInNotepadToolStripMenuItem
            // 
            this.openInNotepadToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.openInNotepadToolStripMenuItem.Name = "openInNotepadToolStripMenuItem";
            this.openInNotepadToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.openInNotepadToolStripMenuItem.Text = "Open in Notepad";
            this.openInNotepadToolStripMenuItem.Click += new System.EventHandler(this.openInNotepadToolStripMenuItem_Click);
            // 
            // btnMIDI
            // 
            this.btnMIDI.AllowDrop = true;
            this.btnMIDI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMIDI.ContextMenuStrip = this.contextMenuStrip3;
            this.btnMIDI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMIDI.Enabled = false;
            this.btnMIDI.Location = new System.Drawing.Point(183, 27);
            this.btnMIDI.Name = "btnMIDI";
            this.btnMIDI.Size = new System.Drawing.Size(75, 48);
            this.btnMIDI.TabIndex = 71;
            this.btnMIDI.Text = "MIDI";
            this.btnMIDI.UseVisualStyleBackColor = false;
            this.btnMIDI.Click += new System.EventHandler(this.btnMIDI_Click);
            this.btnMIDI.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnMIDI_DragDrop);
            this.btnMIDI.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToSongAnalyzerToolStripMenuItem1});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(184, 26);
            // 
            // sendToSongAnalyzerToolStripMenuItem1
            // 
            this.sendToSongAnalyzerToolStripMenuItem1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.sendToSongAnalyzerToolStripMenuItem1.Name = "sendToSongAnalyzerToolStripMenuItem1";
            this.sendToSongAnalyzerToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
            this.sendToSongAnalyzerToolStripMenuItem1.Text = "Send to File Analyzer";
            this.sendToSongAnalyzerToolStripMenuItem1.Click += new System.EventHandler(this.sendToSongAnalyzerToolStripMenuItem1_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Location = new System.Drawing.Point(504, 271);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 30);
            this.btnSave.TabIndex = 72;
            this.btnSave.Text = "Save Changes";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // RBAEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(614, 315);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnMIDI);
            this.Controls.Add(this.btnBackend);
            this.Controls.Add(this.btnWeights);
            this.Controls.Add(this.btnMilo);
            this.Controls.Add(this.btnArt);
            this.Controls.Add(this.btnMogg);
            this.Controls.Add(this.btnDTA);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lstLog);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "RBAEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RBA Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RBAEditor_FormClosing);
            this.Shown += new System.EventHandler(this.RBAEditor_Shown);
            this.TextChanged += new System.EventHandler(this.RBAEditor_TextChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            this.contextMenuStrip4.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip5.ResumeLayout(false);
            this.contextMenuStrip6.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportLogFileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.Button btnDTA;
        private System.Windows.Forms.Button btnMogg;
        private System.Windows.Forms.Button btnArt;
        private System.Windows.Forms.Button btnMilo;
        private System.Windows.Forms.Button btnWeights;
        private System.Windows.Forms.Button btnBackend;
        private System.Windows.Forms.Button btnMIDI;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem sendToAudioAnalyzerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToSongAnalyzerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem sendToSongAnalyzerToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip4;
        private System.Windows.Forms.ToolStripMenuItem sendToVisualizerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip5;
        private System.Windows.Forms.ToolStripMenuItem sendToVisualizerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openWithNotepadToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip6;
        private System.Windows.Forms.ToolStripMenuItem openInNotepadToolStripMenuItem;
    }
}