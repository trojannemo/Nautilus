namespace Nautilus
{
    partial class Wine
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
            this.lblSee = new System.Windows.Forms.Label();
            this.lblMatter = new System.Windows.Forms.Label();
            this.lblMe = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblSee
            // 
            this.lblSee.AutoSize = true;
            this.lblSee.BackColor = System.Drawing.Color.Transparent;
            this.lblSee.Font = new System.Drawing.Font("High Tower Text", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSee.Location = new System.Drawing.Point(28, 211);
            this.lblSee.Name = "lblSee";
            this.lblSee.Size = new System.Drawing.Size(187, 225);
            this.lblSee.TabIndex = 0;
            this.lblSee.Text = "I\r\nSEE\r\nYOU";
            this.lblSee.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMatter
            // 
            this.lblMatter.AutoSize = true;
            this.lblMatter.BackColor = System.Drawing.Color.Transparent;
            this.lblMatter.Font = new System.Drawing.Font("High Tower Text", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatter.Location = new System.Drawing.Point(347, 262);
            this.lblMatter.Name = "lblMatter";
            this.lblMatter.Size = new System.Drawing.Size(241, 114);
            this.lblMatter.TabIndex = 1;
            this.lblMatter.Text = "YOU\r\nMATTER";
            this.lblMatter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMe
            // 
            this.lblMe.AutoSize = true;
            this.lblMe.BackColor = System.Drawing.Color.Transparent;
            this.lblMe.Font = new System.Drawing.Font("High Tower Text", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMe.ForeColor = System.Drawing.Color.Gray;
            this.lblMe.Location = new System.Drawing.Point(396, 377);
            this.lblMe.Name = "lblMe";
            this.lblMe.Size = new System.Drawing.Size(136, 34);
            this.lblMe.TabIndex = 2;
            this.lblMe.Text = "(TO ME)";
            this.lblMe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 50;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Wine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImage = global::Nautilus.Properties.Resources.wine;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.lblMe);
            this.Controls.Add(this.lblMatter);
            this.Controls.Add(this.lblSee);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Wine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.Wine_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSee;
        private System.Windows.Forms.Label lblMatter;
        private System.Windows.Forms.Label lblMe;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}