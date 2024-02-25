namespace Nautilus
{
    partial class SetlistDetails
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
            this.lstStats = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.picloading = new System.Windows.Forms.PictureBox();
            this.chkPercent = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picloading)).BeginInit();
            this.SuspendLayout();
            // 
            // lstStats
            // 
            this.lstStats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colValue});
            this.lstStats.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.lstStats.FullRowSelect = true;
            this.lstStats.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstStats.HideSelection = false;
            this.lstStats.Location = new System.Drawing.Point(12, 12);
            this.lstStats.Name = "lstStats";
            this.lstStats.Size = new System.Drawing.Size(368, 312);
            this.lstStats.TabIndex = 0;
            this.lstStats.UseCompatibleStateImageBehavior = false;
            this.lstStats.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 150;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colValue.Width = 190;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(215)))), ((int)(((byte)(0)))));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Enabled = false;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(293, 335);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 28);
            this.btnClose.TabIndex = 27;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(215)))), ((int)(((byte)(0)))));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Enabled = false;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(12, 335);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(87, 28);
            this.btnExport.TabIndex = 28;
            this.btnExport.Text = "E&xport";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // picloading
            // 
            this.picloading.BackColor = System.Drawing.Color.White;
            this.picloading.Image = global::Nautilus.Properties.Resources.loadingcircle;
            this.picloading.Location = new System.Drawing.Point(146, 107);
            this.picloading.Name = "picloading";
            this.picloading.Size = new System.Drawing.Size(100, 100);
            this.picloading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picloading.TabIndex = 29;
            this.picloading.TabStop = false;
            this.picloading.UseWaitCursor = true;
            // 
            // chkPercent
            // 
            this.chkPercent.AutoSize = true;
            this.chkPercent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkPercent.Enabled = false;
            this.chkPercent.Location = new System.Drawing.Point(141, 342);
            this.chkPercent.Name = "chkPercent";
            this.chkPercent.Size = new System.Drawing.Size(115, 17);
            this.chkPercent.TabIndex = 30;
            this.chkPercent.Text = "Show percentages";
            this.chkPercent.UseVisualStyleBackColor = true;
            this.chkPercent.CheckedChanged += new System.EventHandler(this.chkPercent_CheckedChanged);
            // 
            // SetlistDetails
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(392, 371);
            this.Controls.Add(this.chkPercent);
            this.Controls.Add(this.picloading);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lstStats);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetlistDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setlist Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetlistDetails_FormClosing);
            this.Shown += new System.EventHandler(this.SetlistStats_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picloading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstStats;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox picloading;
        private System.Windows.Forms.CheckBox chkPercent;
    }
}