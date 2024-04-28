namespace ConvertApp
{
    partial class FormatPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormatPicker));
            this.CmbPicker = new System.Windows.Forms.ComboBox();
            this.LblExplanation = new System.Windows.Forms.Label();
            this.BtnAccept = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CmbPicker
            // 
            this.CmbPicker.FormattingEnabled = true;
            this.CmbPicker.Location = new System.Drawing.Point(13, 32);
            this.CmbPicker.Name = "CmbPicker";
            this.CmbPicker.Size = new System.Drawing.Size(250, 21);
            this.CmbPicker.TabIndex = 0;
            this.CmbPicker.Text = "mp3";
            this.CmbPicker.SelectedIndexChanged += new System.EventHandler(this.CmbPicker_SelectedIndexChanged);
            // 
            // LblExplanation
            // 
            this.LblExplanation.Location = new System.Drawing.Point(11, 13);
            this.LblExplanation.Name = "LblExplanation";
            this.LblExplanation.Size = new System.Drawing.Size(251, 16);
            this.LblExplanation.TabIndex = 1;
            this.LblExplanation.Text = "Select a file format to be converted";
            // 
            // BtnAccept
            // 
            this.BtnAccept.Location = new System.Drawing.Point(13, 61);
            this.BtnAccept.Name = "BtnAccept";
            this.BtnAccept.Size = new System.Drawing.Size(75, 23);
            this.BtnAccept.TabIndex = 2;
            this.BtnAccept.Text = "Accept";
            this.BtnAccept.UseVisualStyleBackColor = true;
            // 
            // FormatPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 96);
            this.Controls.Add(this.BtnAccept);
            this.Controls.Add(this.LblExplanation);
            this.Controls.Add(this.CmbPicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormatPicker";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pick a format";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CmbPicker;
        private System.Windows.Forms.Label LblExplanation;
        private System.Windows.Forms.Button BtnAccept;
    }
}