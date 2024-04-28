namespace ConvertApp
{
    partial class FormOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
            this.CmbLanguage = new System.Windows.Forms.ComboBox();
            this.LblSelectLanguage = new System.Windows.Forms.Label();
            this.LblVersionName = new System.Windows.Forms.Label();
            this.BtnAccept = new System.Windows.Forms.Button();
            this.LblMachineTranslated = new System.Windows.Forms.Label();
            this.CmbConsoleTextSize = new System.Windows.Forms.ComboBox();
            this.LblConsoleTextSize = new System.Windows.Forms.Label();
            this.TabCtrlOptions = new System.Windows.Forms.TabControl();
            this.TabGeneral = new System.Windows.Forms.TabPage();
            this.LblFfmpegVersion = new System.Windows.Forms.Label();
            this.BtnDownloadFfmpegExe = new System.Windows.Forms.Button();
            this.TabConvert = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LblRotation = new System.Windows.Forms.Label();
            this.CmbRotation = new System.Windows.Forms.ComboBox();
            this.BtnResetCustomParams = new System.Windows.Forms.Button();
            this.LblTutorial = new System.Windows.Forms.LinkLabel();
            this.LblCustomParams = new System.Windows.Forms.Label();
            this.ChkBoxEnabled = new System.Windows.Forms.CheckBox();
            this.TxtCustomParameters = new System.Windows.Forms.TextBox();
            this.TabIntegration = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.RdioApp = new System.Windows.Forms.RadioButton();
            this.RdioConsole = new System.Windows.Forms.RadioButton();
            this.RdioNone = new System.Windows.Forms.RadioButton();
            this.PctAdmin = new System.Windows.Forms.PictureBox();
            this.LblExplorer = new System.Windows.Forms.Label();
            this.TooltipOptions = new System.Windows.Forms.ToolTip(this.components);
            this.LblIntegrationExplanation = new System.Windows.Forms.Label();
            this.GrpBoxExplanation = new System.Windows.Forms.GroupBox();
            this.TxtFramerate = new System.Windows.Forms.TextBox();
            this.TxtBitrate = new System.Windows.Forms.TextBox();
            this.TabCredits = new System.Windows.Forms.TabPage();
            this.FlowCredits = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TabCtrlOptions.SuspendLayout();
            this.TabGeneral.SuspendLayout();
            this.TabConvert.SuspendLayout();
            this.TabIntegration.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PctAdmin)).BeginInit();
            this.GrpBoxExplanation.SuspendLayout();
            this.TabCredits.SuspendLayout();
            this.FlowCredits.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmbLanguage
            // 
            this.CmbLanguage.FormattingEnabled = true;
            this.CmbLanguage.Location = new System.Drawing.Point(8, 21);
            this.CmbLanguage.Name = "CmbLanguage";
            this.CmbLanguage.Size = new System.Drawing.Size(260, 21);
            this.CmbLanguage.TabIndex = 0;
            this.CmbLanguage.Text = "English";
            this.CmbLanguage.SelectedIndexChanged += new System.EventHandler(this.CmbLanguage_SelectedIndexChanged);
            // 
            // LblSelectLanguage
            // 
            this.LblSelectLanguage.AutoSize = true;
            this.LblSelectLanguage.ForeColor = System.Drawing.Color.Silver;
            this.LblSelectLanguage.Location = new System.Drawing.Point(5, 3);
            this.LblSelectLanguage.Name = "LblSelectLanguage";
            this.LblSelectLanguage.Size = new System.Drawing.Size(100, 13);
            this.LblSelectLanguage.TabIndex = 1;
            this.LblSelectLanguage.Text = "Selected Language";
            this.LblSelectLanguage.Click += new System.EventHandler(this.label1_Click);
            // 
            // LblVersionName
            // 
            this.LblVersionName.AutoSize = true;
            this.LblVersionName.ForeColor = System.Drawing.Color.Silver;
            this.LblVersionName.Location = new System.Drawing.Point(72, 188);
            this.LblVersionName.MinimumSize = new System.Drawing.Size(200, 0);
            this.LblVersionName.Name = "LblVersionName";
            this.LblVersionName.Size = new System.Drawing.Size(200, 13);
            this.LblVersionName.TabIndex = 2;
            this.LblVersionName.Text = "App V: x.x";
            this.LblVersionName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LblVersionName.Click += new System.EventHandler(this.LblVersionName_Click);
            // 
            // BtnAccept
            // 
            this.BtnAccept.AutoSize = true;
            this.BtnAccept.Location = new System.Drawing.Point(12, 179);
            this.BtnAccept.Name = "BtnAccept";
            this.BtnAccept.Size = new System.Drawing.Size(59, 30);
            this.BtnAccept.TabIndex = 3;
            this.BtnAccept.Text = "Accept";
            this.BtnAccept.UseVisualStyleBackColor = true;
            this.BtnAccept.Click += new System.EventHandler(this.BtnAccept_Click);
            // 
            // LblMachineTranslated
            // 
            this.LblMachineTranslated.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LblMachineTranslated.AutoSize = true;
            this.LblMachineTranslated.BackColor = System.Drawing.Color.Transparent;
            this.LblMachineTranslated.Location = new System.Drawing.Point(68, 3);
            this.LblMachineTranslated.MinimumSize = new System.Drawing.Size(200, 0);
            this.LblMachineTranslated.Name = "LblMachineTranslated";
            this.LblMachineTranslated.Size = new System.Drawing.Size(200, 13);
            this.LblMachineTranslated.TabIndex = 4;
            this.LblMachineTranslated.Text = "Japanese is machine translated";
            this.LblMachineTranslated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CmbConsoleTextSize
            // 
            this.CmbConsoleTextSize.FormattingEnabled = true;
            this.CmbConsoleTextSize.Location = new System.Drawing.Point(8, 71);
            this.CmbConsoleTextSize.Name = "CmbConsoleTextSize";
            this.CmbConsoleTextSize.Size = new System.Drawing.Size(260, 21);
            this.CmbConsoleTextSize.TabIndex = 5;
            this.CmbConsoleTextSize.Text = "Small";
            this.CmbConsoleTextSize.SelectedIndexChanged += new System.EventHandler(this.CmbConsoleTextSize_SelectedIndexChanged);
            // 
            // LblConsoleTextSize
            // 
            this.LblConsoleTextSize.AutoSize = true;
            this.LblConsoleTextSize.ForeColor = System.Drawing.Color.Silver;
            this.LblConsoleTextSize.Location = new System.Drawing.Point(5, 53);
            this.LblConsoleTextSize.Name = "LblConsoleTextSize";
            this.LblConsoleTextSize.Size = new System.Drawing.Size(92, 13);
            this.LblConsoleTextSize.TabIndex = 6;
            this.LblConsoleTextSize.Text = "Console Text Size";
            this.LblConsoleTextSize.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // TabCtrlOptions
            // 
            this.TabCtrlOptions.Controls.Add(this.TabGeneral);
            this.TabCtrlOptions.Controls.Add(this.TabConvert);
            this.TabCtrlOptions.Controls.Add(this.TabIntegration);
            this.TabCtrlOptions.Controls.Add(this.TabCredits);
            this.TabCtrlOptions.Location = new System.Drawing.Point(0, 12);
            this.TabCtrlOptions.Multiline = true;
            this.TabCtrlOptions.Name = "TabCtrlOptions";
            this.TabCtrlOptions.SelectedIndex = 0;
            this.TabCtrlOptions.Size = new System.Drawing.Size(285, 162);
            this.TabCtrlOptions.TabIndex = 7;
            // 
            // TabGeneral
            // 
            this.TabGeneral.BackColor = System.Drawing.Color.White;
            this.TabGeneral.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.TabGeneral.Controls.Add(this.LblFfmpegVersion);
            this.TabGeneral.Controls.Add(this.LblSelectLanguage);
            this.TabGeneral.Controls.Add(this.BtnDownloadFfmpegExe);
            this.TabGeneral.Controls.Add(this.CmbConsoleTextSize);
            this.TabGeneral.Controls.Add(this.LblMachineTranslated);
            this.TabGeneral.Controls.Add(this.LblConsoleTextSize);
            this.TabGeneral.Controls.Add(this.CmbLanguage);
            this.TabGeneral.Location = new System.Drawing.Point(4, 22);
            this.TabGeneral.Name = "TabGeneral";
            this.TabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.TabGeneral.Size = new System.Drawing.Size(277, 136);
            this.TabGeneral.TabIndex = 0;
            this.TabGeneral.Text = "General";
            // 
            // LblFfmpegVersion
            // 
            this.LblFfmpegVersion.ForeColor = System.Drawing.Color.Silver;
            this.LblFfmpegVersion.Location = new System.Drawing.Point(8, 98);
            this.LblFfmpegVersion.Name = "LblFfmpegVersion";
            this.LblFfmpegVersion.Size = new System.Drawing.Size(125, 32);
            this.LblFfmpegVersion.TabIndex = 8;
            this.LblFfmpegVersion.Text = "Ffmpeg Version 1.2.4";
            this.LblFfmpegVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LblFfmpegVersion.Visible = false;
            // 
            // BtnDownloadFfmpegExe
            // 
            this.BtnDownloadFfmpegExe.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDownloadFfmpegExe.Location = new System.Drawing.Point(139, 98);
            this.BtnDownloadFfmpegExe.Name = "BtnDownloadFfmpegExe";
            this.BtnDownloadFfmpegExe.Size = new System.Drawing.Size(129, 32);
            this.BtnDownloadFfmpegExe.TabIndex = 7;
            this.BtnDownloadFfmpegExe.Text = "Download Ffmpeg.exe";
            this.BtnDownloadFfmpegExe.UseVisualStyleBackColor = true;
            // 
            // TabConvert
            // 
            this.TabConvert.AutoScroll = true;
            this.TabConvert.BackColor = System.Drawing.Color.White;
            this.TabConvert.Controls.Add(this.TxtBitrate);
            this.TabConvert.Controls.Add(this.TxtFramerate);
            this.TabConvert.Controls.Add(this.label2);
            this.TabConvert.Controls.Add(this.label1);
            this.TabConvert.Controls.Add(this.LblRotation);
            this.TabConvert.Controls.Add(this.CmbRotation);
            this.TabConvert.Controls.Add(this.BtnResetCustomParams);
            this.TabConvert.Controls.Add(this.LblTutorial);
            this.TabConvert.Controls.Add(this.LblCustomParams);
            this.TabConvert.Controls.Add(this.ChkBoxEnabled);
            this.TabConvert.Controls.Add(this.TxtCustomParameters);
            this.TabConvert.Location = new System.Drawing.Point(4, 22);
            this.TabConvert.Name = "TabConvert";
            this.TabConvert.Padding = new System.Windows.Forms.Padding(3);
            this.TabConvert.Size = new System.Drawing.Size(277, 136);
            this.TabConvert.TabIndex = 1;
            this.TabConvert.Text = "Convert";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(155, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Framerate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(80, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Bitrate";
            // 
            // LblRotation
            // 
            this.LblRotation.AutoSize = true;
            this.LblRotation.ForeColor = System.Drawing.Color.Silver;
            this.LblRotation.Location = new System.Drawing.Point(5, 76);
            this.LblRotation.Name = "LblRotation";
            this.LblRotation.Size = new System.Drawing.Size(47, 13);
            this.LblRotation.TabIndex = 6;
            this.LblRotation.Text = "Rotation";
            // 
            // CmbRotation
            // 
            this.CmbRotation.FormattingEnabled = true;
            this.CmbRotation.ItemHeight = 13;
            this.CmbRotation.Location = new System.Drawing.Point(8, 97);
            this.CmbRotation.Name = "CmbRotation";
            this.CmbRotation.Size = new System.Drawing.Size(69, 21);
            this.CmbRotation.TabIndex = 5;
            this.CmbRotation.Text = "0";
            // 
            // BtnResetCustomParams
            // 
            this.BtnResetCustomParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnResetCustomParams.Location = new System.Drawing.Point(196, 45);
            this.BtnResetCustomParams.Name = "BtnResetCustomParams";
            this.BtnResetCustomParams.Size = new System.Drawing.Size(72, 22);
            this.BtnResetCustomParams.TabIndex = 4;
            this.BtnResetCustomParams.Text = "Default";
            this.BtnResetCustomParams.UseVisualStyleBackColor = true;
            // 
            // LblTutorial
            // 
            this.LblTutorial.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LblTutorial.AutoSize = true;
            this.LblTutorial.BackColor = System.Drawing.Color.Transparent;
            this.LblTutorial.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LblTutorial.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LblTutorial.Location = new System.Drawing.Point(131, 49);
            this.LblTutorial.Name = "LblTutorial";
            this.LblTutorial.Size = new System.Drawing.Size(62, 13);
            this.LblTutorial.TabIndex = 3;
            this.LblTutorial.TabStop = true;
            this.LblTutorial.Text = "Explanation";
            this.LblTutorial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblCustomParams
            // 
            this.LblCustomParams.AutoSize = true;
            this.LblCustomParams.BackColor = System.Drawing.Color.Transparent;
            this.LblCustomParams.ForeColor = System.Drawing.Color.Silver;
            this.LblCustomParams.Location = new System.Drawing.Point(5, 3);
            this.LblCustomParams.Name = "LblCustomParams";
            this.LblCustomParams.Size = new System.Drawing.Size(98, 13);
            this.LblCustomParams.TabIndex = 2;
            this.LblCustomParams.Text = "Custom Parameters";
            // 
            // ChkBoxEnabled
            // 
            this.ChkBoxEnabled.AutoSize = true;
            this.ChkBoxEnabled.Location = new System.Drawing.Point(8, 48);
            this.ChkBoxEnabled.Name = "ChkBoxEnabled";
            this.ChkBoxEnabled.Size = new System.Drawing.Size(65, 17);
            this.ChkBoxEnabled.TabIndex = 1;
            this.ChkBoxEnabled.Text = "Enabled";
            this.ChkBoxEnabled.UseVisualStyleBackColor = true;
            // 
            // TxtCustomParameters
            // 
            this.TxtCustomParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtCustomParameters.Enabled = false;
            this.TxtCustomParameters.Location = new System.Drawing.Point(8, 21);
            this.TxtCustomParameters.Name = "TxtCustomParameters";
            this.TxtCustomParameters.Size = new System.Drawing.Size(260, 20);
            this.TxtCustomParameters.TabIndex = 0;
            this.TxtCustomParameters.Text = "-y -i \\\"{0}\\\" \\\"{1}\\\"";
            // 
            // TabIntegration
            // 
            this.TabIntegration.Controls.Add(this.GrpBoxExplanation);
            this.TabIntegration.Controls.Add(this.flowLayoutPanel1);
            this.TabIntegration.Controls.Add(this.LblExplorer);
            this.TabIntegration.Location = new System.Drawing.Point(4, 22);
            this.TabIntegration.Name = "TabIntegration";
            this.TabIntegration.Size = new System.Drawing.Size(277, 136);
            this.TabIntegration.TabIndex = 2;
            this.TabIntegration.Text = "Integration";
            this.TabIntegration.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.RdioApp);
            this.flowLayoutPanel1.Controls.Add(this.RdioConsole);
            this.flowLayoutPanel1.Controls.Add(this.RdioNone);
            this.flowLayoutPanel1.Controls.Add(this.PctAdmin);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(265, 26);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // RdioApp
            // 
            this.RdioApp.AutoSize = true;
            this.RdioApp.Location = new System.Drawing.Point(3, 3);
            this.RdioApp.Name = "RdioApp";
            this.RdioApp.Size = new System.Drawing.Size(44, 17);
            this.RdioApp.TabIndex = 2;
            this.RdioApp.Text = "App";
            this.RdioApp.UseVisualStyleBackColor = true;
            this.RdioApp.CheckedChanged += new System.EventHandler(this.RdioApp_CheckedChanged);
            // 
            // RdioConsole
            // 
            this.RdioConsole.AutoSize = true;
            this.RdioConsole.Location = new System.Drawing.Point(53, 3);
            this.RdioConsole.Name = "RdioConsole";
            this.RdioConsole.Size = new System.Drawing.Size(63, 17);
            this.RdioConsole.TabIndex = 1;
            this.RdioConsole.Text = "Console";
            this.RdioConsole.UseVisualStyleBackColor = true;
            this.RdioConsole.Visible = false;
            // 
            // RdioNone
            // 
            this.RdioNone.AutoSize = true;
            this.RdioNone.Checked = true;
            this.RdioNone.Location = new System.Drawing.Point(122, 3);
            this.RdioNone.Name = "RdioNone";
            this.RdioNone.Size = new System.Drawing.Size(51, 17);
            this.RdioNone.TabIndex = 3;
            this.RdioNone.TabStop = true;
            this.RdioNone.Text = "None";
            this.RdioNone.UseVisualStyleBackColor = true;
            this.RdioNone.CheckedChanged += new System.EventHandler(this.RdioNone_CheckedChanged);
            // 
            // PctAdmin
            // 
            this.PctAdmin.Image = global::ConvertApp.Properties.Resources.priv;
            this.PctAdmin.Location = new System.Drawing.Point(176, 0);
            this.PctAdmin.Margin = new System.Windows.Forms.Padding(0);
            this.PctAdmin.Name = "PctAdmin";
            this.PctAdmin.Size = new System.Drawing.Size(31, 23);
            this.PctAdmin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PctAdmin.TabIndex = 4;
            this.PctAdmin.TabStop = false;
            // 
            // LblExplorer
            // 
            this.LblExplorer.AutoSize = true;
            this.LblExplorer.ForeColor = System.Drawing.Color.Silver;
            this.LblExplorer.Location = new System.Drawing.Point(3, 3);
            this.LblExplorer.Name = "LblExplorer";
            this.LblExplorer.Size = new System.Drawing.Size(161, 13);
            this.LblExplorer.TabIndex = 0;
            this.LblExplorer.Text = "Windows Explorer Context Menu";
            // 
            // TooltipOptions
            // 
            this.TooltipOptions.AutomaticDelay = 200;
            this.TooltipOptions.AutoPopDelay = 21000;
            this.TooltipOptions.InitialDelay = 200;
            this.TooltipOptions.IsBalloon = true;
            this.TooltipOptions.ReshowDelay = 40;
            // 
            // LblIntegrationExplanation
            // 
            this.LblIntegrationExplanation.ForeColor = System.Drawing.Color.DarkGray;
            this.LblIntegrationExplanation.Location = new System.Drawing.Point(1, 6);
            this.LblIntegrationExplanation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.LblIntegrationExplanation.Name = "LblIntegrationExplanation";
            this.LblIntegrationExplanation.Padding = new System.Windows.Forms.Padding(10);
            this.LblIntegrationExplanation.Size = new System.Drawing.Size(264, 73);
            this.LblIntegrationExplanation.TabIndex = 5;
            this.LblIntegrationExplanation.Text = "Set this to \"App\" to add a \"Convert\" option to the right click menu in Windows. S" +
    "o if you right click an mp4 file, it will let you instantly convert it from that" +
    " menu";
            this.LblIntegrationExplanation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GrpBoxExplanation
            // 
            this.GrpBoxExplanation.Controls.Add(this.LblIntegrationExplanation);
            this.GrpBoxExplanation.Location = new System.Drawing.Point(6, 52);
            this.GrpBoxExplanation.Name = "GrpBoxExplanation";
            this.GrpBoxExplanation.Size = new System.Drawing.Size(265, 79);
            this.GrpBoxExplanation.TabIndex = 6;
            this.GrpBoxExplanation.TabStop = false;
            // 
            // TxtFramerate
            // 
            this.TxtFramerate.Location = new System.Drawing.Point(158, 97);
            this.TxtFramerate.Name = "TxtFramerate";
            this.TxtFramerate.Size = new System.Drawing.Size(69, 20);
            this.TxtFramerate.TabIndex = 11;
            // 
            // TxtBitrate
            // 
            this.TxtBitrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBitrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtBitrate.Location = new System.Drawing.Point(83, 97);
            this.TxtBitrate.Name = "TxtBitrate";
            this.TxtBitrate.Size = new System.Drawing.Size(69, 20);
            this.TxtBitrate.TabIndex = 12;
            // 
            // TabCredits
            // 
            this.TabCredits.Controls.Add(this.FlowCredits);
            this.TabCredits.Location = new System.Drawing.Point(4, 22);
            this.TabCredits.Name = "TabCredits";
            this.TabCredits.Size = new System.Drawing.Size(277, 136);
            this.TabCredits.TabIndex = 3;
            this.TabCredits.Text = "Credits";
            this.TabCredits.UseVisualStyleBackColor = true;
            // 
            // FlowCredits
            // 
            this.FlowCredits.Controls.Add(this.label7);
            this.FlowCredits.Controls.Add(this.label3);
            this.FlowCredits.Controls.Add(this.label4);
            this.FlowCredits.Controls.Add(this.label5);
            this.FlowCredits.Controls.Add(this.label6);
            this.FlowCredits.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FlowCredits.Location = new System.Drawing.Point(4, 4);
            this.FlowCredits.Name = "FlowCredits";
            this.FlowCredits.Size = new System.Drawing.Size(270, 129);
            this.FlowCredits.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Eben Sorkin, Mirko Velimirović - Splines Sans Mono";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Komar Dews - Convert icon";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Material Icons";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Ffmpeg";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Silver;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Credits";
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 216);
            this.Controls.Add(this.TabCtrlOptions);
            this.Controls.Add(this.BtnAccept);
            this.Controls.Add(this.LblVersionName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormOptions";
            this.ShowInTaskbar = false;
            this.Text = "Options";
            this.TabCtrlOptions.ResumeLayout(false);
            this.TabGeneral.ResumeLayout(false);
            this.TabGeneral.PerformLayout();
            this.TabConvert.ResumeLayout(false);
            this.TabConvert.PerformLayout();
            this.TabIntegration.ResumeLayout(false);
            this.TabIntegration.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PctAdmin)).EndInit();
            this.GrpBoxExplanation.ResumeLayout(false);
            this.TabCredits.ResumeLayout(false);
            this.FlowCredits.ResumeLayout(false);
            this.FlowCredits.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CmbLanguage;
        private System.Windows.Forms.Label LblSelectLanguage;
        private System.Windows.Forms.Label LblVersionName;
        private System.Windows.Forms.Button BtnAccept;
        private System.Windows.Forms.Label LblMachineTranslated;
        private System.Windows.Forms.ComboBox CmbConsoleTextSize;
        private System.Windows.Forms.Label LblConsoleTextSize;
        private System.Windows.Forms.TabControl TabCtrlOptions;
        private System.Windows.Forms.TabPage TabGeneral;
        private System.Windows.Forms.TabPage TabConvert;
        private System.Windows.Forms.TextBox TxtCustomParameters;
        private System.Windows.Forms.LinkLabel LblTutorial;
        private System.Windows.Forms.Label LblCustomParams;
        private System.Windows.Forms.CheckBox ChkBoxEnabled;
        private System.Windows.Forms.ToolTip TooltipOptions;
        private System.Windows.Forms.Button BtnResetCustomParams;
        private System.Windows.Forms.ComboBox CmbRotation;
        private System.Windows.Forms.Label LblRotation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnDownloadFfmpegExe;
        private System.Windows.Forms.TabPage TabIntegration;
        private System.Windows.Forms.RadioButton RdioApp;
        private System.Windows.Forms.RadioButton RdioConsole;
        private System.Windows.Forms.Label LblExplorer;
        private System.Windows.Forms.RadioButton RdioNone;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label LblFfmpegVersion;
        private System.Windows.Forms.PictureBox PctAdmin;
        private System.Windows.Forms.Label LblIntegrationExplanation;
        private System.Windows.Forms.GroupBox GrpBoxExplanation;
        private System.Windows.Forms.TextBox TxtBitrate;
        private System.Windows.Forms.TextBox TxtFramerate;
        private System.Windows.Forms.TabPage TabCredits;
        private System.Windows.Forms.FlowLayoutPanel FlowCredits;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}