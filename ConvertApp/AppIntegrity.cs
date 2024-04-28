using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

/***
 * Checks if files and locations are where they should be.
 * Also checks for the exe file location. Useful to make sure the convert option in Windows Explorer works.
 * 
 * Good to note about settings in .NET
 * https://stackoverflow.com/questions/38429461/how-to-change-the-predefined-userconfig-directory-of-my-net-application
 * Will have to use my own settings, it seems. Fair enough..
 * 
 * NOTE A better approach for integrity would be to instead of just checking for if the file exists or not, it could use SHA256 to truly verify them.
 *      But that may be a bit overkill for this app.
 * 
 */
namespace ConvertApp
{
    internal class AppIntegrity
    {
        /// <summary>
        /// Checks if the exe file has changed location.
        /// It won't do anything if the Windows Register keys for the file right context menu don't exists.
        /// It also always marks the previous location settings variable to the current location of the exe.
        ///     Meaning that checks will only happen once per location change.
        /// </summary>
        public static void CheckExeLocation()
        {
            //Checks to see if the keys exists in the registry
            if (RegEdit.Exists())
            {
                //Sets up the result for the dialog that'll prompt the user to remove the register keys.
                DialogResult locationChangedResult = DialogResult.None;

                //Checks if the location has changed. Prompt user if it has.
                if (Common.HasExeLocationChanged())
                    locationChangedResult = MessageBox.Show($"The location of the exe has changed from\n{AppStorage.GetCurrentPreviousLocation()} to \n{Common.GetExeLocation()}\n\nThe 'Convert' menu item when right clicking a file in Windows Explorer won't know what to do, so it needs to be changed if it's been added.\nDo that now?", Lang.R.Text.Error, MessageBoxButtons.YesNo);

                //Decision that was made
                if (locationChangedResult == DialogResult.Yes)
                {
                    if(!RegEdit.PromptChange(0))
                        MessageBox.Show("You can always change integration in the settings. But conversions in Explorer won't work until you do");
                }
                else if (locationChangedResult == DialogResult.No)
                    MessageBox.Show("You can always change integration in the settings. But conversions in Explorer won't work until you do");
            }

            //Set the Properties.Settings.Default.previousExeLocation to the current location. The warning only happens once each time it changes.
            AppStorage.SetCurrentPreviousLocation();
        }

        /// <summary>
        /// Checks the locations of the various assets used in the project. If one is missing, then.. I don't know, re-download the app?
        /// </summary>
        public static void ValidateFileLocations()
        {
            //List of missing files if any exists.
            List<string> filesMissing = new List<string>();
            string filesMissingString = "";

            //Load the files that are expected to be in the directories where the exe is.
            List<string> filesList = ReadMetaInfoFile();

            //Check if any of them don't exist
            for(int i = 0; i < filesList.Count; ++i)
            {
                if (!File.Exists(filesList.ElementAt(i)))
                {
                    filesMissing.Add(filesList.ElementAt(i));
                }
            }

            //Put the missing files into one string
            for(int i = 0; i < filesMissing.Count; ++i)
            {
                filesMissingString += filesMissing.ElementAt(i) + "\n";
            }

            //Check if missing files has anything in it. If not, then files are all accounted for and app can proceed normally
            if(filesMissing.Count > 0)
            {
                MessageBox.Show($"One or more files are missing from the app\n\n{filesMissingString}\n\nThe app will try to continue to work, but if things start crashing, this is why. You might want to redownload the app.", Lang.R.Text.Error);
            }
        }

        private static List<string> ReadMetaInfoFile()
        {
            //Initialize some variables
            List<string> result = new List<string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "ConvertApp.Meta.meta_info.txt";

            try
            {

                //Open file stream from the resources
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            //Read entire file line by line and add to the result
                            while (!reader.EndOfStream)
                                result.Add(reader.ReadLine());
                        }
                    }
                    else
                    {
                        //Return null if it failed for whatever reason
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }

            return result;
        }
    }
}
