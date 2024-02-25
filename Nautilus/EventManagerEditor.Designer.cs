namespace Nautilus
{
    partial class EventManagerEditor
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
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.lblArtists = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lstEdit = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblEdit = new System.Windows.Forms.Label();
            this.txtEdit = new System.Windows.Forms.TextBox();
            this.grpEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpEdit
            // 
            this.grpEdit.BackColor = System.Drawing.Color.Transparent;
            this.grpEdit.Controls.Add(this.lblArtists);
            this.grpEdit.Controls.Add(this.btnDelete);
            this.grpEdit.Controls.Add(this.btnNew);
            this.grpEdit.Controls.Add(this.btnEdit);
            this.grpEdit.Controls.Add(this.lstEdit);
            this.grpEdit.Controls.Add(this.btnClose);
            this.grpEdit.Controls.Add(this.lblEdit);
            this.grpEdit.Controls.Add(this.txtEdit);
            this.grpEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpEdit.ForeColor = System.Drawing.Color.Black;
            this.grpEdit.Location = new System.Drawing.Point(12, 13);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.Size = new System.Drawing.Size(442, 376);
            this.grpEdit.TabIndex = 3;
            this.grpEdit.TabStop = false;
            this.grpEdit.Text = "Edit";
            // 
            // lblArtists
            // 
            this.lblArtists.BackColor = System.Drawing.Color.Transparent;
            this.lblArtists.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArtists.ForeColor = System.Drawing.Color.Black;
            this.lblArtists.Location = new System.Drawing.Point(15, 334);
            this.lblArtists.Name = "lblArtists";
            this.lblArtists.Size = new System.Drawing.Size(279, 32);
            this.lblArtists.TabIndex = 6;
            this.lblArtists.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(323, 92);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(103, 29);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Gold;
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNew.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(15, 92);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(103, 29);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "&New Artist";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.LightGreen;
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Enabled = false;
            this.btnEdit.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(170, 92);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(103, 29);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Save / &Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // lstEdit
            // 
            this.lstEdit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstEdit.ForeColor = System.Drawing.Color.Black;
            this.lstEdit.FormattingEnabled = true;
            this.lstEdit.HorizontalScrollbar = true;
            this.lstEdit.ItemHeight = 19;
            this.lstEdit.Location = new System.Drawing.Point(15, 130);
            this.lstEdit.Name = "lstEdit";
            this.lstEdit.Size = new System.Drawing.Size(411, 194);
            this.lstEdit.Sorted = true;
            this.lstEdit.TabIndex = 3;
            this.lstEdit.SelectedIndexChanged += new System.EventHandler(this.lstEdit_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightBlue;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(323, 337);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 29);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblEdit
            // 
            this.lblEdit.BackColor = System.Drawing.Color.Transparent;
            this.lblEdit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEdit.ForeColor = System.Drawing.Color.Black;
            this.lblEdit.Location = new System.Drawing.Point(15, 22);
            this.lblEdit.Name = "lblEdit";
            this.lblEdit.Size = new System.Drawing.Size(411, 32);
            this.lblEdit.TabIndex = 5;
            this.lblEdit.Text = "Artist:";
            this.lblEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEdit
            // 
            this.txtEdit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtEdit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEdit.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEdit.ForeColor = System.Drawing.Color.Black;
            this.txtEdit.Location = new System.Drawing.Point(15, 57);
            this.txtEdit.Name = "txtEdit";
            this.txtEdit.Size = new System.Drawing.Size(411, 29);
            this.txtEdit.TabIndex = 0;
            this.txtEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEdit.TextChanged += new System.EventHandler(this.txtEdit_TextChanged);
            // 
            // EventManagerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(466, 403);
            this.ControlBox = false;
            this.Controls.Add(this.grpEdit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "EventManagerEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EventManagerEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EventManagerEditor_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EventManagerEditor_KeyUp);
            this.grpEdit.ResumeLayout(false);
            this.grpEdit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox grpEdit;
        internal System.Windows.Forms.Label lblArtists;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Button btnNew;
        internal System.Windows.Forms.Button btnEdit;
        internal System.Windows.Forms.ListBox lstEdit;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Label lblEdit;
        internal System.Windows.Forms.TextBox txtEdit;
    }
}