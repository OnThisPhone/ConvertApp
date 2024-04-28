namespace ConvertApp
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.lblSelectFiles = new System.Windows.Forms.Label();
            this.BtnSelectFileInput = new System.Windows.Forms.Button();
            this.BtnSelectFileOutput = new System.Windows.Forms.Button();
            this.LblFileInput = new System.Windows.Forms.Label();
            this.LblFileOutput = new System.Windows.Forms.Label();
            this.CmbFileFormat = new System.Windows.Forms.ComboBox();
            this.LblFileFormatInput = new System.Windows.Forms.Label();
            this.PnlOutput = new System.Windows.Forms.Panel();
            this.LblConsole = new System.Windows.Forms.Label();
            this.TotiCommon = new System.Windows.Forms.ToolTip(this.components);
            this.PrgBarConvert = new System.Windows.Forms.ProgressBar();
            this.BtnConvert = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ChkAutoscroll = new System.Windows.Forms.CheckBox();
            this.BtnConvertOptions = new System.Windows.Forms.Button();
            this.BtnHelp = new System.Windows.Forms.Button();
            this.TotiBalloon = new System.Windows.Forms.ToolTip(this.components);
            this.PnlOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSelectFiles
            // 
            this.lblSelectFiles.AutoSize = true;
            this.lblSelectFiles.ForeColor = System.Drawing.Color.Silver;
            this.lblSelectFiles.Location = new System.Drawing.Point(15, 7);
            this.lblSelectFiles.Name = "lblSelectFiles";
            this.lblSelectFiles.Size = new System.Drawing.Size(77, 16);
            this.lblSelectFiles.TabIndex = 0;
            this.lblSelectFiles.Text = "Select Files";
            // 
            // BtnSelectFileInput
            // 
            this.BtnSelectFileInput.Location = new System.Drawing.Point(12, 32);
            this.BtnSelectFileInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSelectFileInput.Name = "BtnSelectFileInput";
            this.BtnSelectFileInput.Size = new System.Drawing.Size(103, 50);
            this.BtnSelectFileInput.TabIndex = 1;
            this.BtnSelectFileInput.Text = "File Input";
            this.BtnSelectFileInput.UseVisualStyleBackColor = true;
            this.BtnSelectFileInput.Click += new System.EventHandler(this.BtnSelectFileFrom_Click);
            // 
            // BtnSelectFileOutput
            // 
            this.BtnSelectFileOutput.Location = new System.Drawing.Point(12, 89);
            this.BtnSelectFileOutput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSelectFileOutput.Name = "BtnSelectFileOutput";
            this.BtnSelectFileOutput.Size = new System.Drawing.Size(103, 50);
            this.BtnSelectFileOutput.TabIndex = 2;
            this.BtnSelectFileOutput.Text = "Save Location";
            this.BtnSelectFileOutput.UseVisualStyleBackColor = true;
            this.BtnSelectFileOutput.Click += new System.EventHandler(this.BtnSelectFileOutput_Click);
            // 
            // LblFileInput
            // 
            this.LblFileInput.AutoEllipsis = true;
            this.LblFileInput.AutoSize = true;
            this.LblFileInput.Location = new System.Drawing.Point(121, 32);
            this.LblFileInput.MaximumSize = new System.Drawing.Size(280, 0);
            this.LblFileInput.MinimumSize = new System.Drawing.Size(280, 50);
            this.LblFileInput.Name = "LblFileInput";
            this.LblFileInput.Size = new System.Drawing.Size(280, 50);
            this.LblFileInput.TabIndex = 3;
            this.LblFileInput.Text = "(No file selected)";
            this.LblFileInput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblFileOutput
            // 
            this.LblFileOutput.AutoEllipsis = true;
            this.LblFileOutput.AutoSize = true;
            this.LblFileOutput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblFileOutput.Location = new System.Drawing.Point(121, 89);
            this.LblFileOutput.MaximumSize = new System.Drawing.Size(280, 0);
            this.LblFileOutput.MinimumSize = new System.Drawing.Size(247, 50);
            this.LblFileOutput.Name = "LblFileOutput";
            this.LblFileOutput.Size = new System.Drawing.Size(247, 50);
            this.LblFileOutput.TabIndex = 4;
            this.LblFileOutput.Text = "(No file selected)";
            this.LblFileOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LblFileOutput.Click += new System.EventHandler(this.LblFileOutput_Click);
            // 
            // CmbFileFormat
            // 
            this.CmbFileFormat.FormattingEnabled = true;
            this.CmbFileFormat.Location = new System.Drawing.Point(413, 102);
            this.CmbFileFormat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbFileFormat.Name = "CmbFileFormat";
            this.CmbFileFormat.Size = new System.Drawing.Size(179, 24);
            this.CmbFileFormat.TabIndex = 5;
            this.CmbFileFormat.Text = "mp3";
            this.CmbFileFormat.SelectedIndexChanged += new System.EventHandler(this.CmbFileFormat_SelectedIndexChanged);
            // 
            // LblFileFormatInput
            // 
            this.LblFileFormatInput.AutoSize = true;
            this.LblFileFormatInput.Location = new System.Drawing.Point(409, 32);
            this.LblFileFormatInput.MinimumSize = new System.Drawing.Size(0, 50);
            this.LblFileFormatInput.Name = "LblFileFormatInput";
            this.LblFileFormatInput.Size = new System.Drawing.Size(108, 50);
            this.LblFileFormatInput.TabIndex = 6;
            this.LblFileFormatInput.Text = "(No file selected)";
            this.LblFileFormatInput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PnlOutput
            // 
            this.PnlOutput.AutoScroll = true;
            this.PnlOutput.BackColor = System.Drawing.Color.White;
            this.PnlOutput.Controls.Add(this.LblConsole);
            this.PnlOutput.Location = new System.Drawing.Point(12, 160);
            this.PnlOutput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnlOutput.Name = "PnlOutput";
            this.PnlOutput.Size = new System.Drawing.Size(580, 209);
            this.PnlOutput.TabIndex = 7;
            // 
            // LblConsole
            // 
            this.LblConsole.AutoSize = true;
            this.LblConsole.Location = new System.Drawing.Point(4, 4);
            this.LblConsole.Name = "LblConsole";
            this.LblConsole.Size = new System.Drawing.Size(90, 16);
            this.LblConsole.TabIndex = 0;
            this.LblConsole.Text = "App initialized";
            // 
            // PrgBarConvert
            // 
            this.PrgBarConvert.Location = new System.Drawing.Point(11, 377);
            this.PrgBarConvert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PrgBarConvert.Name = "PrgBarConvert";
            this.PrgBarConvert.Size = new System.Drawing.Size(479, 26);
            this.PrgBarConvert.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.PrgBarConvert.TabIndex = 8;
            // 
            // BtnConvert
            // 
            this.BtnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnConvert.AutoSize = true;
            this.BtnConvert.Location = new System.Drawing.Point(492, 376);
            this.BtnConvert.Margin = new System.Windows.Forms.Padding(0);
            this.BtnConvert.Name = "BtnConvert";
            this.BtnConvert.Size = new System.Drawing.Size(101, 28);
            this.BtnConvert.TabIndex = 9;
            this.BtnConvert.Text = "Convert";
            this.BtnConvert.UseVisualStyleBackColor = true;
            this.BtnConvert.Click += new System.EventHandler(this.BtnConvert_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(419, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ChkAutoscroll
            // 
            this.ChkAutoscroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkAutoscroll.Checked = true;
            this.ChkAutoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkAutoscroll.Location = new System.Drawing.Point(413, 133);
            this.ChkAutoscroll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChkAutoscroll.Name = "ChkAutoscroll";
            this.ChkAutoscroll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ChkAutoscroll.Size = new System.Drawing.Size(180, 21);
            this.ChkAutoscroll.TabIndex = 11;
            this.ChkAutoscroll.Text = "Auto Scroll";
            this.ChkAutoscroll.UseVisualStyleBackColor = true;
            this.ChkAutoscroll.CheckedChanged += new System.EventHandler(this.ChkAutoscroll_CheckedChanged);
            // 
            // BtnConvertOptions
            // 
            this.BtnConvertOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnConvertOptions.BackgroundImage = global::ConvertApp.Properties.Resources.settingalpha;
            this.BtnConvertOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnConvertOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnConvertOptions.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.BtnConvertOptions.FlatAppearance.BorderSize = 2;
            this.BtnConvertOptions.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.BtnConvertOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConvertOptions.Location = new System.Drawing.Point(377, 102);
            this.BtnConvertOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnConvertOptions.Name = "BtnConvertOptions";
            this.BtnConvertOptions.Size = new System.Drawing.Size(29, 26);
            this.BtnConvertOptions.TabIndex = 12;
            this.BtnConvertOptions.UseVisualStyleBackColor = true;
            this.BtnConvertOptions.Visible = false;
            // 
            // BtnHelp
            // 
            this.BtnHelp.BackgroundImage = global::ConvertApp.Properties.Resources.hamburger;
            this.BtnHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnHelp.FlatAppearance.BorderSize = 0;
            this.BtnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnHelp.Location = new System.Drawing.Point(560, 7);
            this.BtnHelp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnHelp.Name = "BtnHelp";
            this.BtnHelp.Size = new System.Drawing.Size(33, 28);
            this.BtnHelp.TabIndex = 10;
            this.BtnHelp.UseVisualStyleBackColor = true;
            this.BtnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // TotiBalloon
            // 
            this.TotiBalloon.AutoPopDelay = 30000;
            this.TotiBalloon.InitialDelay = 300;
            this.TotiBalloon.ReshowDelay = 100;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 412);
            this.Controls.Add(this.BtnConvertOptions);
            this.Controls.Add(this.ChkAutoscroll);
            this.Controls.Add(this.BtnHelp);
            this.Controls.Add(this.BtnConvert);
            this.Controls.Add(this.PrgBarConvert);
            this.Controls.Add(this.PnlOutput);
            this.Controls.Add(this.LblFileFormatInput);
            this.Controls.Add(this.CmbFileFormat);
            this.Controls.Add(this.LblFileOutput);
            this.Controls.Add(this.LblFileInput);
            this.Controls.Add(this.BtnSelectFileOutput);
            this.Controls.Add(this.BtnSelectFileInput);
            this.Controls.Add(this.lblSelectFiles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmMain";
            this.Text = "Audio/Video File Converter";
            this.PnlOutput.ResumeLayout(false);
            this.PnlOutput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectFiles;
        private System.Windows.Forms.Button BtnSelectFileInput;
        private System.Windows.Forms.Button BtnSelectFileOutput;
        private System.Windows.Forms.Label LblFileInput;
        private System.Windows.Forms.Label LblFileOutput;
        private System.Windows.Forms.ComboBox CmbFileFormat;
        private System.Windows.Forms.Label LblFileFormatInput;
        private System.Windows.Forms.Panel PnlOutput;
        public System.Windows.Forms.Label LblConsole;
        private System.Windows.Forms.ToolTip TotiCommon;
        private System.Windows.Forms.Button BtnConvert;
        private System.Windows.Forms.Button BtnHelp;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ProgressBar PrgBarConvert;
        private System.Windows.Forms.CheckBox ChkAutoscroll;
        private System.Windows.Forms.Button BtnConvertOptions;
        private System.Windows.Forms.ToolTip TotiBalloon;
    }
}

