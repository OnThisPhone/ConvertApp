using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace ConvertApp
{
    public partial class FormOptions : Form
    {
        private static bool languageWasChanged = false;     //This gets set to true if the combo box for changing language is changed.
        private static bool check = false;                  //Makes so that respective event handlers won't trigger when you programmatically change a control value. I'll admit. This is a bit.. cooky.
                                                            //Avoids recursion.

        //Constructor. Opening the form runs this
        public FormOptions()
        {
            //Some initialization of the window/form itself.
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedToolWindow;

            //Do check
            check = true;

            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = Lang.R.Text.Options;

            //Assign the setting associated with which context menu is selected. Sync it with if it exists or not
            //Note that this doesn't set the number, but assumes true means 2 (APP).
            Properties.Settings.Default.intigration = RegEdit.Exists() ? 2 : 0;

            //Change tab backgrounds and style
            ApplyTabStyling();

            //Populate the languages in the languages combo box.
            foreach (string language in Lang.languages)
                CmbLanguage.Items.Add(language);

            //Populate console text sizes
            string[] consoleTextSizes = { "Small", "Medium", "Big" };
            foreach(string consoleTextSize in consoleTextSizes)
            {
                CmbConsoleTextSize.Items.Add(consoleTextSize);
            }

            //Set default for combo box or current value if it's set.
            if (Properties.Settings.Default.Language != "")
            {
                CmbLanguage.Text = Properties.Settings.Default.Language;
            }
            else
            {
                Properties.Settings.Default.Language = CmbLanguage.Text;
                Properties.Settings.Default.Save();
            }

            //Set text size combo box.
            if(Properties.Settings.Default.ConsoleTextSize != "")
            {
                CmbConsoleTextSize.Text = Properties.Settings.Default.ConsoleTextSize;
            }
            else
            {
                Properties.Settings.Default.ConsoleTextSize = CmbConsoleTextSize.Text;
                Properties.Settings.Default.Save();
            }

            //Put in the "machine translated" label if the current language is machine translated.
            if (Lang.CheckLanguageMachineTranslated())
                LblMachineTranslated.Text = $"{Lang.currentLanguage}{Lang.R.Text.LblMachineTranslated}";
            else
                LblMachineTranslated.Text = "";

            //Get the version of the FFMPEG exe file (Might want to use this at some point)
            /*string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg", "ffmpeg.exe");
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(ffmpegPath);
            LblVersionName.Text += versionInfo.ProductName;*/

            //Apply text to the different controls in the form
            ApplyText();

            //Set App version in the corner
            LblVersionName.Text = Common.GetAppVersion(Common.APP_VER_VERBOSITY.FULL);

            //Set the language changed to false.
            languageWasChanged = false;

            //Sets various different properties of "convert tab" specific controls.
            ConvertOptionsInit();

            //Do some Downloadffmpeg button stuff. Add click to download ffmpeg button
            BtnDownloadFfmpegExe.Click += (o, e) =>
            {
                MyFfmpeg.DownloadFFmpeg();
            };

            //Show or hide the download ffmpeg exe button if it's not in the ffmpeg/ directory
            if (File.Exists(MyFfmpeg.ffmpegExePath))
                BtnDownloadFfmpegExe.Visible = false;

            //Set the current Properties.Settings.Default.intigration
            IntegrationInit();

            //Set the fmpeg version label
            //LblFfmpegVersion.Text = $"Ffmpeg Version: {MyFfmpeg.GetExeVersion()}";

            //Hide the privilege shield icon if the user is an admin
            if(Common.IsAdmin())
                PctAdmin.Visible = false;

            //Add tooltip for the shield icon.
            PctAdmin.MouseHover += new EventHandler((o, e) =>
            {
                TooltipOptions.SetToolTip((Control)o, "This action requires admin privileges.");
            });

            //Set framerate and bitrate text boxes
            TxtBitrate.Text = MyFfmpegOptions.bitrate;
            if(MyFfmpegOptions.framerate != -1)
                TxtFramerate.Text = "" + MyFfmpegOptions.framerate;

            //Do check
            check = false;
        }

        //Override of when the form closes (The x button is pressed, for instance)
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //Change the size of the console text upon exiting.
            ConvertConsole.CConsole.SetTextSize();

            //Save the custom parameters in the options class.
            MyFfmpegOptions.SetCustomParametersWithCheck(TxtCustomParameters.Text);

            //Check bitrate and framerate values and make sure that they're valid. Bitrate requires "k" in the end and framerate has to be a number.
            //Also, apply them if they're valid.
            //Only do this check if bitrate or framerate is not empty
            if (TxtBitrate.Text != "" || TxtFramerate.Text != "")
            {
                //Set some variables
                int frameRate;
                bool checkResult = false;

                //Try the bitrate from the text and return the invalid -1 if it failed (The text wasn't the right format)
                try { frameRate = int.Parse(TxtFramerate.Text); } catch { frameRate = -1; }

                //Check validity only if framerate was parsed correctly or if framerate is empty.
                if (frameRate != -1 || TxtFramerate.Text == "")
                    checkResult = MyFfmpegOptions.CheckFramerateBitrateValidity(frameRate, TxtBitrate.Text);
                else
                    MessageBox.Show("Bitrate or framerate contains invalid characters", Lang.R.Text.Error);

                //If the result of the check was false, cancel closing of the form. Else. Apply the new values to the settings
                if (!checkResult)
                    e.Cancel = true;
                else
                {
                    //Set bit-/framerate
                    MyFfmpegOptions.SetBitrate(TxtBitrate.Text);
                    if(TxtFramerate.Text != "")
                        MyFfmpegOptions.SetFramerate(int.Parse(TxtFramerate.Text));
                    else
                        MyFfmpegOptions.SetFramerate(-1);
                }
            }
            else
            {
                //If both are empty, reset both
                MyFfmpegOptions.SetBitrate("");
                MyFfmpegOptions.SetFramerate(-1);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LblVersionName_Click(object sender, EventArgs e)
        {

        }

        //Event handler for changing the language selector combo box.
        private void CmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            languageWasChanged = true;
            Properties.Settings.Default.Language = ((ComboBox)sender).Text;
            Properties.Settings.Default.Save();
        }

        //Event handler for changing the console text size combo box value
        private void CmbConsoleTextSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConsoleTextSize = ((ComboBox)sender).Text;
            Properties.Settings.Default.Save();
        }

        //Event handler for clicking the accept button
        private void BtnAccept_Click(object sender, EventArgs e)
        {
            //Restarts the app to apply settings (Will stop conversion). But only if the language was changed.
            if (languageWasChanged)
            {
                //Reset whole application
                Process.Start(Application.ExecutablePath);
                Program.mainForm.Close();
            }
            else
            {
                //Closes current window
                Close();
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Applies text to the controls of this form from the Lang class.
        /// </summary>
        private void ApplyText()
        {
            LblSelectLanguage.Text = Lang.R.Text.LblSelectLanguage;
            LblConsoleTextSize.Text = Lang.R.Text.LblConsoleTextSize;
            BtnAccept.Text = Lang.R.Text.BtnAccept;
        }

        /// <summary>
        /// Applies some styling to the tab group. Mostly translates them. (NOTE Name of function should probably be reconsidered)
        /// </summary>
        private void ApplyTabStyling()
        {
            //TabCtrlOptions.BackColor = Color.Black;
            TabCtrlOptions.TabPages[0].Text = Lang.R.Text.General;
            TabCtrlOptions.TabPages[1].Text = Lang.R.Text.BtnConvert;
            LblCustomParams.Text = Lang.R.Text.CustomParameters;
            ChkBoxEnabled.Text = Lang.R.Text.Enabled;
            LblTutorial.Text = Lang.R.Text.Explanation;
            BtnResetCustomParams.Text= Lang.R.Text.Default;
            LblRotation.Text = Lang.R.Text.Rotation;

        }

        /// <summary>
        /// Init controls for the convert tab
        /// </summary>
        private void ConvertOptionsInit()
        {
            //Add a click event handler for when the user clicks the link label "Explain"
            LblTutorial.Click += new EventHandler((object o, EventArgs e) =>
            {
                Process.Start(MyFfmpegOptions.ffmpegTutorialLink);
            });

            //Add event handler for when the checkbox changes.
            ChkBoxEnabled.CheckedChanged += new EventHandler((object o, EventArgs e) =>
            {
                bool chk = ((CheckBox)o).Checked;
                MyFfmpegOptions.SetCustomParametersActive(chk);
                TxtCustomParameters.Enabled = chk;
            });

            //Add content to the rotation combo box.
            int[] rotations = { 0, 90, 180, 270 };
            foreach(int rotation in rotations)
                CmbRotation.Items.Add(rotation);

            //Add event handler for the rotation combo box
            CmbRotation.SelectedIndexChanged += new EventHandler((object o, EventArgs e) =>
            {
                int rotation = int.Parse(((ComboBox)o).Text);
                MyFfmpegOptions.SetRotation(rotation);
            });

            //Set default value of rotation combo box
            CmbRotation.Text = "" + MyFfmpegOptions.rotation;

            //Set values of controls
            //Set current custom parameters active value in settings
            ChkBoxEnabled.Checked = MyFfmpegOptions.customParametersActive;
            
            //Add a tooltip to custom parameters
            TxtCustomParameters.MouseHover += new EventHandler((object o, EventArgs e) =>
            { 
                TooltipOptions.SetToolTip((Control)o, "{0} means the input file and {1} means the output file.\nCheck the explanation link for info on how to write these.\n\nApp only has support for two files for conversions.\nUse 'backslash quotationmark' to get the quotationmarks to show up normally in the parameter upon converting.\n\nNormally, if this option is specified, it won't use the other conversion options.");
            });

            //Add function to default button
            BtnResetCustomParams.Click += new EventHandler((object o, EventArgs e) =>
            {
                TxtCustomParameters.Text = MyFfmpegOptions.defaultCustomParameters;
            });
            
            //Set the value of current custom parameters from settings
            TxtCustomParameters.Text = MyFfmpegOptions.customParameters;

        }

        //Run when the App radio button is changed to.
        private void RdioApp_CheckedChanged(object sender, EventArgs e)
        {
            //Disables the event handler if check is true
            if (check)
                return;

            //Do the reg edit action. Changes the integration to set value
            ChangeRegEdit(sender, 2);
        }

        //Run when the None radio button is changed to
        private void RdioNone_CheckedChanged(object sender, EventArgs e)
        {
            //Disables the event handler if check is true
            if (check)
                return;

            //Do the reg edit action. Changes the integration to set value
            ChangeRegEdit(sender, 0);
        }

        /// <summary>
        /// Does the relatively complex change to the regedit.
        /// Changes the intigration that lets the user convert directly from windows explorer.
        /// Asks for admin privs if the user doesn't have it.
        /// </summary>
        /// <param name="sender">The radio button object reference</param>
        /// <param name="type">The type of context menu it will change to. 2 for app, 1 for console and 0 for none/remove</param>
        private void ChangeRegEdit(object sender, int type)
        {
            //Set an easy to get reference variable to the control
            RadioButton r = (RadioButton)sender;

            //Only run this if it got checked (To stop all of them to run whenever they get unchecked.
            if (r.Checked)
            {
                //Start the process with prompts and stuff. If it returns false, reset integration controls.
                if(!RegEdit.PromptChange(type))
                    ResetIntegrationControls();
            }
        }

        //Resets the controls. Since the whole check this was made, it does that to avoid recursion.
        public void ResetIntegrationControls()
        {
            check = true;
            IntegrationInit();
            check = false;
        }

        //Initializes which radio box that's currently set to true in the settings and change the appropriate controls
        private void IntegrationInit()
        {
            //Reset all of them
            RdioNone.Checked = false;
            RdioApp.Checked = false;

            //Do the checking..
            switch (Properties.Settings.Default.intigration)
            {
                case 0:
                    RdioNone.Checked = true;
                    break;
                case 2:
                    RdioApp.Checked = true;
                    break;

            }
        }
    }
}
