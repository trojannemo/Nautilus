namespace Nautilus
{
    partial class MIDISelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MIDISelector));
            this.btnCleaner = new System.Windows.Forms.Button();
            this.btnAnalyzer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnCleaner
            // 
            this.btnCleaner.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnCleaner.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCleaner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCleaner.Location = new System.Drawing.Point(12, 48);
            this.btnCleaner.Name = "btnCleaner";
            this.btnCleaner.Size = new System.Drawing.Size(102, 28);
            this.btnCleaner.TabIndex = 0;
            this.btnCleaner.Text = "MIDI &Cleaner";
            this.toolTip1.SetToolTip(this.btnCleaner, "Open file with MIDI Cleaner");
            this.btnCleaner.UseVisualStyleBackColor = false;
            this.btnCleaner.Click += new System.EventHandler(this.btnCleaner_Click);
            // 
            // btnAnalyzer
            // 
            this.btnAnalyzer.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnAnalyzer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnalyzer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyzer.Location = new System.Drawing.Point(126, 48);
            this.btnAnalyzer.Name = "btnAnalyzer";
            this.btnAnalyzer.Size = new System.Drawing.Size(102, 28);
            this.btnAnalyzer.TabIndex = 1;
            this.btnAnalyzer.Text = "Song &Analyzer";
            this.toolTip1.SetToolTip(this.btnAnalyzer, "Open file with Song Analyzer");
            this.btnAnalyzer.UseVisualStyleBackColor = false;
            this.btnAnalyzer.Click += new System.EventHandler(this.btnAnalyzer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Which tool should I use?";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightPink;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(199, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(29, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&X";
            this.toolTip1.SetToolTip(this.btnClose, "Exit");
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MIDISelector
            // 
            this.AcceptButton = this.btnCleaner;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(240, 90);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAnalyzer);
            this.Controls.Add(this.btnCleaner);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MIDISelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select MIDI Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCleaner;
        private System.Windows.Forms.Button btnAnalyzer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
    }
}