namespace Nautilus
{
    partial class AudioConverter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioConverter));
            this.label1 = new System.Windows.Forms.Label();
            this.btnOgg = new System.Windows.Forms.Button();
            this.btnOpus = new System.Windows.Forms.Button();
            this.btnMP3 = new System.Windows.Forms.Button();
            this.btnFlac = new System.Windows.Forms.Button();
            this.btnWav = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.qualityOgg = new System.Windows.Forms.NumericUpDown();
            this.qualityFlac = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.qualityOpus = new System.Windows.Forms.NumericUpDown();
            this.qualityMp3 = new System.Windows.Forms.NumericUpDown();
            this.qualityBink = new System.Windows.Forms.NumericUpDown();
            this.picPin = new System.Windows.Forms.PictureBox();
            this.qualityWav = new System.Windows.Forms.DomainUpDown();
            this.btnAbout = new System.Windows.Forms.Button();
            this.lblSuccess = new System.Windows.Forms.Label();
            this.lblFailed = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblWorking = new System.Windows.Forms.Label();
            this.btnBink = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.qualityOgg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityFlac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityOpus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityMp3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityBink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(420, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Drag && drop the file(s) to be converted onto the correct button";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOgg
            // 
            this.btnOgg.AllowDrop = true;
            this.btnOgg.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOgg.Location = new System.Drawing.Point(15, 72);
            this.btnOgg.Name = "btnOgg";
            this.btnOgg.Size = new System.Drawing.Size(50, 50);
            this.btnOgg.TabIndex = 1;
            this.btnOgg.TabStop = false;
            this.btnOgg.Text = "toOGG";
            this.btnOgg.UseVisualStyleBackColor = false;
            this.btnOgg.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnOgg_DragDrop);
            this.btnOgg.DragEnter += new System.Windows.Forms.DragEventHandler(this.btnOgg_DragEnter);
            // 
            // btnOpus
            // 
            this.btnOpus.AllowDrop = true;
            this.btnOpus.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOpus.Location = new System.Drawing.Point(71, 72);
            this.btnOpus.Name = "btnOpus";
            this.btnOpus.Size = new System.Drawing.Size(61, 50);
            this.btnOpus.TabIndex = 2;
            this.btnOpus.TabStop = false;
            this.btnOpus.Text = "toOPUS";
            this.btnOpus.UseVisualStyleBackColor = false;
            this.btnOpus.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnOpus_DragDrop);
            this.btnOpus.DragEnter += new System.Windows.Forms.DragEventHandler(this.btnOgg_DragEnter);
            // 
            // btnMP3
            // 
            this.btnMP3.AllowDrop = true;
            this.btnMP3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnMP3.Location = new System.Drawing.Point(138, 72);
            this.btnMP3.Name = "btnMP3";
            this.btnMP3.Size = new System.Drawing.Size(50, 50);
            this.btnMP3.TabIndex = 3;
            this.btnMP3.TabStop = false;
            this.btnMP3.Text = "toMP3";
            this.btnMP3.UseVisualStyleBackColor = false;
            this.btnMP3.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnMP3_DragDrop);
            this.btnMP3.DragEnter += new System.Windows.Forms.DragEventHandler(this.btnOgg_DragEnter);
            // 
            // btnFlac
            // 
            this.btnFlac.AllowDrop = true;
            this.btnFlac.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnFlac.Location = new System.Drawing.Point(194, 72);
            this.btnFlac.Name = "btnFlac";
            this.btnFlac.Size = new System.Drawing.Size(50, 50);
            this.btnFlac.TabIndex = 4;
            this.btnFlac.TabStop = false;
            this.btnFlac.Text = "toFLAC";
            this.btnFlac.UseVisualStyleBackColor = false;
            this.btnFlac.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnFlac_DragDrop);
            this.btnFlac.DragEnter += new System.Windows.Forms.DragEventHandler(this.btnOgg_DragEnter);
            // 
            // btnWav
            // 
            this.btnWav.AllowDrop = true;
            this.btnWav.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnWav.Location = new System.Drawing.Point(250, 72);
            this.btnWav.Name = "btnWav";
            this.btnWav.Size = new System.Drawing.Size(50, 50);
            this.btnWav.TabIndex = 5;
            this.btnWav.TabStop = false;
            this.btnWav.Text = "toWAV";
            this.btnWav.UseVisualStyleBackColor = false;
            this.btnWav.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnWav_DragDrop);
            this.btnWav.DragEnter += new System.Windows.Forms.DragEventHandler(this.btnOgg_DragEnter);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(420, 37);
            this.label2.TabIndex = 6;
            this.label2.Text = "The values below each button correspond to that audio format\'s quality or bitrate" +
    " setting\r\nIf you don\'t know what you\'re doing, leave the values alone for defaul" +
    "t settings\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // qualityOgg
            // 
            this.qualityOgg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.qualityOgg.Location = new System.Drawing.Point(15, 128);
            this.qualityOgg.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.qualityOgg.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.qualityOgg.Name = "qualityOgg";
            this.qualityOgg.ReadOnly = true;
            this.qualityOgg.Size = new System.Drawing.Size(50, 20);
            this.qualityOgg.TabIndex = 0;
            this.qualityOgg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.qualityOgg, "Higher quality value = better audio quality");
            this.qualityOgg.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // qualityFlac
            // 
            this.qualityFlac.Cursor = System.Windows.Forms.Cursors.Hand;
            this.qualityFlac.Location = new System.Drawing.Point(194, 128);
            this.qualityFlac.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.qualityFlac.Name = "qualityFlac";
            this.qualityFlac.ReadOnly = true;
            this.qualityFlac.Size = new System.Drawing.Size(50, 20);
            this.qualityFlac.TabIndex = 3;
            this.qualityFlac.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.qualityFlac, "Lower compression value = better audio quality");
            this.qualityFlac.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // qualityOpus
            // 
            this.qualityOpus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.qualityOpus.Increment = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.qualityOpus.Location = new System.Drawing.Point(71, 128);
            this.qualityOpus.Maximum = new decimal(new int[] {
            448,
            0,
            0,
            0});
            this.qualityOpus.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.qualityOpus.Name = "qualityOpus";
            this.qualityOpus.ReadOnly = true;
            this.qualityOpus.Size = new System.Drawing.Size(61, 20);
            this.qualityOpus.TabIndex = 1;
            this.qualityOpus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.qualityOpus, "Higher bitrate value = better audio quality");
            this.qualityOpus.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // qualityMp3
            // 
            this.qualityMp3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.qualityMp3.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.qualityMp3.Location = new System.Drawing.Point(138, 128);
            this.qualityMp3.Maximum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.qualityMp3.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.qualityMp3.Name = "qualityMp3";
            this.qualityMp3.ReadOnly = true;
            this.qualityMp3.Size = new System.Drawing.Size(50, 20);
            this.qualityMp3.TabIndex = 2;
            this.qualityMp3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.qualityMp3, "Higher bitrate value = better audio quality");
            this.qualityMp3.Value = new decimal(new int[] {
            320,
            0,
            0,
            0});
            // 
            // qualityBink
            // 
            this.qualityBink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.qualityBink.Location = new System.Drawing.Point(306, 129);
            this.qualityBink.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.qualityBink.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.qualityBink.Name = "qualityBink";
            this.qualityBink.ReadOnly = true;
            this.qualityBink.Size = new System.Drawing.Size(50, 20);
            this.qualityBink.TabIndex = 67;
            this.qualityBink.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.qualityBink, "Higher quality value = better audio quality");
            this.qualityBink.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::Nautilus.Properties.Resources.pinned;
            this.picPin.Location = new System.Drawing.Point(394, 5);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(18, 18);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 65;
            this.picPin.TabStop = false;
            this.picPin.Tag = "pinned";
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // qualityWav
            // 
            this.qualityWav.Enabled = false;
            this.qualityWav.Location = new System.Drawing.Point(251, 128);
            this.qualityWav.Name = "qualityWav";
            this.qualityWav.Size = new System.Drawing.Size(49, 20);
            this.qualityWav.TabIndex = 7;
            this.qualityWav.TabStop = false;
            this.qualityWav.Text = "N/A";
            this.qualityWav.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbout.Location = new System.Drawing.Point(418, 1);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(24, 24);
            this.btnAbout.TabIndex = 8;
            this.btnAbout.Text = "?";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // lblSuccess
            // 
            this.lblSuccess.AutoSize = true;
            this.lblSuccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSuccess.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblSuccess.Location = new System.Drawing.Point(372, 132);
            this.lblSuccess.Name = "lblSuccess";
            this.lblSuccess.Size = new System.Drawing.Size(66, 16);
            this.lblSuccess.TabIndex = 9;
            this.lblSuccess.Text = "Success";
            this.lblSuccess.Visible = false;
            // 
            // lblFailed
            // 
            this.lblFailed.AutoSize = true;
            this.lblFailed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFailed.ForeColor = System.Drawing.Color.Red;
            this.lblFailed.Location = new System.Drawing.Point(387, 132);
            this.lblFailed.Name = "lblFailed";
            this.lblFailed.Size = new System.Drawing.Size(51, 16);
            this.lblFailed.TabIndex = 10;
            this.lblFailed.Text = "Failed";
            this.lblFailed.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblWorking
            // 
            this.lblWorking.AutoSize = true;
            this.lblWorking.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorking.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblWorking.Location = new System.Drawing.Point(362, 132);
            this.lblWorking.Name = "lblWorking";
            this.lblWorking.Size = new System.Drawing.Size(76, 16);
            this.lblWorking.TabIndex = 11;
            this.lblWorking.Text = "Working...";
            this.lblWorking.Visible = false;
            // 
            // btnBink
            // 
            this.btnBink.AllowDrop = true;
            this.btnBink.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnBink.Location = new System.Drawing.Point(306, 72);
            this.btnBink.Name = "btnBink";
            this.btnBink.Size = new System.Drawing.Size(50, 50);
            this.btnBink.TabIndex = 66;
            this.btnBink.TabStop = false;
            this.btnBink.Text = "(Bink)\r\nto\r\nMOGG";
            this.btnBink.UseVisualStyleBackColor = false;
            this.btnBink.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnBink_DragDrop);
            this.btnBink.DragEnter += new System.Windows.Forms.DragEventHandler(this.btnOgg_DragEnter);
            // 
            // AudioConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(444, 159);
            this.Controls.Add(this.qualityBink);
            this.Controls.Add(this.btnBink);
            this.Controls.Add(this.picPin);
            this.Controls.Add(this.lblWorking);
            this.Controls.Add(this.lblFailed);
            this.Controls.Add(this.lblSuccess);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.qualityWav);
            this.Controls.Add(this.qualityMp3);
            this.Controls.Add(this.qualityOpus);
            this.Controls.Add(this.qualityFlac);
            this.Controls.Add(this.qualityOgg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnWav);
            this.Controls.Add(this.btnFlac);
            this.Controls.Add(this.btnMP3);
            this.Controls.Add(this.btnOpus);
            this.Controls.Add(this.btnOgg);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AudioConverter";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Converter";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.qualityOgg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityFlac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityOpus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityMp3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityBink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOgg;
        private System.Windows.Forms.Button btnOpus;
        private System.Windows.Forms.Button btnMP3;
        private System.Windows.Forms.Button btnFlac;
        private System.Windows.Forms.Button btnWav;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown qualityOgg;
        private System.Windows.Forms.NumericUpDown qualityFlac;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown qualityOpus;
        private System.Windows.Forms.NumericUpDown qualityMp3;
        private System.Windows.Forms.DomainUpDown qualityWav;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Label lblSuccess;
        private System.Windows.Forms.Label lblFailed;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblWorking;
        private System.Windows.Forms.PictureBox picPin;
        private System.Windows.Forms.Button btnBink;
        private System.Windows.Forms.NumericUpDown qualityBink;
    }
}

