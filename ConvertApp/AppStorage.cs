using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/***
 * Contains custom app storage.
 * Saved as .storage files
 * 
 * NOTE ONLY SAVES FOR previousExeLocation RIGHT NOW!
 * NOTE Class is a mess. Whadya want?
 * 
 * NOTE Had to make this because the .NET framwork doesn't allow me to easily change the folder names created in the AppData/Local folder for the app
 *      So this is the solution. An easy to get, located where the exe is
 * NOTE Because i'm lazy and don't want to redo all the implementations of the regular .NET implementation on the storage/settings,
 *      right now, this only contains the key for previousExeLocation
 */
namespace ConvertApp
{
    internal class AppStorage
    {
        //Some globals
        public static string fileName = "Settings.storage";         //Name of the settings file
        public static string previousExeLocation = "";              //The entire contents of the settings file

        /// <summary>
        /// Updates the settings file with new info.
        /// </summary>
        public static void Update()
        {
            //Create the file if it doesn't exist.
            if (!SettingsFileExists())
                CreateSettingsFile(fileName);

            //Read the contents
            previousExeLocation = LoadSetting(fileName);
        }

        public static string GetCurrentPreviousLocation()
        {
            Update();
            return previousExeLocation;
        }
        public static void SetCurrentPreviousLocation()
        {
            CreateSettingsFile(fileName);
        }

        /// <summary>
        /// Gets a key contained within the setting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /*public static object Query(string key)
        {

        }*/

        /// <summary>
        /// Loads the settings file that contains all the settings.
        /// Creates a new one if it doesn't exist. Adds the location of the exe file
        /// </summary>
        /// <param name="name">The name of the file. Starts from the exe location</param>
        private static string LoadSetting(string name)
        {
            //The result to return
            string result = "";
            try
            {
                //Read the file and add to the result
                using (StreamReader sr = File.OpenText(name))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        result += s;
                    }
                }
            }
            catch (Exception e)
            {
                //If something goes wrong during reading.
                MessageBox.Show($"{Lang.R.Text.Error}: {e.Message}");
                return "";
            }

            //Return the file contents.
            return result;
        }

        /// <summary>
        /// Creates the settings file itself with all the values used for this function
        /// </summary>
        /// <param name="name">The name of the file. Starts from the exe location</param>
        private static void CreateSettingsFile(string name)
        {
            try
            {
                //Writes to the specified file
                using (FileStream fs = File.Create(name))
                {
                    byte[] data = new UTF8Encoding(true).GetBytes(Common.GetExeLocation());
                    fs.Write(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                //If something goes wrong during writing
                MessageBox.Show($"{Lang.R.Text.Error}: {e.Message}");
            }
        }

        private static bool SettingsFileExists()
        {
            return File.Exists(fileName);
        }


        //OLD STRUCTURE. Might return to it if i expand this functionality.

        /*public static dynamic Settings;                                         //The settings object. You refer to it to get and set settings
        public static string fileName;                                          //Name of the settings file itself.
        private static string[] settingReferences = { "previousExeLocation" };  //This should read from an xml that's automatically generated as new values are called and saved inside the code. But this is how it will work for now.

        /// <summary>
        /// Initializes the class and its variables
        /// </summary>
        public static void Initialize()
        {
            fileName = "Settings.storage";

            Settings = new ExpandoObject();
            AddToDyn(Settings, "Text", "previousExeLocation", value);

            CreateSettingsFile(fileName);
            string data = LoadSettingsFile(fileName);
        }

        /// <summary>
        /// Loads the settings file that contains all the settings.
        /// Creates a new one if it doesn't exist.
        /// </summary>
        /// <param name="name">The name of the file. Starts from the exe location</param>
        private static string LoadSettingsFile(string name)
        {
            //The result to return
            string result = "";
            try
            {
                //Read the file and add to the result
                using (StreamReader sr = File.OpenText(name))
                {
                    string s = "";
                    while((s = sr.ReadLine()) != null)
                    {
                        result += s;
                    }
                }
            }
            catch (Exception e)
            {
                //If something goes wrong during reading.
                MessageBox.Show($"{Lang.R.Text.Error}: {e.Message}");
                return "";
            }

            //Return the file contents.
            return result;
        }

        /// <summary>
        /// Creates the settings file itself with all the values used for this function
        /// </summary>
        /// <param name="name">The name of the file. Starts from the exe location</param>
        private static void CreateSettingsFile(string name)
        {
            try
            {
                //Writes to the specified file
                using (FileStream fs = File.Create(name))
                {
                    byte[] data = new UTF8Encoding(true).GetBytes("Whoa");
                    fs.Write(data, 0, data.Length);
                }
            }
            catch(Exception e)
            {
                //If something goes wrong during writing
                MessageBox.Show($"{Lang.R.Text.Error}: {e.Message}");
            }
        }

        private static bool SettingsFileExists()
        {
            return File.Exists(fileName);
        }

        //Adds a chain of objects that can be used throughout the program by using AppStorage.Settings.
        private static void AddToDyn(dynamic target, string wordName, string key, string value)
        {
            //Error handling
            if (!(target is IDictionary<string, object> dictionary))
            {
                throw new ArgumentException("The target parameter has to be a dictionary type object");
            }

            if (!dictionary.TryGetValue(wordName, out var word))
            {
                word = new ExpandoObject();
                dictionary[wordName] = word;
            }

            var wordDictionary = (IDictionary<string, object>)word;
            wordDictionary[key] = value;
        }
        */
        //previousExeLocation
    }
}
