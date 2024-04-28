using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/***
 * Used to integrate with the system itself
 * It can use exe parameters and change the context menu in explorer to let you convert with a menu item from there.
 * Contains a "context" variable that could be useful to know how the app was opened.
 *      Maybe even a context called "debug"
 *      
 * NOTES ON REGEDIT/SHELL INTEGRATION
 * HKEY_CLASSES_ROOT -> * -> shell -> (Menu item name. Folder/key) -> command (Make key/folder)
 *      The (Default) value in the command folder of the menu item you made (The folder) should be the path of the exe.
 *      The icon should be in the folder that represents your menu item. And as a string called "icon"
 *      
 *      ...
            shell
                 Convert (string icon = path to icon)
                        command ((Default) = path to exe)
 */
namespace ConvertApp
{
    internal class SystemIntegration
    {
        //Declare some globals and what not.
        /***
         *  -ir         = InstantRun will be set to true or false. 
         *  -ot         = Opening type (Can be 0 or 1; App and Console).
         *  -ffmpeg     = Means it will start reading ffmpeg parameters.
         *  -ffmpegend  = Means it will stop reading ffmpeg parameters.
         *  -context    = Specifies the context with which the app was opened. Could be "explorer" if it's something that's integrated into windows explorer.
         *  -oc         = Override close. Set this to true if you want auto convert at startup but not auto close
         *  -op         = Override picker. Set this to true if you don't want to show the format picker at startup.
         *  -settingConv= Sets the context menu integration setting to either app, console or none. 2 app, 1 console, 0 none. This parameter is set by the FormOptions which calls the Common function RestartAppWithAdminPriv
         *                
         */
        public static readonly string[] logicalParameters = //Logical parameters to check for in the param/args list. Used for the args in the Main function.
            {"-ir", "-ot", "-ffmpeg", "-ffmpegend", "-context", "-oc", "-op", "-settingConv"};

        public static bool InstantRun = false;              //If it's true, it will auto run a conversion at startup and auto close the app once it's done. (Only applies to if the app is opened. Not the ffmpeg console)
        public static OPEN_TYPE OpenType = OPEN_TYPE.APP;   //How the app will open an instant run conversion. Let's you use either the app's window or the ffmpeg console window.
        public static string Context = "";                  //Context that was used to enter the app.
        public static string ffmpegParameters = "";         //The ffmpeg parameters that can be set by this class. The ParseArgs function sets it
        public static bool OverrideClose = false;           //Override Close. If it's true, the app won't auto close when the auto conversion is complete
        public static bool OverridePicker = false;          //Set this to true if you don't want to show the format picker at startup.

        public static bool Active = true;                   //Mostly for debugging. None of this functionality will run if it's set to false. Keep in mind that changing the windows explorer context menu from the settings can break if this is set to false.
        public static bool SkippedParams = false;           //Used in leui of being able to check if parameters were set but the active variable was set to false.
                                                            //This gets used in the Form1 OnLoad function and maybe elsewhere too.

        //Enumerator
        //NOTE There's a code in FormOptions Properties.Settings.Default.intigration = RegEdit.Exists() ? 2 : 0;
        //     It treats the logic as though there's only two options. So if the "console" option ever gets added, make sure to change this too.
        public enum OPEN_TYPE
        {
            APP,
            CONSOLE
        }

        /// <summary>
        /// (DUMB FUNCTION. USE WITH CAUTION)
        /// Gets both in and out paramters from an FFMPEG param string.
        /// It's kind of dumb.. It treats the first set of " it finds as the In and the second set as out.
        /// So if that causes problems, that's why.
        /// </summary>
        /// <param name="ffmpegParamString">The ffmpeg parameter string</param>
        /// <param name="FormatPicked">Typically gets set by the FormatPicker.FormatPicked variable</param>
        /// <returns>An array. 0 is in and 1 is out</returns>
        public static string[] GetIOFromFFMpegParam(string ffmpegParamString, string FormatPicked)
        {
            //In and out
            string In = "";
            string Out = "";
            int reading = -1;

            //Step through the ffmpeg param string bit by bit. A bit ugly, but i think that's in-keeping with the function
            for(int i = 0; i < ffmpegParamString.Length; ++i)
            {
                char cur = ffmpegParamString[i];
                if (cur == '\"')
                    ++reading;

                if(cur != '\"' && reading == 0)
                    In += cur;
                if (cur != '\"' && reading == 2)
                    Out += cur;
            }

            //Check if out is empty (Wasn't specified. Default behavior) and make it the "to be converted" file type of the input
            if (Out == "")
            {
                //Check for last instance of . in the string and assume that's the format. Then make the output the same as input but without the format part.
                int index = In.LastIndexOf('.');
                Out = In.Substring(0, index);

                //Add the format that was picked with the FormatPicked variable
                if (FormatPicked != "" && FormatPicked != null)
                    Out += "." + FormatPicked;
                else
                    Out += "." + MyFfmpeg.CompatibleFormats[0];//Set mp3 as the default format.
            }

            //Check if out is empty (Wasn't specified. Default behavior) and add the In path (Excluding file) to the output
            /*if(Out == "")
            {
                //It treats \\ in the string as one character. Because it is. It counts from that character to the start of the string
                int index = In.LastIndexOf('\\');

                //Add the path itself to the output. Yay!
                Out = In.Substring(0, index + 1);
            }*/

            //Add output if only an input was specified.
            if (Common.CountCharsUsingIndex(ffmpegParameters, '\"') >= 2)
                ffmpegParameters = AddOutputParam(ffmpegParameters, Out);

            //Return with the result
            string[] result = { In, Out };
            return result;
        }

        /// <summary>
        /// Parses the args for the exe file and sets variables accordingly.
        /// Should be placed after language has been initialized (Since it uses it)\n
        /// See the SystemIntegration class for an explanation on the types of custom parameters there are.
        /// 
        /// Param (Param being things like -context) and value being something it could be assigned to (Like "explorer")
        /// It will use the last args index to assign a value for a param as the one it assigns it to.. If that made sense.
        ///     For instance, if -ir false true true false is inputted, it will only take that last "false" and use that.
        ///     Probably not the most elegant solution, but it works.
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void ParseArgs(string[] args)
        {
            /*string[] testargs = { "-ir", "false", "-ot", "1", "-ffmpeg", "-y", "-i", "\"input.mp3\" \"output.mp4\"", "-ffmpegend", "-context", "explorer" };
            args = testargs;*/

            //Some variables that indicates what and how things are being parsed.
            bool readingFfmpeg = false;     //Override for the parser if ffmpeg parameters are currently being read.
            string currentlyReading = "";   //What the parser is currently reading
            string ffmpegParams = "";       //The entire ffmpeg param string. Used to make conversions.

            if (Active)
            {
                try
                {
                    //Step through the entire list of parameters/args
                    for (int i = 0; i < args.Length; ++i)
                    {
                        //Check if something new will be read
                        for (int s = 0; s < logicalParameters.Length; ++s)
                        {
                            //Don't check for new things to read if FFMPEG is reading (To remove conflicts)
                            //Else. Only check for -ffmpegend to end it.
                            if (!readingFfmpeg)
                            {
                                if (args[i] == logicalParameters[s])
                                    currentlyReading = args[i];
                            }
                            else
                            {
                                if (args[i] == "-ffmpegend")
                                {
                                    currentlyReading = "";
                                    readingFfmpeg = false;
                                }
                            }
                        }

                        //Go through a switch and read values, but only the param type isn't the current thing being read.
                        if (currentlyReading != args[i])
                            switch (currentlyReading)
                            {
                                case "-ir":
                                    InstantRun = bool.Parse(args[i]);
                                    break;
                                case "-ot":
                                    OpenType = (OPEN_TYPE)int.Parse(args[i]);
                                    break;
                                case "-context":
                                    Context = args[i];
                                    break;
                                case "-oc":
                                    OverrideClose = bool.Parse(args[i]);
                                    break;
                                case "-op":
                                    OverridePicker = bool.Parse(args[i]);
                                    break;
                                case "-settingConv":
                                    RegEdit.ChangeContextMenu(int.Parse(args[i]));
                                    break;
                                case "-ffmpeg":
                                    ffmpegParams += args[i] + " ";
                                    break;
                            }
                    }
                }
                catch (Exception e)
                {
                    //Write an error if something goes wrong
                    MessageBox.Show($"Something went wrong when reading app parameters. It was probably malformed.\nHere's all i know: {e.Message}", Lang.R.Text.Error);
                }
            }
            //Make so that the app only says it skipped parameters if any parameter (When opening the app) was inputted.
            else if(!Active && args.Length != 0)
            {
                SkippedParams = true;
            }

            //Set the ffmpeg parameters (Note: The "out" parameters are added in the GetIOFrom function (Just above)
            ffmpegParameters = ffmpegParams;
        }

        /// <summary>
        /// (DUMB)
        /// Adds an output parameter to the ffmpeg parameters.
        /// It checks for how many " are present. If only two are there, it will add the output to the end of the ffmpeg param string
        /// </summary>
        /// <param name="ffmpegParam">The ffmpeg parameter string in full</param>
        /// <param name="output">The output path and file to be placed inside the string</param>
        /// <returns>The same ffmpeg param string but with the output added</returns>
        private static string AddOutputParam(string ffmpegParam, string output)
        {
            return $"{ffmpegParam} \"{output}\"";
        }
    }
}
