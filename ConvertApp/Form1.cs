using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Forms.Application;

namespace ConvertApp
{
    public partial class FrmMain : Form
    {
        //Define a private class used for the options menu items. It combines the function used for when clicking an item with its corresponding text and more
        class OptionsMenuItem
        {
            public delegate void Func(object sender, EventArgs e);  //Delegate function for the list of functions used for each options item
            private Func func;                                      //The func object itself
            private string text;                                    //Text for the item
            private bool check;                                     //Determines if the item is checked or not.
            private Bitmap icon;                                    //The icon associated with the item.

            public OptionsMenuItem(string text, Func func)
            {
                this.text = text;
                this.func = func;
                this.check = false;
                this.icon = null;
            }
            public OptionsMenuItem(string text, Func func, bool isChecked)
            {
                this.func = func;
                this.text = text;
                this.check = isChecked;
                this.icon = null;
            }
            public OptionsMenuItem(string text, Func func, bool isChecked, Bitmap icon)
            {
                this.func = func;
                this.text = text;
                this.check = isChecked;
                this.icon = icon;
            }

            public string getText()
            {
                return text;
            }
            public Func getFunc()
            {
                return func;
            }
            public bool isChecked()
            {
                return check;
            }
            public Bitmap getIcon()
            {
                return icon;
            }
        }

        //Function used to move tooltip windows.
        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr h, int x, int y, int width, int height, bool redraw);

        //Define some globals
        private static bool ValidConversion = true;
        private static string SelectedFileInputPath = null;
        private static string SelectedFileOutputPath = null;
        private static string SelectedFileLastConversion = null;

        private static ContextMenuStrip optionsMenu;

        //Constructor. This runs when the app starts and when the window itself gets created.
        public FrmMain()
        {
            //Initialize window components and styles
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            WindowState = FormWindowState.Normal;
            MaximizeBox = false;
            StartPosition = FormStartPosition.Manual;



            //Show the format picker the first thing it does if an auto conversion was set and the SystemIntegration isn't set to ignore this. And it's active.
            if (SystemIntegration.Active && SystemIntegration.InstantRun && !SystemIntegration.OverridePicker)
            {
                FormatPicker picker = new FormatPicker();
                DialogResult pickerResult = picker.ShowDialog();
                if (pickerResult == DialogResult.Cancel)
                    Application.Exit();
                //else

            }

            //Get the last location on the screen as set by the app when it exists
            if (Properties.Settings.Default.LastWindowLocation != null)
            {
                Location = Properties.Settings.Default.LastWindowLocation;
                if(Location.X == 0)
                    Location = new Point(30, 50);
            }
            else
            {
                Location = new Point(30, 50);//The default position of the window the first time the app starts
                Properties.Settings.Default.LastWindowLocation = Location;
            }

            //Add a context menu items to the cog icon and intialize it
            SetOptionsMenu();

            //Set auto scroll check box click event handler
            ChkAutoscroll.Click += new EventHandler(ConsoleAutoScrollToggle);

            //Add formats to the combo box.
            for (int i = 0; i < MyFfmpeg.CompatibleFormats.Length; ++i)
            {
                CmbFileFormat.Items.Add(MyFfmpeg.CompatibleFormats[i]);
            }

            //Initialize and set initial text to console
            ConvertConsole.CConsole = new ConvertConsole((Label)LblConsole, ((Panel)PnlOutput));
            if (File.Exists(MyFfmpeg.ffmpegExePath))
            {
                ConvertConsole.CConsole.WriteLineTo(Lang.R.Text.LblConsole);
                ConvertConsole.CConsole.WriteLineTo(Lang.R.Text.ReadyToUse);
            }
            else
                ConvertConsole.CConsole.WriteLineTo("Ffmpeg.exe file missing. You can download it in the settings menu.");

            //Change some fonts
            ConvertConsole.CConsole.SetTextSize();

            //Add tooltips where ever they're needed.
            LblFileInput.MouseHover += new EventHandler(ToolTipListener);
            LblFileFormatInput.MouseHover += new EventHandler(ToolTipListener);
            BtnConvert.MouseHover += new EventHandler(ToolTipListener);
            BtnConvert.MouseLeave += (s, e) =>
            {
                TotiBalloon.Hide((Control)s);
            };

            //Initialize FFMPEG (Used for the audio/video conversions)
            MyFfmpeg.Init(PrgBarConvert, BtnConvert, this);

            //Change text of various controls in the form using the Lang class
            ApplyText();

        }

        //OnLoad override. Does stuff after the constructor.
        protected override void OnLoad(EventArgs e)
        {
            //Check if the ffmpeg exe is present.
            if(!File.Exists(MyFfmpeg.ffmpegExePath))
                MyFfmpeg.PromptFFmpegDownload();

            //Write a dummy character. It unfudges some weird glitchy behavior when blitting out "ready to use"
            ConvertConsole.CConsole.WriteLineTo("-----");

            //Check for app context and tell the user about it.
            if (SystemIntegration.Context != "")
                ConvertConsole.CConsole.WriteLineTo($"App opened from: {SystemIntegration.Context}");

            //If params are present, blit that out in the console and set all the different controls and variables
            if(SystemIntegration.ffmpegParameters != "")
            {
                if (SystemIntegration.Active)
                {
                    //Get the IO from the ffmpeg parameters
                    string[] io = SystemIntegration.GetIOFromFFMpegParam(SystemIntegration.ffmpegParameters, FormatPicker.FormatPicked);

                    //Blit out params on console
                    ConvertConsole.CConsole.WriteLineTo($"Params: {SystemIntegration.ffmpegParameters}");
                    ConvertConsole.CConsole.WriteLineTo($"IO: {io[0]} - {io[1]}");

                    //Set all the related views and values
                    SetConversion(io[0], io[1]);

                    //Run conversion if instant run was specified in the SystemIntegration class. The button handler does it.. Probably shouldn't
                    if (SystemIntegration.InstantRun)
                    {
                        //Check if format is compatible. Otherwise, message the error and auto close if it's set to do so.
                        if (Common.CheckCompatibleFormats(io[1], MyFfmpeg.CompatibleFormats))
                            BtnConvert_Click(null, null);
                        else
                        {
                            MessageBox.Show(Lang.Format(Lang.R.Text.InputFileSelectedNotSupported, io[1]));
                            if (!SystemIntegration.OverrideClose && SystemIntegration.InstantRun)
                                Application.Exit();
                        }
                    }
                }
            }
            //Show a warning if parameters were found but were skipped due to Active being false. This gets set in the SystemIntegration.ParseArgs function
            else if (SystemIntegration.SkippedParams && !SystemIntegration.Active)
            {
                ConvertConsole.CConsole.WriteLineTo($"WARNING: App parameters are deactivated. Set 'Active' in SystemIntegration to true.");
            }

            //Standard onload from inherited class
            base.OnLoad(e);
        }

        //Override OnPaint for the form to do some custom "paint" actions on the window's "client" area.
        protected override void OnPaint(PaintEventArgs e)
        {
            //The inherited paint action. For normal paint stuff.
            base.OnPaint(e);

            //Draw a border on the "console background" panel.
            Rectangle pos = PnlOutput.ClientRectangle;
            pos.Offset(new Point(PnlOutput.Left-1, PnlOutput.Top-1));
            pos.Height = pos.Height+2;
            pos.Width = pos.Width+2;
            ControlPaint.DrawBorder(e.Graphics, pos,
                                      Color.DarkGray, 1, ButtonBorderStyle.Solid,
                                      Color.DarkGray, 1, ButtonBorderStyle.Solid,
                                      Color.DarkGray, 1, ButtonBorderStyle.Solid,
                                      Color.DarkGray, 1, ButtonBorderStyle.Solid);
        }

        //Override of when the form closes (The x button is pressed, for instance)
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //Save app window location on the screen
            Properties.Settings.Default.LastWindowLocation = Location;
            Properties.Settings.Default.Save();

            //Important to properly terminate the ffmpeg process.
            //Otherwise, it can live on forever in the background even when this app has been terminated.
            MyFfmpeg.Terminate();
            base.OnFormClosing(e);
        }

        //Event handler for when you click the "File Input" button.
        private void BtnSelectFileFrom_Click(object sender, EventArgs e)
        {
            //When the File Input button is clicked
            //Opens dialog box, sets the selected file path to the label and cuts the label into just the file name
            string output = Common.OpenFileDialog();

            //Check if what was picked by the user is a compatible file. Otherwise, give the user an error message.
            if (Common.CheckCompatibleFormats(output, MyFfmpeg.CompatibleFormats))
            {
                LblFileInput.Text = Common.CutPath(output);
                SelectedFileInputPath = output;
                ValidConversion = WriteToLblFileFormatInput();

                ConvertConsole.CConsole.WriteLineTo(Lang.Format(Lang.R.Text.InputFileSelected, output));
            }
            else
            {
                if (output != "")
                    MessageBox.Show(Lang.Format(Lang.R.Text.InputFileSelectedNotSupported, output));
            }
        }

        /// <summary>
        /// A generic listener for tool tip hover (When you hover your mouse and a text shows up)
        /// Is applies to a few controls in the form.
        /// </summary>
        /// <param name="sender">EventHandler variable. Contains the object that this event was triggered on</param>
        /// <param name="e">EventHandler variable. Carries event data (Mouse pos, etc)</param>
        /// <remarks>May be a bit overdesigned. Lambda expressions for hover event listeners would work just as well</remarks>
        private void ToolTipListener(object sender, EventArgs e)
        {
            //Initialize and set text to show in the tool tip.
            string TTText = "";

            //Go through all the different names of the control that's being hovered over.
            switch (((Control)sender).Name)
            {
                case "LblFileInput":
                    TTText = SelectedFileInputPath;
                    break;
                case "LblFileFormatInput":
                    TTText = Lang.R.Text.ConversionTakePlaceToolTip;
                    break;
                case "BtnConvert":

                    //TODO This should probably be put in its own function. Or something.
                    //Set the message to its default
                    string performParams = Lang.R.Text.NoInOutSelected;

                    //check if the path is empty or not
                    if (SelectedFileOutputPath != "" && SelectedFileOutputPath != null)
                    {
                        string fileToConvert = SelectedFileOutputPath + @"\" + MakeOutputFileName(CmbFileFormat.Text);
                        performParams = MyFfmpeg.GetFFmpegArguments(SelectedFileInputPath, fileToConvert);
                        performParams = MyFfmpegOptions.ExplainParameters(performParams);
                    }

                    //Blit its own special balloon tool tip and return it.
                    TTText = $"{Lang.R.Text.WillPerform}:\n{performParams}";
                    Size textSize = TextRenderer.MeasureText(TTText, Font);

                    int padding = 10;
                    int ttWidth = (textSize.Width + padding*2) + 30;
                    int ttHeight = textSize.Height + padding*2;

                    TotiBalloon.Popup += (ss, ee) =>
                    {
                        ee.ToolTipSize = new Size(ttWidth, ttHeight);
                        //ee.ToolTipBounds = new Rectangle(0,0, 200, 60);
                    };
                    TotiBalloon.Draw += (ss, ee) =>
                    {
                        MyPaint formatter = new MyPaint();
                        List<MyPaint.Segment> segments = formatter.FormatTooltip(TTText);//

                        float x = padding;
                        float y = padding;
                        float lineHeight = ee.Graphics.MeasureString("A", Font).Height;

                        ee.DrawBackground();
                        ee.DrawBorder();
                        //SizeF textSize = ee.Graphics.MeasureString(ee.ToolTipText, Font);
                        
                        Brush bgBrush = new SolidBrush(Color.White);
                        ee.Graphics.FillRectangle(bgBrush, new Rectangle(1, 1, (ttWidth - 2), (ttHeight - 2)));
                        //ee.Graphics.DrawString(((ToolTip)ss).GetToolTip((Control)ss), Font, Brushes.Red, 10, 1);
                        /*ee.Graphics.DrawString(ee.ToolTipText, Font, Brushes.Black, 0 + padding, 0 + padding);
                        */

                        foreach (MyPaint.Segment segment in segments)
                        {
                            using (Brush brush = new SolidBrush(segment.Color))
                            {
                                //if (x + textSize.Width > Width - 10)
                                //{
                                //x = 10; // Start a new line
                                //y += lineHeight;
                                //}
                                if (segment.newLine) x = 10;
                                if(segment.newLine) y += lineHeight - 1;
                                ee.Graphics.DrawString(segment.Text, Font, brush, x, y);
                                

                                // Update x position for the next segment
                                x += ee.Graphics.MeasureString(segment.Text, Font).Width;
                            }

                            
                        }
                    };
                    TotiBalloon.OwnerDraw = true;
                    TotiBalloon.Show(TTText, (Control)sender, -(ttWidth - ((Control)sender).Width), -ttHeight, 30000);

                    return;
            }

            //Blit out the actual tool tip.
            TotiCommon.SetToolTip((Control)sender, TTText);
        }

        //Output file selector event handler. When clicking the "Save location" button.
        private void BtnSelectFileOutput_Click(object sender, EventArgs e)
        {
            //Check that the input isn't empty (The video or audio to be converted into something else. Selected using the "File Input" button.
            if(SelectedFileInputPath != "" && SelectedFileInputPath != null)
            {
                //Open the dialog that let's the user pick.
                string output = Common.OpenFileDialogCreateOutput();

                //Check if something was picked and assign it the output it found if it was.
                if (output != "" && output != null)
                {
                    SelectedFileOutputPath = output;
                    LblFileOutput.Text = output;

                    ConvertConsole.CConsole.WriteLineTo(Lang.Format(Lang.R.Text.OutputFolderChanged, output));
                }
            }
            else
            {
                MessageBox.Show(Lang.R.Text.PleaseSelectInputFile);
            }
        }

        /// <summary>
        /// Simply writes to the label that has the conversion text in it. What formats will be converted.
        /// </summary>
        /// <returns>A bool indicating if the current conversion is valid or not</returns>
        private bool WriteToLblFileFormatInput()
        {
            bool isValid = true;
            //Only bother writing anything if the selected input (From the file picker dialog by the user) isn't empty or null
            if (SelectedFileInputPath != null && SelectedFileInputPath != "")
            {
                //Get the "from" and "to" file extensions to be drawn at the right side of the buttons that picked them. It shows the conversion.
                string fFrom = Common.SelectFileType(SelectedFileInputPath);    //Get the file type (mp3, mp4, etc) from the selected input
                string fTo = CmbFileFormat.Text;                                //The combo box is just file types, so simply use the value in it
                string fExtra = "";                                             //The extra string to put at the end of the final output.

                //If the two file formats are the same, mark the end of the conversion indicator as "invalid" (It won't work)
                if (fFrom == fTo)
                {
                    fExtra = Lang.R.Text.Invalid;
                    isValid = false;
                }

                //Write the text to the label
                LblFileFormatInput.Text = $"{fFrom} => {fTo} {fExtra}";

            }

            return isValid;
        }

        //Event Handler for when the file format combo box changes its value
        private void CmbFileFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Writes to the label that shows what conversion will take place.
            ValidConversion = WriteToLblFileFormatInput();
        }

        //Makes the output file name
        public static string MakeOutputFileName(string fileFormat)
        {
            //Make the output file name
            string[] parts = SelectedFileInputPath.Split('\\');
            string fn = parts[parts.Length - 1];
            fn = fn.Split('.')[0];
            fn += "." + fileFormat;

            return fn;
        }

        //Event handler for the convert button
        private void BtnConvert_Click(object sender, EventArgs e)
        {
            //Start conversion.
            if (SelectedFileInputPath != null && SelectedFileInputPath != "" &&
                SelectedFileOutputPath != null && SelectedFileOutputPath != "" &&
                ValidConversion &&
                !MyFfmpeg.ThreadRunning)
            {
                //Make the output file name
                /*string[] parts = SelectedFileInputPath.Split('\\');
                string fn = parts[parts.Length - 1];
                fn = fn.Split('.')[0];
                fn += "." + CmbFileFormat.Text;*/
                string fn = MakeOutputFileName(CmbFileFormat.Text);

                //Put the final result of the entire path and conversion type into a variable to be re-used. If it doesn't contain a dot, assume it's just the path and not the same, and add the name
                if(!SelectedFileOutputPath.Contains("."))       //Add the file name. Normal conversion
                    SelectedFileLastConversion = SelectedFileOutputPath + @"\" + fn;
                else if(SelectedFileOutputPath.Contains("."))   //Don't add a file name. Conversion made using exe parameters
                    SelectedFileLastConversion = SelectedFileOutputPath;

                //Write a console message. Do some processing for the console too.
                string insert = Lang.FormatReplace(Lang.R.Text.ConvertingTo, " ", "\n", SelectedFileInputPath, SelectedFileOutputPath + @"\" + fn);
                string msg = $"------\n" + insert + "\n------";
                ConvertConsole.CConsole.WriteLineTo(msg);

                //Run ffmpeg with select input and output
                MyFfmpeg.Run(SelectedFileInputPath, SelectedFileLastConversion);
            }
            else
            {
                //Write errors
                string errorMsg = "";

                if (MyFfmpeg.ThreadRunning)
                {
                    StopConversion(sender, e);
                    return;
                    //errorMsg += "A conversion is already running.\n\n";
                }
                if (!ValidConversion)
                    errorMsg += Lang.R.Text.CantConvertTwoEqual + "\n\n";
                if(SelectedFileInputPath == null || SelectedFileInputPath == "")
                    errorMsg += Lang.R.Text.InputFileInvalid + "\n\n";
                if (SelectedFileOutputPath == null || SelectedFileOutputPath == "")
                    errorMsg += Lang.R.Text.OutputFolderInvalid + "\n\n";

                MessageBox.Show(errorMsg, Lang.R.Text.Error);
            }
        }

        //Event Handler for the hamburger button at the top right corner. TODO Change name.
        private void BtnHelp_Click(object sender, EventArgs e)
        {
            //Calculate where the options menu should appear.
            Point oploc = ((Button)sender).Location;        //Start in the location of the button
            oploc.X -= optionsMenu.Width;

            optionsMenu.Show(this, oploc);
        }

        //Event handler for the auto scroll check box
        private void ChkAutoscroll_CheckedChanged(object sender, EventArgs e)
        {
            //ConsoleAutoScrollToggle(sender, e);
            //Moved to constructor and made into a click listener/event handler
        }

        //Event handler for the file output dialog.
        private void LblFileOutput_Click(object sender, EventArgs e)
        {
            //What object to refer to. Might be kind of dumb, but here it is anyway.
            if(sender.GetType() == typeof(Label))
                Common.OpenFolder(((Label)sender).Text);
            else
                Common.OpenFolder(LblFileOutput.Text);
        }
        
        //Shows the credits message box
        private void ShowCredits(object sender, EventArgs e)
        {
            MessageBox.Show(Lang.R.Text.CreditsText.Replace(@"\n", "\n"), Lang.R.Text.About);
        }

        //Exits the app
        public void ExitApp(object sender, EventArgs e)
        {
            //MyFfmpeg.Terminate();
            Close();
        }

        //Clears the console
        private void ClearConsole(object sender, EventArgs e)
        {
            //Runs the clear function
            ConvertConsole.CConsole.Clear();
            //LblConsole.Text = "";//Does this due to a strange bug that randomly happens where it claims that paramter is not valid inside the Clear() function
            //LblConsole.Invalidate();
        }
        
        //Toggles auto scrolling and refreshes/redraws the options menu to check/uncheck the item's checkbox
        private void ConsoleAutoScrollToggle(object sender, EventArgs e)
        {
            //Change the auto scroll params in both the console and the options menu
            ConvertConsole.CConsole.ToggleAutoScroll();
            SetOptionsMenu();

            //Checks or unchecks the auto scroll check box
            //if(sender.GetType() != typeof(CheckBox))
            ChkAutoscroll.Checked = ConvertConsole.autoScroll;
        }

        //Stops conversion
        private void StopConversion(object sender, EventArgs e)
        {
            if (MyFfmpeg.ThreadRunning)
            {
                MyFfmpeg.Terminate();

                PrgBarConvert.Value = 0;
                BtnConvert.Text = Lang.R.Text.BtnConvert;
                ConvertConsole.CConsole.WriteLineTo(Lang.R.Text.ConversionHasStopped);
            }
            else
            {
                ConvertConsole.CConsole.WriteLineTo(Lang.R.Text.NoConversionInProg);
            }
        }

        //Plays the latest conversion that was done. A nice way to check it out.
        private void PlayLastConversion(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(SelectedFileLastConversion);
            }
            catch(Exception err)
            {
                
                //Exception nerr = new Exception(err.Message);
                ConvertConsole.CConsole.WriteLineTo(Lang.R.Text.FailedToPlayLastCon.Replace(@"\n", "\n"));
                MessageBox.Show(err.Message, Lang.R.Text.Error);
            }
        }

        //Opens the options menu (The form containing the options) that's bound to its parent.
        private void OpenOptionsMenu(object sender, EventArgs e)
        {
            FormOptions options = new FormOptions();
            options.ShowDialog();
        }

        //Sets the options menu (That opens up when pressing the hamburger icon to the top right)
        //This function gets called whenever something inside of it has to change too.
        private void SetOptionsMenu()
        {
            //Instantiate the menu and set some initial values
            optionsMenu = new ContextMenuStrip();
            optionsMenu.AutoSize = true;
            optionsMenu.BackColor = Color.White;

            //Initialize the menu item text and the functions for them.
            OptionsMenuItem[] items =
            {
                new OptionsMenuItem((string)Lang.R.Text.Convert,                BtnConvert_Click, false, Properties.Resources.converting),
                new OptionsMenuItem((string)Lang.R.Text.ClearConsole,           ClearConsole),
                new OptionsMenuItem((string)Lang.R.Text.AutoScroll,             ConsoleAutoScrollToggle, ConvertConsole.autoScroll),
                new OptionsMenuItem((string)Lang.R.Text.PlayLastConversion,     PlayLastConversion),
                new OptionsMenuItem((string)Lang.R.Text.StopConversion,         StopConversion),
                new OptionsMenuItem((string)Lang.R.Text.Options,                OpenOptionsMenu, false, Properties.Resources.setting),
                new OptionsMenuItem((string)Lang.R.Text.About,                  ShowCredits),
                new OptionsMenuItem((string)Lang.R.Text.Exit,                   ExitApp),
            };

            //Add items to the list.
            for (int i = 0; i < items.Length; ++i)
            {
                ToolStripMenuItem mi = new ToolStripMenuItem(items[i].getText());
                //mi.Click += (object sender, EventArgs e) => { MessageBox.Show("MESSAGE!!" + i); };
                mi.Checked = items[i].isChecked();
                mi.Click += items[i].getFunc().Invoke;
                if (items[i].getIcon() != null)
                    mi.Image = items[i].getIcon();
                optionsMenu.Items.Add(mi);
            }
        }

        /// <summary>
        /// Sets a conversion instantly by setting all the relevant variables and controls
        /// </summary>
        /// <param name="In">In file</param>
        /// <param name="Out">Out file</param>
        private void SetConversion(string In, string Out)
        {
            SelectedFileOutputPath = Out;
            SelectedFileInputPath = In;

            if(Out != null && Out != "")
                CmbFileFormat.Text = Out.Split('.')[Out.Split('.').Length - 1];

            LblFileInput.Text = In;
            LblFileOutput.Text = Out;

            string fileToConvert = SelectedFileOutputPath;

            SelectedFileLastConversion = fileToConvert;
            //fileToConvert

            ValidConversion = WriteToLblFileFormatInput();
            //string fileToConvert = SelectedFileOutputPath + @"\" + MakeOutputFileName(CmbFileFormat.Text);
            //performParams = MyFfmpeg.GetFFmpegArguments(SelectedFileInputPath, fileToConvert);
            //SelectedFileOutputPath = output;
            //LblFileFormatInput
            //ValidConversion = WriteToLblFileFormatInput();
        }

        /// <summary>
        /// Applies text from the Lang class using the Lang.R variable.
        /// Applies it controls that aren't given anything aside from initially in the design window/document.
        /// Initial values, you might think it as
        /// </summary>
        /// <remarks>Could probably have been made a bit more elegant. This is kind of a dumb approach. Should iterate through all the controls and set them based on the names and.. Well, it's little enough to where this is good enough.</remarks>
        private void ApplyText()
        {
            Text = Lang.R.Text.AppTitle;
            LblFileInput.Text = Lang.R.Text.NoFileSelected;
            LblFileOutput.Text = Lang.R.Text.NoFileSelected;
            LblFileFormatInput.Text = Lang.R.Text.NoFileSelected;
            BtnSelectFileInput.Text = Lang.R.Text.BtnSelectFileInput;
            BtnSelectFileOutput.Text = Lang.R.Text.BtnSelectFileOutput;
            BtnConvert.Text = Lang.R.Text.BtnConvert;
            LblConsole.Text = Lang.R.Text.LblConsole;
            lblSelectFiles.Text = Lang.R.Text.lblSelectFiles;
            ChkAutoscroll.Text = Lang.R.Text.ChkAutoscroll;
        }
    }
}
