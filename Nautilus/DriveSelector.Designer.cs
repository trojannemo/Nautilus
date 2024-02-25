namespace Nautilus
{
    partial class DriveSelector
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
            this.lstDrives = new System.Windows.Forms.ListView();
            this.colDrive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFree = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstDrives
            // 
            this.lstDrives.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDrive,
            this.colName,
            this.colSize,
            this.colFree});
            this.lstDrives.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lstDrives.FullRowSelect = true;
            this.lstDrives.GridLines = true;
            this.lstDrives.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstDrives.HideSelection = false;
            this.lstDrives.Location = new System.Drawing.Point(12, 28);
            this.lstDrives.MultiSelect = false;
            this.lstDrives.Name = "lstDrives";
            this.lstDrives.Size = new System.Drawing.Size(380, 166);
            this.lstDrives.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstDrives.TabIndex = 0;
            this.lstDrives.UseCompatibleStateImageBehavior = false;
            this.lstDrives.View = System.Windows.Forms.View.Details;
            this.lstDrives.SelectedIndexChanged += new System.EventHandler(this.lstDrives_SelectedIndexChanged);
            // 
            // colDrive
            // 
            this.colDrive.Text = "Drive";
            this.colDrive.Width = 40;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 150;
            // 
            // colSize
            // 
            this.colSize.Text = "Size";
            this.colSize.Width = 80;
            // 
            // colFree
            // 
            this.colFree.Text = "Available";
            this.colFree.Width = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Click on a drive to select it:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Press Esc to cancel";
            // 
            // DriveSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(402, 204);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstDrives);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DriveSelector";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Drive Selector";
            this.Shown += new System.EventHandler(this.DriveSelector_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DriveSelector_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstDrives;
        private System.Windows.Forms.ColumnHeader colDrive;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colFree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}