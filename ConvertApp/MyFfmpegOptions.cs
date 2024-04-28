using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

/***
 * Options class that keeps track of parameters that ges set in the FormOptions class
 * Also keeps track of ffmpeg parameters and what not. Which are an options thing in it of itself.
 * 
 * 
 */
namespace ConvertApp
{
    internal class MyFfmpegOptions
    {
        //Define some globals
        public static bool customParametersActive = false;          //Indicates if the app should use the custom parameters or not
        public static string customParameters = "";                 //Custom FFmpeg parameters. Let's you use any ffmpeg command that exists.
        public static readonly string ffmpegTutorialLink =          //Link to a resource that explains what commands are available
            "https://ffmpeg.org/ffmpeg.html";    
        public static readonly string defaultCustomParameters =     //Default convert string
            "-y -i \\\"{0}\\\" \\\"{1}\\\"";
        public static int rotation = 0;                             //How much the converter should rotate the video 0, 90, 180, 270
        public static string bitrate = "";                          //Bitrate. Includes a "k" letter, so it had to be a string. Treats bufsize and b:v as the same. Because why not?
        public static int framerate = -1;                           //Framerate.

        /// <summary>
        /// Initializes all the values.
        /// </summary>
        public static void Init()
        {
            Load();
        }

        /// <summary>
        /// Saves all the parameters related to this into local storage.
        /// </summary>
        public static void Save()
        {
            Properties.Settings.Default.customParametersActive = customParametersActive;
            Properties.Settings.Default.customParameters = customParameters;
            Properties.Settings.Default.rotation = rotation;
            Properties.Settings.Default.bitrate = bitrate;
            Properties.Settings.Default.framerate = framerate;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Loads values
        /// </summary>
        public static void Load()
        {
            customParametersActive = Properties.Settings.Default.customParametersActive;
            customParameters = Properties.Settings.Default.customParameters;
            rotation = Properties.Settings.Default.rotation;
            bitrate = Properties.Settings.Default.bitrate;
            framerate = Properties.Settings.Default.framerate;
        }

        /// <summary>
        /// Sets and saves the custom parameter boolean.
        /// </summary>
        /// <param name="val">Value to set it to</param>
        public static void SetCustomParametersActive(bool val)
        {
            customParametersActive = val;
            Save();
        }

        /// <summary>
        /// Sets and saves the custom parameter string.
        /// </summary>
        /// <param name="val">Value to set it to</param>
        public static void SetCustomParameters(string val)
        {
            customParameters = val;
            Save();
        }

        /// <summary>
        /// Sets and saves the rotation
        /// </summary>
        /// <param name="val">Value to set it to</param>
        public static void SetRotation(int val)
        {
            rotation = val;
            Save();
        }

        /// <summary>
        /// Sets and saves the bitrate
        /// </summary>
        /// <param name="val">Value to set it to</param>
        public static void SetBitrate(string val)
        {
            bitrate = val;
            Save();
        }

        /// <summary>
        /// Sets and saves the framerate
        /// </summary>
        /// <param name="val">Value to set it to</param>
        public static void SetFramerate(int val)
        {
            framerate = val;
            Save();
        }

        /// <summary>
        /// Checks framerate and bitrate and determines if the values inputted are valid.
        /// Framerate has to be a number and bitrate must end with "k".
        /// It also checks if the numbers are too large.
        /// </summary>
        /// <param name="framerate">Framerate. Can be between 1 and 1000</param>
        /// <param name="bitrate">Bitrate. Can be between 1k and 320k</param>
        /// <returns>Success if true. Fail if false</returns>
        /// <remarks>This should have been split between two functions. Good to know for another time.</remarks>
        public static bool CheckFramerateBitrateValidity(int framerate, string bitrate)
        {
            //If framerate is outside of expected values
            if((framerate < 1 || framerate > 1000) && framerate != -1)
            {
                MessageBox.Show("Framerate has to be between 1 and 1000", Lang.R.Text.Error);
                return false;
            }

            //Check bitrate
            try
            {
                //Sus out if it's a number. Removes the k and does a regex that matches numbers only.
                string bitrateStrNr = bitrate.ToLower().Replace("k", "");
                int bitrateNr = int.Parse(bitrateStrNr);
                bool isNumber = new Regex(@"^\d+$").IsMatch(bitrateStrNr);

                //Check that the number is within parameters (1 and 320)
                if(bitrateNr > 320 || bitrateNr < 1)
                {
                    MessageBox.Show("Bitrate has to be between 1 and 320", Lang.R.Text.Error);
                    return false;
                }

                //Check that it ends with k. If not, then..
                if (!bitrate.EndsWith("k"))
                {
                    MessageBox.Show("Bitrate has to end with a 'k' (Example: 160k)", Lang.R.Text.Error);
                    return false;
                }
            }
            catch
            {
                //If anything during parsing goes wrong, it was the wrong format anyway, and will return false. It failed.
                if (bitrate != "")
                {
                    MessageBox.Show("Bitrate was the wrong format entirely. It has to be a number between 1 and 320 and end with a k (Ex: 160k)", Lang.R.Text.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            //If all goes well, return true. VALID!
            return true;
        }

        /// <summary>
        /// Compiles parameters from the options menu with the custom parameter string.
        /// So if you have rotate on 90 and you have custom parameter enabled, it will attempt to add rotation to that string.
        /// (Might not always work).
        /// </summary>
        public static void CompileParameters()
        {
            //TODO To be added at some point maybe.
        }

        /// <summary>
        /// Takes the parsed ffmpeg param string and adds options to it depending on which was selected in the options menu.
        /// </summary>
        /// <param name="input">The parsed ffmpeg string to add stuff to</param>
        /// <returns>Same string with added parameters</returns>
        public static string ApplyOptionsToFFmpegParams(string input)
        {
            //string to be manipulated
            string result = "";

            /**Add commands to the start of the input string**/
            //Add rotation
            if (rotation != 0)
            {
                result += "-display_rotation " + rotation + " ";
            }

            //Commit to start of the input string
            result = result + input;

            /**Add commands between input and output**/
            //Do some stuff to make it possible to insert in "the middle" (Between input/output in the ffmpeg string)
            string indexToFind = "\" \"";                               //Index to find. Assumes the space between the two are always exactly like that. Which it should be.
            int indexOfCenter = result.IndexOf(indexToFind) + 1;        //Get the middle of input and output.
            string subInput = result.Substring(0, indexOfCenter);       //Get the first part from start to the center point.
            result = result.Replace(subInput, "");                      //Erase the first part of input (From the middle of in-/output)
            string contentToAddInMiddle = "";                           //Use this variable to insert things that will be in the middle

            //Add bitrate
            if (bitrate != "")
            {
                contentToAddInMiddle += "-b:v " + bitrate;
                contentToAddInMiddle += " -bufsize " + bitrate;
            }

            //Add framerate
            if(framerate != -1)
            {
                contentToAddInMiddle += " -r " + framerate;
            }

            //Add the content to the middle. Commit!
            //subInput is the original first part that was split.
            //Add a space (Note that the right part of contentToAddInMiddle doesn't need a space)
            //Add the content that should be in the middle
            //Add the "result" variable to the end. Which, at this point, contains the second half of the split in the middle that happened.
            result = subInput + " " + contentToAddInMiddle + result;

            /**Add things to the end of the input string*/
            //NOTHING YET...

            //Return the result
            return result;
        }

        /// <summary>
        /// Breaks up and "explains" parameters for each new line. Mainly used in the convert button tooltip.
        /// The "explanations" are: Param, Value
        /// For instance, it will break up -y -i "inputpath" "output" path into
        ///         Param: -y
        ///         Param: -i
        ///         Value: "inputpath"
        ///         Value: "inputpath"
        /// It treats lines starting with - as a param and others are values
        /// </summary>
        /// <param name="ffmpegParamString">An Ffmpeg parameter string</param>
        /// <returns>The formatted string explaining the parameter string</returns>
        /// <remarks>Could be expanded to actually explain each parameter from the documentation. And then this function could be put in its own form as a reference. But we'll see..</remarks>
        public static string ExplainParameters(string ffmpegParamString)
        {
            string result = "";
            try
            {
                //Splits the string with spaces.
                string[] pa = ffmpegParamString.Split(' ');

                //This is here to make sure that the app doesn't count spaces inside of strings (EX: "text")
                bool readingString = false;

                //Steps through the whole list and adds the appropriate text to each line
                for(int i = 0; i < pa.Length; ++i)
                {
                    //Mark the different types and add them to the result.
                    if (pa[i].StartsWith("-"))
                    {
                        result += "PARAM: " + pa[i] + $"{ExplainParam(pa[i], true)}";
                    }
                    else
                    {
                        //Don't count it as new VALs if it's still going through a string.
                        if (!readingString)
                            result += "VAL: " + pa[i];//Normal operation
                        else
                            result += pa[i];//The reason the readingString check needs to happen is because a new line was made on a string. So re-add that.

                        if (pa[i].Contains("\""))
                        {
                            readingString = !readingString;
                        }
                    }

                    //Only add a new line to the result if it's not at the end of the list and if it isn't reading a string
                    if (i != pa.Length - 1 && !readingString)
                        result += "\n";
                }
            }
            catch
            {
                //Returns with an "invalid" text if it catches an exception at any point
                return Lang.R.Text.Invalid;
            }

            return result;
        }

        /// <summary>
        /// Explains ffmpeg parameters. Pretty neat, huh?
        /// Only does it for the most common ones right now, but this can be filled with just about any.
        /// </summary>
        /// <param name="param">Ffmpeg Param to explain</param>
        /// <param name="showArrow">Whether to show a little arrow</param>
        /// <returns>The explanation</returns>
        public static string ExplainParam(string param, bool showArrow)
        {
            //The result to use
            string result = "";

            //Check cases that has an explanation
            switch (param)
            {
                case "-y":
                    result = "Forces output to always overwrite existing file if one exists";
                    break;
                case "-i":
                    result = "Sets in-/output of conversion";
                    break;
                case "-b:v":
                    result = "Sets video bitrate of conversion";
                    break;
                case "-r":
                    result = "Forces framerate to specified value";
                    break;
                case "-bufsize":
                    result = "Sets the buffer size for the encoding process";
                    break;
            }

            //Shows the arrow if it was requested and if result isn't empty
            if (showArrow && result != "")
                result = " <= " + result;

            //Blit the result
            return result;
        }

        /// <summary>
        /// Does the same as setting it normally, but it does error/warning checking depending on certain conditions.
        /// 
        /// Checks if its empty
        /// Checks if it doesn't contain the -y ffmpeg parameter.
        /// Checks if it contains {0} or {1}
        /// 
        /// </summary>
        /// <param name="val"></param>
        public static void SetCustomParametersWithCheck(string val)
        {
            //Set the result parameter. -1 isn't an actual one, but it makes sense to use for NA/null/nil/undefined
            DialogResult result = (DialogResult)(-1);

            //Check some changes that would cause a problem and show a warning
            if (LogicIsEmpty(val))              //Check if the string is empty
            {
                result = MessageBox.Show(Lang.R.Text.LeavingCustomParamsEmpty, Lang.R.Text.WarningCustomParam, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else if (LogicYParam(val))          //Check if it doesn't contain -y
            {
                result = MessageBox.Show(Lang.R.Text.RemovingY, Lang.R.Text.WarningCustomParam, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else if (LogicContainsIO(val))    //Check if it contains input and output
                     
            {
                result = MessageBox.Show(Lang.R.Text.NeedZeroOne, Lang.R.Text.WarningCustomParam, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            //Set the parameter
            if(result == (DialogResult)(-1) || result == DialogResult.Yes)
                SetCustomParameters(val);
        }

        /// <summary>
        /// Warns about parameter errors. To be blitted out in the console or other places.
        /// The function takes every function in this method that starts with "Logic" and invokes all of them
        /// After invokation, it gets the result of these functions and adds a warning/error text to the result variable
        /// </summary>
        /// <returns>Empty if no warnings. Otherwise, it gets specific</returns>
        /// <param name="val">The value to be checked. This is typically customParameters</param>
        /// <param name="breakResult">If it's true, it will break/return the function as soon as it finds one error/warning. Resulting in only one string getting written and returned</param>
        /// <remarks>Very overdesigned and kind of dumb with the reflect API, but it's too fun not to use</remarks>
        public static string WarnParameterErrors(string val, bool breakResult)
        {
            //Initialize the result. It will add text each time it finds an error/warning unless breakResult is true
            string result = "";

            //Initialize some reflect stuff
            Type type = typeof(MyFfmpegOptions);//Used to check for functions. In what context. Which is this class
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);//Gets every function in this class.
            
            //Step through every method in this class
            foreach(MethodInfo method in methods)
            {
                //Sets a name parameter. Easier to refer to and technically faster
                string name = method.Name;

                //Only pick functions starting with "Logic"
                if (name.StartsWith("Logic"))
                {
                    //The logic method object itself. To be invoked
                    MethodInfo logicMethod = type.GetMethod(name, BindingFlags.Static | BindingFlags.Public);

                    //Condition result for the "Logic" function
                    bool condition = (bool)logicMethod.Invoke(null, new object[] { val });

                    //Check the function name, and if the condition was met, add to the result string.
                    if (condition)
                    {
                        switch (name)
                        {
                            case "LogicIsEmpty":
                                result += Lang.R.Text.ErrorFFmpegParamsEmpty;
                                break;
                            case "LogicContainsIO":
                                result += Lang.R.Text.ErrorNoIOVars;
                                break;
                            case "LogicYParam":
                                result += Lang.R.Text.WarningNoY;
                                break;
                        }

                        //Add a new line
                        result += "\n";

                        //Return the function if one condition was ever reached
                        if (breakResult)
                            return result;
                    }
                }
            }

            //Return with the result
            return result;
        }

        //Logical operations for when checking custom ffmpeg parameters. Error handling and what not.
        //They get deferred to here for readability and because it might be useful to use these exact conditions in other functionality.
        //And it might be neat to put these in an array and check them one by one.
        //Also contains the delegate function to refer to these functions
        public delegate bool MyLogic(string val);
        public static bool LogicIsEmpty(string val) { return val.Trim() == ""; }
        public static bool LogicContainsIO(string val) { return (!val.Contains("{0}") || !val.Contains("{1}")); }
        public static bool LogicYParam(string val) { return !val.Contains("-y"); }
    }
}
