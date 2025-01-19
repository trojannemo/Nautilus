namespace Nautilus
{
    partial class DJHeroTrackSelector
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
            this.btnTrack1 = new System.Windows.Forms.Button();
            this.btnTrack2 = new System.Windows.Forms.Button();
            this.btnTrack3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTrack1
            // 
            this.btnTrack1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrack1.Location = new System.Drawing.Point(12, 12);
            this.btnTrack1.Name = "btnTrack1";
            this.btnTrack1.Size = new System.Drawing.Size(320, 40);
            this.btnTrack1.TabIndex = 0;
            this.btnTrack1.Text = "No track available";
            this.btnTrack1.UseVisualStyleBackColor = true;
            this.btnTrack1.Click += new System.EventHandler(this.btnTrack1_Click);
            // 
            // btnTrack2
            // 
            this.btnTrack2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrack2.Location = new System.Drawing.Point(12, 58);
            this.btnTrack2.Name = "btnTrack2";
            this.btnTrack2.Size = new System.Drawing.Size(320, 40);
            this.btnTrack2.TabIndex = 1;
            this.btnTrack2.Text = "No track available";
            this.btnTrack2.UseVisualStyleBackColor = true;
            this.btnTrack2.Click += new System.EventHandler(this.btnTrack2_Click);
            // 
            // btnTrack3
            // 
            this.btnTrack3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrack3.Location = new System.Drawing.Point(12, 104);
            this.btnTrack3.Name = "btnTrack3";
            this.btnTrack3.Size = new System.Drawing.Size(320, 40);
            this.btnTrack3.TabIndex = 2;
            this.btnTrack3.Text = "No track available";
            this.btnTrack3.UseVisualStyleBackColor = true;
            this.btnTrack3.Click += new System.EventHandler(this.btnTrack3_Click);
            // 
            // DJHeroTrackSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(344, 155);
            this.Controls.Add(this.btnTrack3);
            this.Controls.Add(this.btnTrack2);
            this.Controls.Add(this.btnTrack1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DJHeroTrackSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select track to play";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTrack1;
        private System.Windows.Forms.Button btnTrack2;
        private System.Windows.Forms.Button btnTrack3;
    }
}