namespace Nautilus
{
    partial class ThemeSelector
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
            this.lstThemes = new System.Windows.Forms.ListBox();
            this.btnUse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstThemes
            // 
            this.lstThemes.FormattingEnabled = true;
            this.lstThemes.Location = new System.Drawing.Point(12, 12);
            this.lstThemes.Name = "lstThemes";
            this.lstThemes.Size = new System.Drawing.Size(182, 160);
            this.lstThemes.TabIndex = 0;
            this.lstThemes.SelectedIndexChanged += new System.EventHandler(this.lstThemes_SelectedIndexChanged);
            // 
            // btnUse
            // 
            this.btnUse.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUse.ForeColor = System.Drawing.Color.White;
            this.btnUse.Location = new System.Drawing.Point(12, 178);
            this.btnUse.Name = "btnUse";
            this.btnUse.Size = new System.Drawing.Size(182, 28);
            this.btnUse.TabIndex = 26;
            this.btnUse.Text = "Use selected theme";
            this.btnUse.UseVisualStyleBackColor = false;
            this.btnUse.Click += new System.EventHandler(this.btnUse_Click);
            // 
            // ThemeSelector
            // 
            this.AcceptButton = this.btnUse;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(207, 217);
            this.Controls.Add(this.btnUse);
            this.Controls.Add(this.lstThemes);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThemeSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Choose a Theme:";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Themes_FormClosing);
            this.Shown += new System.EventHandler(this.Themes_Shown);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox lstThemes;
        private System.Windows.Forms.Button btnUse;
    }
}