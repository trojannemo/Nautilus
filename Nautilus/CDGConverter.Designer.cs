namespace Nautilus
{
    partial class CDGConverter
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboBackground = new System.Windows.Forms.ComboBox();
            this.cboText = new System.Windows.Forms.ComboBox();
            this.cboSung = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboFont = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.picSample = new System.Windows.Forms.PictureBox();
            this.lblText = new System.Windows.Forms.Label();
            this.lblHighlight = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioKeep = new System.Windows.Forms.RadioButton();
            this.radioRemove = new System.Windows.Forms.RadioButton();
            this.chkKeepLRC = new System.Windows.Forms.CheckBox();
            this.chkSilent = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Drop Rock Band CON file(s) here...";
            // 
            // cboBackground
            // 
            this.cboBackground.FormattingEnabled = true;
            this.cboBackground.Items.AddRange(new object[] {
            "Black",
            "White",
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "Cyan",
            "Magenta",
            "Gray",
            "Orange",
            "Pink",
            "Purple",
            "Teal",
            "Lime",
            "Navy",
            "Brown"});
            this.cboBackground.Location = new System.Drawing.Point(15, 53);
            this.cboBackground.Name = "cboBackground";
            this.cboBackground.Size = new System.Drawing.Size(60, 21);
            this.cboBackground.TabIndex = 0;
            this.cboBackground.Text = "Black";
            this.cboBackground.SelectedIndexChanged += new System.EventHandler(this.cboBackground_SelectedIndexChanged);
            // 
            // cboText
            // 
            this.cboText.FormattingEnabled = true;
            this.cboText.Items.AddRange(new object[] {
            "Black",
            "White",
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "Cyan",
            "Magenta",
            "Gray",
            "Orange",
            "Pink",
            "Purple",
            "Teal",
            "Lime",
            "Navy",
            "Brown"});
            this.cboText.Location = new System.Drawing.Point(81, 53);
            this.cboText.Name = "cboText";
            this.cboText.Size = new System.Drawing.Size(60, 21);
            this.cboText.TabIndex = 1;
            this.cboText.Text = "White";
            this.cboText.SelectedIndexChanged += new System.EventHandler(this.cboText_SelectedIndexChanged);
            // 
            // cboSung
            // 
            this.cboSung.FormattingEnabled = true;
            this.cboSung.Items.AddRange(new object[] {
            "Black",
            "White",
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "Cyan",
            "Magenta",
            "Gray",
            "Orange",
            "Pink",
            "Purple",
            "Teal",
            "Lime",
            "Navy",
            "Brown"});
            this.cboSung.Location = new System.Drawing.Point(147, 53);
            this.cboSung.Name = "cboSung";
            this.cboSung.Size = new System.Drawing.Size(60, 21);
            this.cboSung.TabIndex = 2;
            this.cboSung.Text = "Blue";
            this.cboSung.SelectedIndexChanged += new System.EventHandler(this.cboSung_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Background";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Text";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Highlight";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Font";
            // 
            // cboFont
            // 
            this.cboFont.FormattingEnabled = true;
            this.cboFont.Items.AddRange(new object[] {
            "Arial",
            "Consolas",
            "Courier New",
            "Lucida Console"});
            this.cboFont.Location = new System.Drawing.Point(15, 105);
            this.cboFont.Name = "cboFont";
            this.cboFont.Size = new System.Drawing.Size(126, 21);
            this.cboFont.TabIndex = 3;
            this.cboFont.Text = "Arial";
            this.cboFont.SelectedIndexChanged += new System.EventHandler(this.cboFont_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(144, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Style";
            // 
            // cboType
            // 
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "Bold",
            "Regular"});
            this.cboType.Location = new System.Drawing.Point(147, 105);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(60, 21);
            this.cboType.TabIndex = 4;
            this.cboType.Text = "Bold";
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboFont_SelectedIndexChanged);
            // 
            // picSample
            // 
            this.picSample.BackColor = System.Drawing.Color.Black;
            this.picSample.Location = new System.Drawing.Point(228, 35);
            this.picSample.Name = "picSample";
            this.picSample.Size = new System.Drawing.Size(155, 90);
            this.picSample.TabIndex = 11;
            this.picSample.TabStop = false;
            // 
            // lblText
            // 
            this.lblText.BackColor = System.Drawing.Color.Black;
            this.lblText.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.ForeColor = System.Drawing.Color.White;
            this.lblText.Location = new System.Drawing.Point(228, 52);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(155, 23);
            this.lblText.TabIndex = 12;
            this.lblText.Text = "TEXT";
            this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHighlight
            // 
            this.lblHighlight.BackColor = System.Drawing.Color.Black;
            this.lblHighlight.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighlight.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblHighlight.Location = new System.Drawing.Point(228, 87);
            this.lblHighlight.Name = "lblHighlight";
            this.lblHighlight.Size = new System.Drawing.Size(155, 23);
            this.lblHighlight.TabIndex = 13;
            this.lblHighlight.Text = "HIGHLIGHT";
            this.lblHighlight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.White;
            this.btnHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Location = new System.Drawing.Point(308, 4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 14;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
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
            this.picWorking.Location = new System.Drawing.Point(259, 146);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(110, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picWorking.TabIndex = 68;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioKeep);
            this.groupBox1.Controls.Add(this.radioRemove);
            this.groupBox1.Location = new System.Drawing.Point(15, 141);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 52);
            this.groupBox1.TabIndex = 69;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vocal options (multitrack only)";
            // 
            // radioKeep
            // 
            this.radioKeep.AutoSize = true;
            this.radioKeep.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioKeep.Location = new System.Drawing.Point(121, 19);
            this.radioKeep.Name = "radioKeep";
            this.radioKeep.Size = new System.Drawing.Size(85, 17);
            this.radioKeep.TabIndex = 1;
            this.radioKeep.TabStop = true;
            this.radioKeep.Text = "Keep Vocals";
            this.radioKeep.UseVisualStyleBackColor = true;
            // 
            // radioRemove
            // 
            this.radioRemove.AutoSize = true;
            this.radioRemove.Checked = true;
            this.radioRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioRemove.Location = new System.Drawing.Point(15, 19);
            this.radioRemove.Name = "radioRemove";
            this.radioRemove.Size = new System.Drawing.Size(100, 17);
            this.radioRemove.TabIndex = 0;
            this.radioRemove.TabStop = true;
            this.radioRemove.Text = "Remove Vocals";
            this.radioRemove.UseVisualStyleBackColor = true;
            // 
            // chkKeepLRC
            // 
            this.chkKeepLRC.AutoSize = true;
            this.chkKeepLRC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkKeepLRC.Location = new System.Drawing.Point(299, 176);
            this.chkKeepLRC.Name = "chkKeepLRC";
            this.chkKeepLRC.Size = new System.Drawing.Size(84, 17);
            this.chkKeepLRC.TabIndex = 70;
            this.chkKeepLRC.Text = "Keep .lrc file";
            this.chkKeepLRC.UseVisualStyleBackColor = true;
            // 
            // chkSilent
            // 
            this.chkSilent.AutoSize = true;
            this.chkSilent.Checked = true;
            this.chkSilent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSilent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSilent.Location = new System.Drawing.Point(241, 176);
            this.chkSilent.Name = "chkSilent";
            this.chkSilent.Size = new System.Drawing.Size(52, 17);
            this.chkSilent.TabIndex = 71;
            this.chkSilent.Text = "Silent";
            this.chkSilent.UseVisualStyleBackColor = true;
            // 
            // CDGConverter
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(395, 204);
            this.Controls.Add(this.chkSilent);
            this.Controls.Add(this.chkKeepLRC);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.lblHighlight);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.picSample);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboFont);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSung);
            this.Controls.Add(this.cboText);
            this.Controls.Add(this.cboBackground);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDGConverter";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rock Band to Karaoke Converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CDGConverter_FormClosing);
            this.Shown += new System.EventHandler(this.CDGConverter_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.CDGConverter_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.CDGConverter_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.picSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBackground;
        private System.Windows.Forms.ComboBox cboText;
        private System.Windows.Forms.ComboBox cboSung;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboFont;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.PictureBox picSample;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label lblHighlight;
        private System.Windows.Forms.Button btnHelp;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioKeep;
        private System.Windows.Forms.RadioButton radioRemove;
        private System.Windows.Forms.CheckBox keepLRC;
        private System.Windows.Forms.CheckBox chkKeepLRC;
        private System.Windows.Forms.CheckBox chkSilent;
    }
}