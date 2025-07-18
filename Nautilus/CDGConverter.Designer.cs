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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboFont = new System.Windows.Forms.ComboBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioKeep = new System.Windows.Forms.RadioButton();
            this.radioRemove = new System.Windows.Forms.RadioButton();
            this.chkKeepTOML = new System.Windows.Forms.CheckBox();
            this.grpHarm1 = new System.Windows.Forms.GroupBox();
            this.lblTextHighlight1 = new System.Windows.Forms.Label();
            this.lblTextColor1 = new System.Windows.Forms.Label();
            this.picBackground1 = new System.Windows.Forms.PictureBox();
            this.cboTextHighlight1 = new System.Windows.Forms.ComboBox();
            this.cboTextColor1 = new System.Windows.Forms.ComboBox();
            this.grpHarm2 = new System.Windows.Forms.GroupBox();
            this.lblTextHighlight2 = new System.Windows.Forms.Label();
            this.lblTextColor2 = new System.Windows.Forms.Label();
            this.picBackground2 = new System.Windows.Forms.PictureBox();
            this.cboTextHighlight2 = new System.Windows.Forms.ComboBox();
            this.cboTextColor2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.grpHarm3 = new System.Windows.Forms.GroupBox();
            this.lblTextHighlight3 = new System.Windows.Forms.Label();
            this.lblTextColor3 = new System.Windows.Forms.Label();
            this.picBackground3 = new System.Windows.Forms.PictureBox();
            this.cboTextHighlight3 = new System.Windows.Forms.ComboBox();
            this.cboTextColor3 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkHarmonies = new System.Windows.Forms.CheckBox();
            this.lblFontQuestion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpHarm1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground1)).BeginInit();
            this.grpHarm2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground2)).BeginInit();
            this.grpHarm3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground3)).BeginInit();
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
            this.cboBackground.Size = new System.Drawing.Size(82, 21);
            this.cboBackground.TabIndex = 0;
            this.cboBackground.Text = "Black";
            this.cboBackground.SelectedIndexChanged += new System.EventHandler(this.cboBackground_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Background Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Text";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Highlight";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(186, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Font";
            // 
            // cboFont
            // 
            this.cboFont.FormattingEnabled = true;
            this.cboFont.Items.AddRange(new object[] {
            "Arial | arialbd.ttf"});
            this.cboFont.Location = new System.Drawing.Point(187, 53);
            this.cboFont.Name = "cboFont";
            this.cboFont.Size = new System.Drawing.Size(165, 21);
            this.cboFont.TabIndex = 3;
            this.cboFont.Text = "Arial | arialbd.ttf";
            this.cboFont.SelectedIndexChanged += new System.EventHandler(this.cboFont_SelectedIndexChanged);
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.White;
            this.btnHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Location = new System.Drawing.Point(277, 4);
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
            this.picWorking.Location = new System.Drawing.Point(242, 457);
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
            this.groupBox1.Location = new System.Drawing.Point(15, 436);
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
            // chkKeepTOML
            // 
            this.chkKeepTOML.AutoSize = true;
            this.chkKeepTOML.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkKeepTOML.Location = new System.Drawing.Point(250, 413);
            this.chkKeepTOML.Name = "chkKeepTOML";
            this.chkKeepTOML.Size = new System.Drawing.Size(92, 17);
            this.chkKeepTOML.TabIndex = 70;
            this.chkKeepTOML.Text = "Keep .toml file";
            this.chkKeepTOML.UseVisualStyleBackColor = true;
            // 
            // grpHarm1
            // 
            this.grpHarm1.Controls.Add(this.lblTextHighlight1);
            this.grpHarm1.Controls.Add(this.lblTextColor1);
            this.grpHarm1.Controls.Add(this.picBackground1);
            this.grpHarm1.Controls.Add(this.cboTextHighlight1);
            this.grpHarm1.Controls.Add(this.cboTextColor1);
            this.grpHarm1.Controls.Add(this.label3);
            this.grpHarm1.Controls.Add(this.label4);
            this.grpHarm1.Location = new System.Drawing.Point(15, 80);
            this.grpHarm1.Name = "grpHarm1";
            this.grpHarm1.Size = new System.Drawing.Size(337, 105);
            this.grpHarm1.TabIndex = 72;
            this.grpHarm1.TabStop = false;
            this.grpHarm1.Text = "Lead Vocals / Harm 1";
            // 
            // lblTextHighlight1
            // 
            this.lblTextHighlight1.BackColor = System.Drawing.Color.Black;
            this.lblTextHighlight1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextHighlight1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblTextHighlight1.Location = new System.Drawing.Point(172, 60);
            this.lblTextHighlight1.Name = "lblTextHighlight1";
            this.lblTextHighlight1.Size = new System.Drawing.Size(155, 23);
            this.lblTextHighlight1.TabIndex = 18;
            this.lblTextHighlight1.Text = "HIGHLIGHT";
            this.lblTextHighlight1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTextColor1
            // 
            this.lblTextColor1.BackColor = System.Drawing.Color.Black;
            this.lblTextColor1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextColor1.ForeColor = System.Drawing.Color.White;
            this.lblTextColor1.Location = new System.Drawing.Point(172, 29);
            this.lblTextColor1.Name = "lblTextColor1";
            this.lblTextColor1.Size = new System.Drawing.Size(155, 23);
            this.lblTextColor1.TabIndex = 17;
            this.lblTextColor1.Text = "TEXT";
            this.lblTextColor1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picBackground1
            // 
            this.picBackground1.BackColor = System.Drawing.Color.Black;
            this.picBackground1.Location = new System.Drawing.Point(172, 17);
            this.picBackground1.Name = "picBackground1";
            this.picBackground1.Size = new System.Drawing.Size(155, 79);
            this.picBackground1.TabIndex = 16;
            this.picBackground1.TabStop = false;
            // 
            // cboTextHighlight1
            // 
            this.cboTextHighlight1.FormattingEnabled = true;
            this.cboTextHighlight1.Items.AddRange(new object[] {
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
            this.cboTextHighlight1.Location = new System.Drawing.Point(88, 38);
            this.cboTextHighlight1.Name = "cboTextHighlight1";
            this.cboTextHighlight1.Size = new System.Drawing.Size(73, 21);
            this.cboTextHighlight1.TabIndex = 15;
            this.cboTextHighlight1.Text = "Blue";
            this.cboTextHighlight1.SelectedIndexChanged += new System.EventHandler(this.cboTextHighlight1_SelectedIndexChanged);
            // 
            // cboTextColor1
            // 
            this.cboTextColor1.FormattingEnabled = true;
            this.cboTextColor1.Items.AddRange(new object[] {
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
            this.cboTextColor1.Location = new System.Drawing.Point(14, 38);
            this.cboTextColor1.Name = "cboTextColor1";
            this.cboTextColor1.Size = new System.Drawing.Size(68, 21);
            this.cboTextColor1.TabIndex = 14;
            this.cboTextColor1.Text = "White";
            this.cboTextColor1.SelectedIndexChanged += new System.EventHandler(this.cboTextColor1_SelectedIndexChanged);
            // 
            // grpHarm2
            // 
            this.grpHarm2.Controls.Add(this.lblTextHighlight2);
            this.grpHarm2.Controls.Add(this.lblTextColor2);
            this.grpHarm2.Controls.Add(this.picBackground2);
            this.grpHarm2.Controls.Add(this.cboTextHighlight2);
            this.grpHarm2.Controls.Add(this.cboTextColor2);
            this.grpHarm2.Controls.Add(this.label9);
            this.grpHarm2.Controls.Add(this.label10);
            this.grpHarm2.Location = new System.Drawing.Point(15, 191);
            this.grpHarm2.Name = "grpHarm2";
            this.grpHarm2.Size = new System.Drawing.Size(337, 105);
            this.grpHarm2.TabIndex = 73;
            this.grpHarm2.TabStop = false;
            this.grpHarm2.Text = "Harm 2";
            // 
            // lblTextHighlight2
            // 
            this.lblTextHighlight2.BackColor = System.Drawing.Color.Black;
            this.lblTextHighlight2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextHighlight2.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblTextHighlight2.Location = new System.Drawing.Point(172, 60);
            this.lblTextHighlight2.Name = "lblTextHighlight2";
            this.lblTextHighlight2.Size = new System.Drawing.Size(155, 23);
            this.lblTextHighlight2.TabIndex = 18;
            this.lblTextHighlight2.Text = "HIGHLIGHT";
            this.lblTextHighlight2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTextColor2
            // 
            this.lblTextColor2.BackColor = System.Drawing.Color.Black;
            this.lblTextColor2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextColor2.ForeColor = System.Drawing.Color.White;
            this.lblTextColor2.Location = new System.Drawing.Point(172, 29);
            this.lblTextColor2.Name = "lblTextColor2";
            this.lblTextColor2.Size = new System.Drawing.Size(155, 23);
            this.lblTextColor2.TabIndex = 17;
            this.lblTextColor2.Text = "TEXT";
            this.lblTextColor2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picBackground2
            // 
            this.picBackground2.BackColor = System.Drawing.Color.Black;
            this.picBackground2.Location = new System.Drawing.Point(172, 17);
            this.picBackground2.Name = "picBackground2";
            this.picBackground2.Size = new System.Drawing.Size(155, 79);
            this.picBackground2.TabIndex = 16;
            this.picBackground2.TabStop = false;
            // 
            // cboTextHighlight2
            // 
            this.cboTextHighlight2.FormattingEnabled = true;
            this.cboTextHighlight2.Items.AddRange(new object[] {
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
            this.cboTextHighlight2.Location = new System.Drawing.Point(88, 38);
            this.cboTextHighlight2.Name = "cboTextHighlight2";
            this.cboTextHighlight2.Size = new System.Drawing.Size(73, 21);
            this.cboTextHighlight2.TabIndex = 15;
            this.cboTextHighlight2.Text = "Blue";
            this.cboTextHighlight2.SelectedIndexChanged += new System.EventHandler(this.cboTextHighlight2_SelectedIndexChanged);
            // 
            // cboTextColor2
            // 
            this.cboTextColor2.FormattingEnabled = true;
            this.cboTextColor2.Items.AddRange(new object[] {
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
            this.cboTextColor2.Location = new System.Drawing.Point(14, 38);
            this.cboTextColor2.Name = "cboTextColor2";
            this.cboTextColor2.Size = new System.Drawing.Size(68, 21);
            this.cboTextColor2.TabIndex = 14;
            this.cboTextColor2.Text = "White";
            this.cboTextColor2.SelectedIndexChanged += new System.EventHandler(this.cboTextColor2_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Text";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(84, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Highlight";
            // 
            // grpHarm3
            // 
            this.grpHarm3.Controls.Add(this.lblTextHighlight3);
            this.grpHarm3.Controls.Add(this.lblTextColor3);
            this.grpHarm3.Controls.Add(this.picBackground3);
            this.grpHarm3.Controls.Add(this.cboTextHighlight3);
            this.grpHarm3.Controls.Add(this.cboTextColor3);
            this.grpHarm3.Controls.Add(this.label11);
            this.grpHarm3.Controls.Add(this.label12);
            this.grpHarm3.Location = new System.Drawing.Point(15, 302);
            this.grpHarm3.Name = "grpHarm3";
            this.grpHarm3.Size = new System.Drawing.Size(337, 105);
            this.grpHarm3.TabIndex = 74;
            this.grpHarm3.TabStop = false;
            this.grpHarm3.Text = "Harm 3";
            // 
            // lblTextHighlight3
            // 
            this.lblTextHighlight3.BackColor = System.Drawing.Color.Black;
            this.lblTextHighlight3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextHighlight3.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblTextHighlight3.Location = new System.Drawing.Point(172, 60);
            this.lblTextHighlight3.Name = "lblTextHighlight3";
            this.lblTextHighlight3.Size = new System.Drawing.Size(155, 23);
            this.lblTextHighlight3.TabIndex = 18;
            this.lblTextHighlight3.Text = "HIGHLIGHT";
            this.lblTextHighlight3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTextColor3
            // 
            this.lblTextColor3.BackColor = System.Drawing.Color.Black;
            this.lblTextColor3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextColor3.ForeColor = System.Drawing.Color.White;
            this.lblTextColor3.Location = new System.Drawing.Point(172, 29);
            this.lblTextColor3.Name = "lblTextColor3";
            this.lblTextColor3.Size = new System.Drawing.Size(155, 23);
            this.lblTextColor3.TabIndex = 17;
            this.lblTextColor3.Text = "TEXT";
            this.lblTextColor3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picBackground3
            // 
            this.picBackground3.BackColor = System.Drawing.Color.Black;
            this.picBackground3.Location = new System.Drawing.Point(172, 17);
            this.picBackground3.Name = "picBackground3";
            this.picBackground3.Size = new System.Drawing.Size(155, 79);
            this.picBackground3.TabIndex = 16;
            this.picBackground3.TabStop = false;
            // 
            // cboTextHighlight3
            // 
            this.cboTextHighlight3.FormattingEnabled = true;
            this.cboTextHighlight3.Items.AddRange(new object[] {
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
            this.cboTextHighlight3.Location = new System.Drawing.Point(88, 38);
            this.cboTextHighlight3.Name = "cboTextHighlight3";
            this.cboTextHighlight3.Size = new System.Drawing.Size(73, 21);
            this.cboTextHighlight3.TabIndex = 15;
            this.cboTextHighlight3.Text = "Blue";
            this.cboTextHighlight3.SelectedIndexChanged += new System.EventHandler(this.cboTextHighlight3_SelectedIndexChanged);
            // 
            // cboTextColor3
            // 
            this.cboTextColor3.FormattingEnabled = true;
            this.cboTextColor3.Items.AddRange(new object[] {
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
            this.cboTextColor3.Location = new System.Drawing.Point(14, 38);
            this.cboTextColor3.Name = "cboTextColor3";
            this.cboTextColor3.Size = new System.Drawing.Size(68, 21);
            this.cboTextColor3.TabIndex = 14;
            this.cboTextColor3.Text = "White";
            this.cboTextColor3.SelectedIndexChanged += new System.EventHandler(this.cboTextColor3_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Text";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(84, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Highlight";
            // 
            // chkHarmonies
            // 
            this.chkHarmonies.AutoSize = true;
            this.chkHarmonies.Checked = true;
            this.chkHarmonies.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHarmonies.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkHarmonies.Location = new System.Drawing.Point(15, 413);
            this.chkHarmonies.Name = "chkHarmonies";
            this.chkHarmonies.Size = new System.Drawing.Size(158, 17);
            this.chkHarmonies.TabIndex = 75;
            this.chkHarmonies.Text = "Do harmonies when present";
            this.chkHarmonies.UseVisualStyleBackColor = true;
            // 
            // lblFontQuestion
            // 
            this.lblFontQuestion.AutoSize = true;
            this.lblFontQuestion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFontQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFontQuestion.Location = new System.Drawing.Point(337, 36);
            this.lblFontQuestion.Name = "lblFontQuestion";
            this.lblFontQuestion.Size = new System.Drawing.Size(15, 16);
            this.lblFontQuestion.TabIndex = 76;
            this.lblFontQuestion.Text = "?";
            this.lblFontQuestion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblFontQuestion_MouseClick);
            // 
            // CDGConverter
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(364, 498);
            this.Controls.Add(this.lblFontQuestion);
            this.Controls.Add(this.chkHarmonies);
            this.Controls.Add(this.grpHarm3);
            this.Controls.Add(this.grpHarm2);
            this.Controls.Add(this.grpHarm1);
            this.Controls.Add(this.chkKeepTOML);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.cboFont);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
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
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpHarm1.ResumeLayout(false);
            this.grpHarm1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground1)).EndInit();
            this.grpHarm2.ResumeLayout(false);
            this.grpHarm2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground2)).EndInit();
            this.grpHarm3.ResumeLayout(false);
            this.grpHarm3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBackground;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboFont;
        private System.Windows.Forms.Button btnHelp;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioKeep;
        private System.Windows.Forms.RadioButton radioRemove;
        private System.Windows.Forms.CheckBox keepTOML;
        private System.Windows.Forms.CheckBox chkKeepTOML;
        private System.Windows.Forms.GroupBox grpHarm1;
        private System.Windows.Forms.Label lblTextHighlight1;
        private System.Windows.Forms.Label lblTextColor1;
        private System.Windows.Forms.PictureBox picBackground1;
        private System.Windows.Forms.ComboBox cboTextHighlight1;
        private System.Windows.Forms.ComboBox cboTextColor1;
        private System.Windows.Forms.GroupBox grpHarm2;
        private System.Windows.Forms.Label lblTextHighlight2;
        private System.Windows.Forms.Label lblTextColor2;
        private System.Windows.Forms.PictureBox picBackground2;
        private System.Windows.Forms.ComboBox cboTextHighlight2;
        private System.Windows.Forms.ComboBox cboTextColor2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox grpHarm3;
        private System.Windows.Forms.Label lblTextHighlight3;
        private System.Windows.Forms.Label lblTextColor3;
        private System.Windows.Forms.PictureBox picBackground3;
        private System.Windows.Forms.ComboBox cboTextHighlight3;
        private System.Windows.Forms.ComboBox cboTextColor3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkHarmonies;
        private System.Windows.Forms.Label lblFontQuestion;
    }
}