namespace Nautilus
{
    partial class MiniStudio
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.grpChorus = new System.Windows.Forms.GroupBox();
            this.chorusWaveForm = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.chorusPhase = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.chorusWetDry = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.chorusFrequency = new System.Windows.Forms.NumericUpDown();
            this.chorusFeedback = new System.Windows.Forms.NumericUpDown();
            this.chorusDepth = new System.Windows.Forms.NumericUpDown();
            this.chorusDelay = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpEcho = new System.Windows.Forms.GroupBox();
            this.echoPanDelay = new System.Windows.Forms.CheckBox();
            this.EchoWetDry = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.echoRightDelay = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.echoLeftDelay = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.echoFeedback = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.grpReverb = new System.Windows.Forms.GroupBox();
            this.reverbTime = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.reverbMix = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.reverbGain = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.reverbRatio = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.switchChorus = new System.Windows.Forms.PictureBox();
            this.switchEcho = new System.Windows.Forms.PictureBox();
            this.switchReverb = new System.Windows.Forms.PictureBox();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.effectsWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.switchFlanger = new System.Windows.Forms.PictureBox();
            this.switchDistortion = new System.Windows.Forms.PictureBox();
            this.switchCompression = new System.Windows.Forms.PictureBox();
            this.switchGargle = new System.Windows.Forms.PictureBox();
            this.picVolSlider = new System.Windows.Forms.PictureBox();
            this.grpDistortion = new System.Windows.Forms.GroupBox();
            this.distortionCutoff = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.distortionFrequency = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.distortionBandwith = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.distortionGain = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.distortionEdge = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.grpCompression = new System.Windows.Forms.GroupBox();
            this.compressionThreshold = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.compressionRelease = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.compressionRatio = new System.Windows.Forms.NumericUpDown();
            this.compressionPredelay = new System.Windows.Forms.NumericUpDown();
            this.compressionGain = new System.Windows.Forms.NumericUpDown();
            this.compressionAttack = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.grpFlanger = new System.Windows.Forms.GroupBox();
            this.flangerWaveForm = new System.Windows.Forms.NumericUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.flangerPhase = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.flangerWetDry = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.flangerFrequency = new System.Windows.Forms.NumericUpDown();
            this.flangerFeedback = new System.Windows.Forms.NumericUpDown();
            this.flangerDepth = new System.Windows.Forms.NumericUpDown();
            this.flangerDelay = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.grpGargle = new System.Windows.Forms.GroupBox();
            this.gargleWaveShape = new System.Windows.Forms.NumericUpDown();
            this.label36 = new System.Windows.Forms.Label();
            this.gargleRate = new System.Windows.Forms.NumericUpDown();
            this.label38 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.picSpect = new System.Windows.Forms.PictureBox();
            this.playbackTimer = new System.Windows.Forms.Timer(this.components);
            this.picVolBackground = new System.Windows.Forms.PictureBox();
            this.grpChorus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chorusWaveForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusPhase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusWetDry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusFeedback)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusDelay)).BeginInit();
            this.grpEcho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EchoWetDry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.echoRightDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.echoLeftDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.echoFeedback)).BeginInit();
            this.grpReverb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reverbTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reverbMix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reverbGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reverbRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchChorus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchEcho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchReverb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchFlanger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchDistortion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchCompression)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchGargle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolSlider)).BeginInit();
            this.grpDistortion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.distortionCutoff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.distortionFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.distortionBandwith)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.distortionGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.distortionEdge)).BeginInit();
            this.grpCompression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compressionThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionRelease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionPredelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionAttack)).BeginInit();
            this.grpFlanger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flangerWaveForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerPhase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerWetDry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerFeedback)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerDelay)).BeginInit();
            this.grpGargle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gargleWaveShape)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gargleRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(13, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(292, 48);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Drag && drop your track anywhere to get started\r\nSupported input formats are .ogg" +
    ", .mp3 and .wav\r\nOutput file will be in .wav format for best quality\r\n";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpChorus
            // 
            this.grpChorus.BackColor = System.Drawing.Color.Silver;
            this.grpChorus.Controls.Add(this.chorusWaveForm);
            this.grpChorus.Controls.Add(this.label8);
            this.grpChorus.Controls.Add(this.chorusPhase);
            this.grpChorus.Controls.Add(this.label7);
            this.grpChorus.Controls.Add(this.chorusWetDry);
            this.grpChorus.Controls.Add(this.label6);
            this.grpChorus.Controls.Add(this.chorusFrequency);
            this.grpChorus.Controls.Add(this.chorusFeedback);
            this.grpChorus.Controls.Add(this.chorusDepth);
            this.grpChorus.Controls.Add(this.chorusDelay);
            this.grpChorus.Controls.Add(this.label5);
            this.grpChorus.Controls.Add(this.label4);
            this.grpChorus.Controls.Add(this.label3);
            this.grpChorus.Controls.Add(this.label2);
            this.grpChorus.Enabled = false;
            this.grpChorus.Location = new System.Drawing.Point(49, 65);
            this.grpChorus.Name = "grpChorus";
            this.grpChorus.Size = new System.Drawing.Size(432, 92);
            this.grpChorus.TabIndex = 1;
            this.grpChorus.TabStop = false;
            this.grpChorus.Text = "Chorus";
            // 
            // chorusWaveForm
            // 
            this.chorusWaveForm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chorusWaveForm.Location = new System.Drawing.Point(275, 56);
            this.chorusWaveForm.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.chorusWaveForm.Name = "chorusWaveForm";
            this.chorusWaveForm.Size = new System.Drawing.Size(35, 16);
            this.chorusWaveForm.TabIndex = 6;
            this.chorusWaveForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chorusWaveForm.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(214, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Wave Form";
            // 
            // chorusPhase
            // 
            this.chorusPhase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chorusPhase.Enabled = false;
            this.chorusPhase.Location = new System.Drawing.Point(161, 56);
            this.chorusPhase.Name = "chorusPhase";
            this.chorusPhase.Size = new System.Drawing.Size(35, 16);
            this.chorusPhase.TabIndex = 5;
            this.chorusPhase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(124, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Phase";
            // 
            // chorusWetDry
            // 
            this.chorusWetDry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chorusWetDry.Location = new System.Drawing.Point(75, 56);
            this.chorusWetDry.Name = "chorusWetDry";
            this.chorusWetDry.Size = new System.Drawing.Size(35, 16);
            this.chorusWetDry.TabIndex = 4;
            this.chorusWetDry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Wet/Dry Mix";
            // 
            // chorusFrequency
            // 
            this.chorusFrequency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chorusFrequency.Location = new System.Drawing.Point(387, 24);
            this.chorusFrequency.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.chorusFrequency.Name = "chorusFrequency";
            this.chorusFrequency.Size = new System.Drawing.Size(35, 16);
            this.chorusFrequency.TabIndex = 3;
            this.chorusFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chorusFeedback
            // 
            this.chorusFeedback.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chorusFeedback.Location = new System.Drawing.Point(282, 24);
            this.chorusFeedback.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.chorusFeedback.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
            this.chorusFeedback.Name = "chorusFeedback";
            this.chorusFeedback.Size = new System.Drawing.Size(35, 16);
            this.chorusFeedback.TabIndex = 2;
            this.chorusFeedback.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chorusDepth
            // 
            this.chorusDepth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chorusDepth.Location = new System.Drawing.Point(163, 24);
            this.chorusDepth.Name = "chorusDepth";
            this.chorusDepth.Size = new System.Drawing.Size(35, 16);
            this.chorusDepth.TabIndex = 1;
            this.chorusDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chorusDepth.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // chorusDelay
            // 
            this.chorusDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chorusDelay.Location = new System.Drawing.Point(63, 24);
            this.chorusDelay.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.chorusDelay.Name = "chorusDelay";
            this.chorusDelay.Size = new System.Drawing.Size(35, 16);
            this.chorusDelay.TabIndex = 0;
            this.chorusDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(331, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Frequency";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(212, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Feedback (%)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Depth (%)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Delay (ms)";
            // 
            // grpEcho
            // 
            this.grpEcho.BackColor = System.Drawing.Color.Silver;
            this.grpEcho.Controls.Add(this.echoPanDelay);
            this.grpEcho.Controls.Add(this.EchoWetDry);
            this.grpEcho.Controls.Add(this.label12);
            this.grpEcho.Controls.Add(this.echoRightDelay);
            this.grpEcho.Controls.Add(this.label11);
            this.grpEcho.Controls.Add(this.echoLeftDelay);
            this.grpEcho.Controls.Add(this.label10);
            this.grpEcho.Controls.Add(this.echoFeedback);
            this.grpEcho.Controls.Add(this.label9);
            this.grpEcho.Enabled = false;
            this.grpEcho.Location = new System.Drawing.Point(49, 175);
            this.grpEcho.Name = "grpEcho";
            this.grpEcho.Size = new System.Drawing.Size(340, 92);
            this.grpEcho.TabIndex = 2;
            this.grpEcho.TabStop = false;
            this.grpEcho.Text = "Echo";
            // 
            // echoPanDelay
            // 
            this.echoPanDelay.AutoSize = true;
            this.echoPanDelay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.echoPanDelay.Location = new System.Drawing.Point(263, 24);
            this.echoPanDelay.Name = "echoPanDelay";
            this.echoPanDelay.Size = new System.Drawing.Size(75, 17);
            this.echoPanDelay.TabIndex = 11;
            this.echoPanDelay.Text = "Pan Delay";
            this.echoPanDelay.UseVisualStyleBackColor = true;
            // 
            // EchoWetDry
            // 
            this.EchoWetDry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EchoWetDry.Location = new System.Drawing.Point(75, 56);
            this.EchoWetDry.Name = "EchoWetDry";
            this.EchoWetDry.Size = new System.Drawing.Size(35, 16);
            this.EchoWetDry.TabIndex = 10;
            this.EchoWetDry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Wet/Dry Mix";
            // 
            // echoRightDelay
            // 
            this.echoRightDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.echoRightDelay.Location = new System.Drawing.Point(202, 56);
            this.echoRightDelay.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.echoRightDelay.Name = "echoRightDelay";
            this.echoRightDelay.Size = new System.Drawing.Size(51, 16);
            this.echoRightDelay.TabIndex = 9;
            this.echoRightDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.echoRightDelay.Value = new decimal(new int[] {
            333,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(119, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Right Delay (ms)";
            // 
            // echoLeftDelay
            // 
            this.echoLeftDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.echoLeftDelay.Location = new System.Drawing.Point(201, 24);
            this.echoLeftDelay.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.echoLeftDelay.Name = "echoLeftDelay";
            this.echoLeftDelay.Size = new System.Drawing.Size(51, 16);
            this.echoLeftDelay.TabIndex = 8;
            this.echoLeftDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.echoLeftDelay.Value = new decimal(new int[] {
            333,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(126, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Left Delay (ms)";
            // 
            // echoFeedback
            // 
            this.echoFeedback.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.echoFeedback.Location = new System.Drawing.Point(80, 24);
            this.echoFeedback.Name = "echoFeedback";
            this.echoFeedback.Size = new System.Drawing.Size(35, 16);
            this.echoFeedback.TabIndex = 7;
            this.echoFeedback.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Feedback (%)";
            // 
            // grpReverb
            // 
            this.grpReverb.BackColor = System.Drawing.Color.Silver;
            this.grpReverb.Controls.Add(this.reverbTime);
            this.grpReverb.Controls.Add(this.label16);
            this.grpReverb.Controls.Add(this.reverbMix);
            this.grpReverb.Controls.Add(this.label15);
            this.grpReverb.Controls.Add(this.reverbGain);
            this.grpReverb.Controls.Add(this.label14);
            this.grpReverb.Controls.Add(this.reverbRatio);
            this.grpReverb.Controls.Add(this.label13);
            this.grpReverb.Enabled = false;
            this.grpReverb.Location = new System.Drawing.Point(49, 284);
            this.grpReverb.Name = "grpReverb";
            this.grpReverb.Size = new System.Drawing.Size(310, 92);
            this.grpReverb.TabIndex = 3;
            this.grpReverb.TabStop = false;
            this.grpReverb.Text = "Reverb";
            // 
            // reverbTime
            // 
            this.reverbTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reverbTime.Location = new System.Drawing.Point(241, 56);
            this.reverbTime.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.reverbTime.Name = "reverbTime";
            this.reverbTime.Size = new System.Drawing.Size(35, 16);
            this.reverbTime.TabIndex = 15;
            this.reverbTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(152, 56);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(90, 13);
            this.label16.TabIndex = 13;
            this.label16.Text = "Reverb Time (ms)";
            // 
            // reverbMix
            // 
            this.reverbMix.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reverbMix.Location = new System.Drawing.Point(91, 56);
            this.reverbMix.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.reverbMix.Minimum = new decimal(new int[] {
            96,
            0,
            0,
            -2147483648});
            this.reverbMix.Name = "reverbMix";
            this.reverbMix.Size = new System.Drawing.Size(51, 16);
            this.reverbMix.TabIndex = 14;
            this.reverbMix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 56);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Reverb Mix (dB)";
            // 
            // reverbGain
            // 
            this.reverbGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reverbGain.Location = new System.Drawing.Point(245, 24);
            this.reverbGain.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.reverbGain.Minimum = new decimal(new int[] {
            96,
            0,
            0,
            -2147483648});
            this.reverbGain.Name = "reverbGain";
            this.reverbGain.Size = new System.Drawing.Size(51, 16);
            this.reverbGain.TabIndex = 13;
            this.reverbGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(168, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Input Gain (dB)";
            // 
            // reverbRatio
            // 
            this.reverbRatio.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reverbRatio.DecimalPlaces = 3;
            this.reverbRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.reverbRatio.Location = new System.Drawing.Point(107, 24);
            this.reverbRatio.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            196608});
            this.reverbRatio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.reverbRatio.Name = "reverbRatio";
            this.reverbRatio.Size = new System.Drawing.Size(51, 16);
            this.reverbRatio.TabIndex = 12;
            this.reverbRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.reverbRatio.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(99, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "High Freq RT Ratio";
            // 
            // switchChorus
            // 
            this.switchChorus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchChorus.Location = new System.Drawing.Point(13, 84);
            this.switchChorus.Name = "switchChorus";
            this.switchChorus.Size = new System.Drawing.Size(30, 55);
            this.switchChorus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.switchChorus.TabIndex = 4;
            this.switchChorus.TabStop = false;
            this.switchChorus.Tag = "\"0\"";
            this.toolTip1.SetToolTip(this.switchChorus, "Click to enable/disable the Chorus effects");
            this.switchChorus.MouseClick += new System.Windows.Forms.MouseEventHandler(this.switchChorus_MouseClick);
            // 
            // switchEcho
            // 
            this.switchEcho.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchEcho.Location = new System.Drawing.Point(13, 195);
            this.switchEcho.Name = "switchEcho";
            this.switchEcho.Size = new System.Drawing.Size(30, 55);
            this.switchEcho.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.switchEcho.TabIndex = 5;
            this.switchEcho.TabStop = false;
            this.switchEcho.Tag = "\"0\"";
            this.toolTip1.SetToolTip(this.switchEcho, "Click to enable/disable the Echo effects");
            this.switchEcho.MouseClick += new System.Windows.Forms.MouseEventHandler(this.switchEcho_MouseClick);
            // 
            // switchReverb
            // 
            this.switchReverb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchReverb.Location = new System.Drawing.Point(13, 302);
            this.switchReverb.Name = "switchReverb";
            this.switchReverb.Size = new System.Drawing.Size(30, 55);
            this.switchReverb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.switchReverb.TabIndex = 6;
            this.switchReverb.TabStop = false;
            this.switchReverb.Tag = "\"0\"";
            this.toolTip1.SetToolTip(this.switchReverb, "Click to enable/disable the Reverb effects");
            this.switchReverb.MouseClick += new System.Windows.Forms.MouseEventHandler(this.switchReverb_MouseClick);
            // 
            // picWorking
            // 
            this.picWorking.Image = global::Nautilus.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(457, 159);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(128, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWorking.TabIndex = 64;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // effectsWorker
            // 
            this.effectsWorker.WorkerReportsProgress = true;
            this.effectsWorker.WorkerSupportsCancellation = true;
            this.effectsWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.effectsWorker_DoWork);
            this.effectsWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.effectsWorker_RunWorkerCompleted);
            // 
            // switchFlanger
            // 
            this.switchFlanger.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchFlanger.Location = new System.Drawing.Point(512, 83);
            this.switchFlanger.Name = "switchFlanger";
            this.switchFlanger.Size = new System.Drawing.Size(30, 55);
            this.switchFlanger.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.switchFlanger.TabIndex = 73;
            this.switchFlanger.TabStop = false;
            this.switchFlanger.Tag = "\"0\"";
            this.toolTip1.SetToolTip(this.switchFlanger, "Click to enable/disable the Flanger effects");
            this.switchFlanger.MouseClick += new System.Windows.Forms.MouseEventHandler(this.switchFlanger_MouseClick);
            // 
            // switchDistortion
            // 
            this.switchDistortion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchDistortion.Location = new System.Drawing.Point(512, 195);
            this.switchDistortion.Name = "switchDistortion";
            this.switchDistortion.Size = new System.Drawing.Size(30, 55);
            this.switchDistortion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.switchDistortion.TabIndex = 72;
            this.switchDistortion.TabStop = false;
            this.switchDistortion.Tag = "\"0\"";
            this.toolTip1.SetToolTip(this.switchDistortion, "Click to enable/disable the Distortion effects");
            this.switchDistortion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.switchDistortion_MouseClick);
            // 
            // switchCompression
            // 
            this.switchCompression.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchCompression.Location = new System.Drawing.Point(385, 303);
            this.switchCompression.Name = "switchCompression";
            this.switchCompression.Size = new System.Drawing.Size(30, 55);
            this.switchCompression.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.switchCompression.TabIndex = 71;
            this.switchCompression.TabStop = false;
            this.switchCompression.Tag = "\"0\"";
            this.toolTip1.SetToolTip(this.switchCompression, "Click to enable/disable the Compression effects");
            this.switchCompression.MouseClick += new System.Windows.Forms.MouseEventHandler(this.switchCompression_MouseClick);
            // 
            // switchGargle
            // 
            this.switchGargle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchGargle.Location = new System.Drawing.Point(803, 302);
            this.switchGargle.Name = "switchGargle";
            this.switchGargle.Size = new System.Drawing.Size(30, 55);
            this.switchGargle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.switchGargle.TabIndex = 76;
            this.switchGargle.TabStop = false;
            this.switchGargle.Tag = "\"0\"";
            this.toolTip1.SetToolTip(this.switchGargle, "Click to enable/disable the Gargle effects");
            this.switchGargle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.switchGargle_MouseClick);
            // 
            // picVolSlider
            // 
            this.picVolSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picVolSlider.Location = new System.Drawing.Point(418, 215);
            this.picVolSlider.Name = "picVolSlider";
            this.picVolSlider.Size = new System.Drawing.Size(69, 19);
            this.picVolSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picVolSlider.TabIndex = 86;
            this.picVolSlider.TabStop = false;
            this.toolTip1.SetToolTip(this.picVolSlider, "Adjust volume");
            this.picVolSlider.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picVolSlider_MouseDoubleClick);
            this.picVolSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picVolSlider_MouseDown);
            this.picVolSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picVolSlider_MouseMove);
            this.picVolSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picVolSlider_MouseUp);
            // 
            // grpDistortion
            // 
            this.grpDistortion.BackColor = System.Drawing.Color.Silver;
            this.grpDistortion.Controls.Add(this.distortionCutoff);
            this.grpDistortion.Controls.Add(this.label21);
            this.grpDistortion.Controls.Add(this.distortionFrequency);
            this.grpDistortion.Controls.Add(this.label17);
            this.grpDistortion.Controls.Add(this.distortionBandwith);
            this.grpDistortion.Controls.Add(this.label18);
            this.grpDistortion.Controls.Add(this.distortionGain);
            this.grpDistortion.Controls.Add(this.label19);
            this.grpDistortion.Controls.Add(this.distortionEdge);
            this.grpDistortion.Controls.Add(this.label20);
            this.grpDistortion.Enabled = false;
            this.grpDistortion.Location = new System.Drawing.Point(548, 175);
            this.grpDistortion.Name = "grpDistortion";
            this.grpDistortion.Size = new System.Drawing.Size(432, 92);
            this.grpDistortion.TabIndex = 69;
            this.grpDistortion.TabStop = false;
            this.grpDistortion.Text = "Distortion";
            // 
            // distortionCutoff
            // 
            this.distortionCutoff.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.distortionCutoff.Location = new System.Drawing.Point(355, 56);
            this.distortionCutoff.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.distortionCutoff.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.distortionCutoff.Name = "distortionCutoff";
            this.distortionCutoff.Size = new System.Drawing.Size(51, 16);
            this.distortionCutoff.TabIndex = 13;
            this.distortionCutoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.distortionCutoff.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(233, 56);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(121, 13);
            this.label21.TabIndex = 12;
            this.label21.Text = "Pre Lowpass Cutoff (Hz)";
            // 
            // distortionFrequency
            // 
            this.distortionFrequency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.distortionFrequency.Location = new System.Drawing.Point(165, 56);
            this.distortionFrequency.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.distortionFrequency.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.distortionFrequency.Name = "distortionFrequency";
            this.distortionFrequency.Size = new System.Drawing.Size(51, 16);
            this.distortionFrequency.TabIndex = 11;
            this.distortionFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.distortionFrequency.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 56);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(155, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "Post EQ Center Frequency (Hz)";
            // 
            // distortionBandwith
            // 
            this.distortionBandwith.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.distortionBandwith.Location = new System.Drawing.Point(334, 24);
            this.distortionBandwith.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.distortionBandwith.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.distortionBandwith.Name = "distortionBandwith";
            this.distortionBandwith.Size = new System.Drawing.Size(51, 16);
            this.distortionBandwith.TabIndex = 9;
            this.distortionBandwith.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.distortionBandwith.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(212, 24);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(121, 13);
            this.label18.TabIndex = 7;
            this.label18.Text = "Post EQ Bandwidth (Hz)";
            // 
            // distortionGain
            // 
            this.distortionGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.distortionGain.Location = new System.Drawing.Point(155, 24);
            this.distortionGain.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.distortionGain.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            this.distortionGain.Name = "distortionGain";
            this.distortionGain.Size = new System.Drawing.Size(40, 16);
            this.distortionGain.TabIndex = 8;
            this.distortionGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(109, 24);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(51, 13);
            this.label19.TabIndex = 5;
            this.label19.Text = "Gain (dB)";
            // 
            // distortionEdge
            // 
            this.distortionEdge.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.distortionEdge.Location = new System.Drawing.Point(59, 24);
            this.distortionEdge.Name = "distortionEdge";
            this.distortionEdge.Size = new System.Drawing.Size(35, 16);
            this.distortionEdge.TabIndex = 7;
            this.distortionEdge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.distortionEdge.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 24);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(49, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "Edge (%)";
            // 
            // grpCompression
            // 
            this.grpCompression.BackColor = System.Drawing.Color.Silver;
            this.grpCompression.Controls.Add(this.compressionThreshold);
            this.grpCompression.Controls.Add(this.label22);
            this.grpCompression.Controls.Add(this.compressionRelease);
            this.grpCompression.Controls.Add(this.label23);
            this.grpCompression.Controls.Add(this.compressionRatio);
            this.grpCompression.Controls.Add(this.compressionPredelay);
            this.grpCompression.Controls.Add(this.compressionGain);
            this.grpCompression.Controls.Add(this.compressionAttack);
            this.grpCompression.Controls.Add(this.label24);
            this.grpCompression.Controls.Add(this.label25);
            this.grpCompression.Controls.Add(this.label26);
            this.grpCompression.Controls.Add(this.label27);
            this.grpCompression.Enabled = false;
            this.grpCompression.Location = new System.Drawing.Point(421, 284);
            this.grpCompression.Name = "grpCompression";
            this.grpCompression.Size = new System.Drawing.Size(354, 92);
            this.grpCompression.TabIndex = 68;
            this.grpCompression.TabStop = false;
            this.grpCompression.Text = "Compression";
            // 
            // compressionThreshold
            // 
            this.compressionThreshold.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.compressionThreshold.Location = new System.Drawing.Point(304, 56);
            this.compressionThreshold.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.compressionThreshold.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            this.compressionThreshold.Name = "compressionThreshold";
            this.compressionThreshold.Size = new System.Drawing.Size(35, 16);
            this.compressionThreshold.TabIndex = 5;
            this.compressionThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.compressionThreshold.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(227, 56);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(76, 13);
            this.label22.TabIndex = 10;
            this.label22.Text = "Threshold (dB)";
            // 
            // compressionRelease
            // 
            this.compressionRelease.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.compressionRelease.Location = new System.Drawing.Point(164, 56);
            this.compressionRelease.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.compressionRelease.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.compressionRelease.Name = "compressionRelease";
            this.compressionRelease.Size = new System.Drawing.Size(48, 16);
            this.compressionRelease.TabIndex = 4;
            this.compressionRelease.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.compressionRelease.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(96, 56);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(68, 13);
            this.label23.TabIndex = 8;
            this.label23.Text = "Release (ms)";
            // 
            // compressionRatio
            // 
            this.compressionRatio.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.compressionRatio.Location = new System.Drawing.Point(43, 56);
            this.compressionRatio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.compressionRatio.Name = "compressionRatio";
            this.compressionRatio.Size = new System.Drawing.Size(35, 16);
            this.compressionRatio.TabIndex = 3;
            this.compressionRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.compressionRatio.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // compressionPredelay
            // 
            this.compressionPredelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.compressionPredelay.Location = new System.Drawing.Point(307, 24);
            this.compressionPredelay.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.compressionPredelay.Name = "compressionPredelay";
            this.compressionPredelay.Size = new System.Drawing.Size(35, 16);
            this.compressionPredelay.TabIndex = 2;
            this.compressionPredelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.compressionPredelay.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // compressionGain
            // 
            this.compressionGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.compressionGain.Location = new System.Drawing.Point(189, 24);
            this.compressionGain.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.compressionGain.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            this.compressionGain.Name = "compressionGain";
            this.compressionGain.Size = new System.Drawing.Size(35, 16);
            this.compressionGain.TabIndex = 1;
            this.compressionGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // compressionAttack
            // 
            this.compressionAttack.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.compressionAttack.DecimalPlaces = 2;
            this.compressionAttack.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.compressionAttack.Location = new System.Drawing.Point(69, 24);
            this.compressionAttack.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.compressionAttack.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.compressionAttack.Name = "compressionAttack";
            this.compressionAttack.Size = new System.Drawing.Size(56, 16);
            this.compressionAttack.TabIndex = 0;
            this.compressionAttack.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.compressionAttack.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(9, 56);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(35, 13);
            this.label24.TabIndex = 6;
            this.label24.Text = "Ratio ";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(237, 24);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(70, 13);
            this.label25.TabIndex = 4;
            this.label25.Text = "Predelay (ms)";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(138, 24);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(51, 13);
            this.label26.TabIndex = 2;
            this.label26.Text = "Gain (dB)";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(9, 24);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(60, 13);
            this.label27.TabIndex = 1;
            this.label27.Text = "Attack (ms)";
            // 
            // grpFlanger
            // 
            this.grpFlanger.BackColor = System.Drawing.Color.Silver;
            this.grpFlanger.Controls.Add(this.flangerWaveForm);
            this.grpFlanger.Controls.Add(this.label28);
            this.grpFlanger.Controls.Add(this.flangerPhase);
            this.grpFlanger.Controls.Add(this.label29);
            this.grpFlanger.Controls.Add(this.flangerWetDry);
            this.grpFlanger.Controls.Add(this.label30);
            this.grpFlanger.Controls.Add(this.flangerFrequency);
            this.grpFlanger.Controls.Add(this.flangerFeedback);
            this.grpFlanger.Controls.Add(this.flangerDepth);
            this.grpFlanger.Controls.Add(this.flangerDelay);
            this.grpFlanger.Controls.Add(this.label31);
            this.grpFlanger.Controls.Add(this.label32);
            this.grpFlanger.Controls.Add(this.label33);
            this.grpFlanger.Controls.Add(this.label34);
            this.grpFlanger.Enabled = false;
            this.grpFlanger.Location = new System.Drawing.Point(548, 65);
            this.grpFlanger.Name = "grpFlanger";
            this.grpFlanger.Size = new System.Drawing.Size(432, 92);
            this.grpFlanger.TabIndex = 74;
            this.grpFlanger.TabStop = false;
            this.grpFlanger.Text = "Flanger";
            // 
            // flangerWaveForm
            // 
            this.flangerWaveForm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.flangerWaveForm.Location = new System.Drawing.Point(275, 56);
            this.flangerWaveForm.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.flangerWaveForm.Name = "flangerWaveForm";
            this.flangerWaveForm.Size = new System.Drawing.Size(35, 16);
            this.flangerWaveForm.TabIndex = 6;
            this.flangerWaveForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.flangerWaveForm.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(214, 56);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(62, 13);
            this.label28.TabIndex = 12;
            this.label28.Text = "Wave Form";
            // 
            // flangerPhase
            // 
            this.flangerPhase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.flangerPhase.Enabled = false;
            this.flangerPhase.Location = new System.Drawing.Point(159, 56);
            this.flangerPhase.Name = "flangerPhase";
            this.flangerPhase.Size = new System.Drawing.Size(35, 16);
            this.flangerPhase.TabIndex = 5;
            this.flangerPhase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(124, 56);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(37, 13);
            this.label29.TabIndex = 10;
            this.label29.Text = "Phase";
            // 
            // flangerWetDry
            // 
            this.flangerWetDry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.flangerWetDry.Location = new System.Drawing.Point(75, 56);
            this.flangerWetDry.Name = "flangerWetDry";
            this.flangerWetDry.Size = new System.Drawing.Size(35, 16);
            this.flangerWetDry.TabIndex = 4;
            this.flangerWetDry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(9, 56);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(67, 13);
            this.label30.TabIndex = 8;
            this.label30.Text = "Wet/Dry Mix";
            // 
            // flangerFrequency
            // 
            this.flangerFrequency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.flangerFrequency.Location = new System.Drawing.Point(387, 24);
            this.flangerFrequency.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.flangerFrequency.Name = "flangerFrequency";
            this.flangerFrequency.Size = new System.Drawing.Size(35, 16);
            this.flangerFrequency.TabIndex = 3;
            this.flangerFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // flangerFeedback
            // 
            this.flangerFeedback.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.flangerFeedback.Location = new System.Drawing.Point(282, 24);
            this.flangerFeedback.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.flangerFeedback.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
            this.flangerFeedback.Name = "flangerFeedback";
            this.flangerFeedback.Size = new System.Drawing.Size(35, 16);
            this.flangerFeedback.TabIndex = 2;
            this.flangerFeedback.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // flangerDepth
            // 
            this.flangerDepth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.flangerDepth.Location = new System.Drawing.Point(163, 24);
            this.flangerDepth.Name = "flangerDepth";
            this.flangerDepth.Size = new System.Drawing.Size(35, 16);
            this.flangerDepth.TabIndex = 1;
            this.flangerDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.flangerDepth.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // flangerDelay
            // 
            this.flangerDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.flangerDelay.Location = new System.Drawing.Point(63, 24);
            this.flangerDelay.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.flangerDelay.Name = "flangerDelay";
            this.flangerDelay.Size = new System.Drawing.Size(35, 16);
            this.flangerDelay.TabIndex = 0;
            this.flangerDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(331, 24);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(57, 13);
            this.label31.TabIndex = 6;
            this.label31.Text = "Frequency";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(212, 24);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(72, 13);
            this.label32.TabIndex = 4;
            this.label32.Text = "Feedback (%)";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(112, 24);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(53, 13);
            this.label33.TabIndex = 2;
            this.label33.Text = "Depth (%)";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(9, 24);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(56, 13);
            this.label34.TabIndex = 1;
            this.label34.Text = "Delay (ms)";
            // 
            // grpGargle
            // 
            this.grpGargle.BackColor = System.Drawing.Color.Silver;
            this.grpGargle.Controls.Add(this.gargleWaveShape);
            this.grpGargle.Controls.Add(this.label36);
            this.grpGargle.Controls.Add(this.gargleRate);
            this.grpGargle.Controls.Add(this.label38);
            this.grpGargle.Enabled = false;
            this.grpGargle.Location = new System.Drawing.Point(839, 284);
            this.grpGargle.Name = "grpGargle";
            this.grpGargle.Size = new System.Drawing.Size(141, 92);
            this.grpGargle.TabIndex = 75;
            this.grpGargle.TabStop = false;
            this.grpGargle.Text = "Gargle";
            // 
            // gargleWaveShape
            // 
            this.gargleWaveShape.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gargleWaveShape.Location = new System.Drawing.Point(79, 56);
            this.gargleWaveShape.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gargleWaveShape.Name = "gargleWaveShape";
            this.gargleWaveShape.Size = new System.Drawing.Size(51, 16);
            this.gargleWaveShape.TabIndex = 14;
            this.gargleWaveShape.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.gargleWaveShape.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(9, 56);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(70, 13);
            this.label36.TabIndex = 10;
            this.label36.Text = "Wave Shape";
            // 
            // gargleRate
            // 
            this.gargleRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gargleRate.Location = new System.Drawing.Point(62, 24);
            this.gargleRate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gargleRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gargleRate.Name = "gargleRate";
            this.gargleRate.Size = new System.Drawing.Size(51, 16);
            this.gargleRate.TabIndex = 12;
            this.gargleRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.gargleRate.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(9, 24);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(52, 13);
            this.label38.TabIndex = 0;
            this.label38.Text = "Rate (Hz)";
            // 
            // btnImport
            // 
            this.btnImport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Location = new System.Drawing.Point(349, 9);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 23);
            this.btnImport.TabIndex = 1;
            this.btnImport.TabStop = false;
            this.btnImport.Text = "Import Preset";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Location = new System.Drawing.Point(349, 38);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.TabStop = false;
            this.btnExport.Text = "Export Preset";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnApply
            // 
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.Enabled = false;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Location = new System.Drawing.Point(702, 23);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.TabStop = false;
            this.btnApply.Text = "Apply Effects";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlay.Enabled = false;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Location = new System.Drawing.Point(788, 23);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(40, 23);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.TabStop = false;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPause.Enabled = false;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Location = new System.Drawing.Point(834, 23);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(48, 23);
            this.btnPause.TabIndex = 5;
            this.btnPause.TabStop = false;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(888, 23);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(40, 23);
            this.btnStop.TabIndex = 6;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Enabled = false;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(934, 23);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(46, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // picSpect
            // 
            this.picSpect.Location = new System.Drawing.Point(548, 10);
            this.picSpect.Name = "picSpect";
            this.picSpect.Size = new System.Drawing.Size(105, 48);
            this.picSpect.TabIndex = 84;
            this.picSpect.TabStop = false;
            this.picSpect.Visible = false;
            // 
            // playbackTimer
            // 
            this.playbackTimer.Tick += new System.EventHandler(this.playbackTimer_Tick);
            // 
            // picVolBackground
            // 
            this.picVolBackground.Location = new System.Drawing.Point(433, 180);
            this.picVolBackground.Name = "picVolBackground";
            this.picVolBackground.Size = new System.Drawing.Size(39, 91);
            this.picVolBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picVolBackground.TabIndex = 85;
            this.picVolBackground.TabStop = false;
            // 
            // MiniStudio
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(992, 387);
            this.Controls.Add(this.picVolSlider);
            this.Controls.Add(this.picVolBackground);
            this.Controls.Add(this.picSpect);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.switchGargle);
            this.Controls.Add(this.grpGargle);
            this.Controls.Add(this.grpFlanger);
            this.Controls.Add(this.switchFlanger);
            this.Controls.Add(this.switchDistortion);
            this.Controls.Add(this.switchCompression);
            this.Controls.Add(this.grpDistortion);
            this.Controls.Add(this.grpCompression);
            this.Controls.Add(this.picWorking);
            this.Controls.Add(this.switchReverb);
            this.Controls.Add(this.switchEcho);
            this.Controls.Add(this.switchChorus);
            this.Controls.Add(this.grpReverb);
            this.Controls.Add(this.grpEcho);
            this.Controls.Add(this.grpChorus);
            this.Controls.Add(this.lblInfo);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MiniStudio";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mini Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Vocalizer_FormClosing);
            this.Shown += new System.EventHandler(this.Vocalizer_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Vocalizer_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Vocalizer_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MiniStudio_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MiniStudio_KeyUp);
            this.grpChorus.ResumeLayout(false);
            this.grpChorus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chorusWaveForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusPhase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusWetDry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusFeedback)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chorusDelay)).EndInit();
            this.grpEcho.ResumeLayout(false);
            this.grpEcho.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EchoWetDry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.echoRightDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.echoLeftDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.echoFeedback)).EndInit();
            this.grpReverb.ResumeLayout(false);
            this.grpReverb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reverbTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reverbMix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reverbGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reverbRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchChorus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchEcho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchReverb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchFlanger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchDistortion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchCompression)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchGargle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolSlider)).EndInit();
            this.grpDistortion.ResumeLayout(false);
            this.grpDistortion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.distortionCutoff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.distortionFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.distortionBandwith)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.distortionGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.distortionEdge)).EndInit();
            this.grpCompression.ResumeLayout(false);
            this.grpCompression.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compressionThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionRelease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionPredelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionAttack)).EndInit();
            this.grpFlanger.ResumeLayout(false);
            this.grpFlanger.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flangerWaveForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerPhase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerWetDry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerFeedback)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flangerDelay)).EndInit();
            this.grpGargle.ResumeLayout(false);
            this.grpGargle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gargleWaveShape)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gargleRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolBackground)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox grpChorus;
        private System.Windows.Forms.GroupBox grpEcho;
        private System.Windows.Forms.GroupBox grpReverb;
        private System.Windows.Forms.PictureBox switchChorus;
        private System.Windows.Forms.PictureBox switchEcho;
        private System.Windows.Forms.PictureBox switchReverb;
        private System.Windows.Forms.NumericUpDown chorusDepth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown chorusDelay;
        private System.Windows.Forms.NumericUpDown chorusFeedback;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown chorusFrequency;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown chorusPhase;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown chorusWetDry;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown chorusWaveForm;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown echoFeedback;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown EchoWetDry;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown echoRightDelay;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown echoLeftDelay;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox echoPanDelay;
        private System.Windows.Forms.NumericUpDown reverbGain;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown reverbRatio;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown reverbTime;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown reverbMix;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox picWorking;
        private System.ComponentModel.BackgroundWorker effectsWorker;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox grpDistortion;
        private System.Windows.Forms.NumericUpDown distortionBandwith;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown distortionGain;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown distortionEdge;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.GroupBox grpCompression;
        private System.Windows.Forms.NumericUpDown compressionThreshold;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown compressionRelease;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown compressionRatio;
        private System.Windows.Forms.NumericUpDown compressionPredelay;
        private System.Windows.Forms.NumericUpDown compressionGain;
        private System.Windows.Forms.NumericUpDown compressionAttack;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.PictureBox switchFlanger;
        private System.Windows.Forms.PictureBox switchDistortion;
        private System.Windows.Forms.PictureBox switchCompression;
        private System.Windows.Forms.NumericUpDown distortionCutoff;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown distortionFrequency;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox grpFlanger;
        private System.Windows.Forms.NumericUpDown flangerWaveForm;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.NumericUpDown flangerPhase;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.NumericUpDown flangerWetDry;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.NumericUpDown flangerFrequency;
        private System.Windows.Forms.NumericUpDown flangerFeedback;
        private System.Windows.Forms.NumericUpDown flangerDepth;
        private System.Windows.Forms.NumericUpDown flangerDelay;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.PictureBox switchGargle;
        private System.Windows.Forms.GroupBox grpGargle;
        private System.Windows.Forms.NumericUpDown gargleWaveShape;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.NumericUpDown gargleRate;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox picSpect;
        private System.Windows.Forms.Timer playbackTimer;
        private System.Windows.Forms.PictureBox picVolBackground;
        private System.Windows.Forms.PictureBox picVolSlider;
    }
}