using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertApp
{
    internal class Common
    {
        //Enumerator used for the GetAppVersion function
        public enum APP_VER_VERBOSITY
        {
            VERSION,
            APP,
            FULL,
        };

        //Some constants and statics
        public static readonly float FONT_SIZE_SMALL = 7f;
        public static readonly float FONT_SIZE_MEDIUM = 9f;
        public static readonly float FONT_SIZE_BIG = 12f;

        /// <summary>
        /// Gets the location of the exe file.
        /// </summary>
        /// <returns>The location of the exe file</returns>
        public static string GetExeLocation()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        /// <summary>
        /// Checks if the location of the exe file has changed.
        /// Useful to make sure the user knows that the convert action in the context menu item (Convert) in Windows Explorer won't work
        /// </summary>
        /// <returns></returns>
        public static bool HasExeLocationChanged()
        {
            //Set it the first time the app starts and mark it as "not changed"/false since it's empty
            if(AppStorage.GetCurrentPreviousLocation() == "")
            {
                AppStorage.SetCurrentPreviousLocation();
                //Properties.Settings.Default.previousExeLocation = GetExeLocation();
                return false;
            }

            //If the previous location isn't empty and it isn't the location of the exe, mark it as having changed location
            if (AppStorage.GetCurrentPreviousLocation() != GetExeLocation())
                return true;
            else
                return false;
            /*if (Properties.Settings.Default.previousExeLocation != GetExeLocation())
                return true;
            else
                return false;*/
        }

        /// <summary>
        /// Restarts the app and adds admin privs to it.
        /// DANGER: It's generally recommended that only the specific process that does the admin required action should get admin privileges.
        ///         To that end, this should happen in a different exe or DLL or something like that. But i mean.. Come on.
        /// </summary>
        /// <param name="contextMenuType">The type of parameter the app will change once the app starts up again. 2 for app, 1 for console, 0 for none</param>
        /// <returns>Success status. True means it worked and false means an error occured.</returns>
        public static bool RestartAppWithAdminPrivs(int contextMenuType)
        {
            //The process object
            ProcessStartInfo psi = new ProcessStartInfo();

            //Set filename to the current exe path
            psi.FileName = Application.ExecutablePath;

            //This makes sure that it'll run as administrator.
            psi.Verb = "runas";
            
            //Sets the argument that does the change to the setting at application start.
            psi.Arguments = $"-settingConv {contextMenuType}";

            try
            {
                //Start process and exit it if it managed to start.
                Process.Start(psi);
                Application.Exit();
            }
            catch(Exception e)
            {
                MessageBox.Show($"{Lang.R.Text.Error}: {e.Message}", Lang.R.Text.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if the user is currently an admin or not.
        /// </summary>
        /// <returns></returns>
        public static bool IsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Opens a dialog box that lets you pick a file.
        /// </summary>
        /// <returns>The file path</returns>
        public static string OpenFileDialog()
        {
            var fd = new OpenFileDialog();
            if(fd.ShowDialog() == DialogResult.OK)
            {
                return fd.FileName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Converts a time string with the format hh:mm:ss into milliseconds.
        /// </summary>
        /// <param name="TimeStr">The string that has that format</param>
        /// <returns>milliseconds</returns>
        public static long ConvertTimeStringToMilliseconds(string TimeStr)
        {
            //Parses the time string
            TimeSpan TimeSpan = TimeSpan.ParseExact(TimeStr, "hh':'mm':'ss'.'ff", null);

            return (long)TimeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// A dialog box that lets you choose what folder to use.
        /// </summary>
        /// <returns>Returns a folder path depending on what the user selected. Empty if user didn't click ok</returns>
        public static string OpenFileDialogCreateOutput()
        {
            var folderBrowser = new FolderBrowserDialog();

            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                return folderBrowser.SelectedPath;
                //string filePath = Path.Combine(folderBrowser.SelectedPath, "myFile.txt");
                //File.WriteAllText(filePath, "Hello, world!");
                //MessageBox.Show($"File created at: {filePath}", "Message");
            }

            return "";
        }

        /// <summary>
        /// Cuts the path out of a file string and only keeps the file name and its extention
        /// </summary>
        /// <returns>Only the file name and extention. Or an empty string if it ends with a /</returns>
        public static string CutPath(string FilePath)
        {
            //Three outcomes. The file path ends with \, it's a proper file path or it doesn't contain any \.
            //It won't check for any other.

            //Doesn't contain a \, ends with \. Defaults to proper path otherwise.
            if (!FilePath.Contains("\\"))
            {
                return FilePath;
            }
            else if(FilePath.EndsWith("\\"))
            {
                return "";
            }
            else
            {
                string[] split = FilePath.Split('\\');
                return split[split.Length-1];
            }
        }
        /// <summary>
        /// Picks out the file type of the file. Can be a path or just the file name itself
        /// </summary>
        /// <param name="File">File name and/or path</param>
        /// <returns>Returns the extension/file type</returns>
        public static string SelectFileType(string File)
        {
            try
            {
                string r = File.Split('.')[File.Split('.').Length - 1];
                return r;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// A function that checks if the file is compatible (So no exe or anything. Just the ones found in formats)
        /// </summary>
        /// <param name="FileName">The file name. Can be the full path as long as it ends with the file extension</param>
        /// <param name="Formats">List of compatible formats</param>
        /// <returns>Returns true if it's compatible. False otherwise</returns>
        public static bool CheckCompatibleFormats(string FileName, string[] Formats)
        {
            try
            {
                string[] parts = FileName.Split('.');
                string ext = parts[parts.Length - 1].ToLower();

                foreach(string check in Formats)
                {
                    if (ext == check)
                        return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Loads a font located in the project's files. Make sure to check the properties of the font you want to use in the files and change the build action to "Embedded Resource" for this to work.
        /// </summary>
        /// <param name="internalUri">Internal uri. Includes the name of the font in the files too. They're formatted as such: Folder.Folder.File.extension</param>
        /// <returns>The font to be used by anything that can have a Font object assigned to it. Or null if it failed to load</returns>
        public static Font LoadEmbededFont(string internalUri, float size)
        {
            //Assign some variables
            string fontAssetPath = $"ConvertApp.{internalUri}";     //string representing the location of the font in the project's files.
            string fontName = "";                                   //Has the font name itself in it
            Font f = null;                                          //Font object to assign to so it can be returned

            //Get the fontName (A little ugly. But it works. No error handling either)
            string[] parts = internalUri.Split('.');
            fontName = parts[parts.Length - 2] + "." + parts[parts.Length - 1];

            //Wrap the opening of the file in a using statement to do error handling (Much like try/catch/finally
            using(Stream fStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fontAssetPath))
            {
                if(fStream != null)
                {
                    //Read font data from stream
                    byte[] data = new byte[fStream.Length];
                    fStream.Read(data, 0, (int)data.Length);

                    //Add a font to a collection object
                    PrivateFontCollection PrivFontColl = new PrivateFontCollection();
                    IntPtr fDataPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
                    PrivFontColl.AddMemoryFont(fDataPtr, data.Length);

                    //Get the first font. Duh.
                    f = new Font(PrivFontColl.Families[0], size);

                    //Return with the f object. Assign as necessary.
                    return f;
                }
            }

            return null;
        }
        public static Font LoadEmbededFont(string internalUri)
        {
            return LoadEmbededFont(internalUri, 12f);
        }

        /// <summary>
        /// Opens a folder
        /// </summary>
        /// <param name="folderPath">Path to the folder</param>
        public static void OpenFolder(string folderPath)
        {
            try
            {
                Process.Start("explorer.exe", folderPath);
            }
            catch
            {
                MessageBox.Show("Folder doesn't exist");
            }
        }

        /// <summary>
        /// Reads a file and returns everything in it.
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>The file contents</returns>
        public static String ReadFromFile(string filePath)
        {
            //Initialize the result
            string result = "";

            //Check if the file exists before trying to use it
            if (File.Exists(filePath))
            {
                //Read the entire file to end. Error handle too.
                using (StreamReader reader = new StreamReader(filePath))
                {
                    result = reader.ReadToEnd();
                }
            }

            //Return the result
            return result;
        }

        /// <summary>
        /// Gets the app version
        /// </summary>
        /// <param name="verbosity">Verbosity level. How much should be regurgitated</param>
        /// <returns></returns>
        public static string GetAppVersion(APP_VER_VERBOSITY verbosity)
        {
            //Get the version from the project's files
            //Note that you can change the version number if you go to Project (Int the solution explorer) -> Properties -> Application -> Assembly information
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;

            switch (verbosity)
            {
                case APP_VER_VERBOSITY.VERSION:
                    return $"{ver.Major}.{ver.Minor}";
                case APP_VER_VERBOSITY.APP:
                    return $"{Lang.R.Text.AppVer}: {ver.Major}.{ver.Major}";
                case APP_VER_VERBOSITY.FULL:
                    return $"{Lang.R.Text.ApplicationVersion}: {ver.Major}.{ver.Minor}:{ver.Build}";
                default:
                    return Lang.R.Text.Error;
            }
        }

        /// <summary>
        /// Parses the font size text found in the Properties.Settings.Default.ConsoleTextSize variable.
        /// It defaults to "Small" but also has Medium and Big.
        /// FONT_SIZE constants are available in this class.
        /// </summary>
        /// <returns>Returns the float value representing the size</returns>
        public static float ParseFontSizeFromSettings()
        {
            //Initialize some stuff
            string sizeStr = "Small";//Default value
            float result = FONT_SIZE_SMALL;

            //If it's been selected in options.
            if(Properties.Settings.Default.ConsoleTextSize != "")
            {
                sizeStr = Properties.Settings.Default.ConsoleTextSize;
            }

            //Set sizes accordingly
            switch (sizeStr)
            {
                case "Small":
                    result = FONT_SIZE_SMALL;
                    break;
                case "Medium":
                    result = FONT_SIZE_MEDIUM;
                    break;
                case "Big":
                    result = FONT_SIZE_BIG;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Reads a string and if that string is null, it will return it as empty.
        /// Used to make sure to always get an empty string. No need to check for null and empty.
        /// </summary>
        /// <param name="str">Input</param>
        /// <returns>The input, but reverted to empty if it's null</returns>
        public static string ReadNullString(string str)
        {
            if (str == null)
                return "";
            else
                return str;
        }

        /// <summary>
        /// Counts the amount of times a character appears in a string.
        /// </summary>
        /// <param name="source">The string to be counted</param>
        /// <param name="toFind">The character to find</param>
        /// <returns>The result of the calculation</returns>
        /// <remarks>Thanks to code-maze for the function</remarks>
        public static int CountCharsUsingIndex(string source, char toFind)
        {
            int count = 0;
            int n = 0;
            while ((n = source.IndexOf(toFind, n) + 1) != 0)
            {
                n++;
                count++;
            }
            return count;
        }
    }
}
