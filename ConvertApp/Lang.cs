using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ConvertApp
{
    /***
     * Contains language functionality.
     * Let's the program refer to words in the program with normal object references
     * Such as Lang.R.Text.Credits
     * Reads from an XML file and has the ability to use different languages.
     *
     * I did it this way because i felt like it.
     * I know i could easily just have done a cursory Google search and found the proper way to add language functionality to the app, but this was more fun and i always wanted to do it.
     *      It did cause some issues with how controls got their values translated, though.
     *
     */
    internal class Lang
    {
        //public static Dictionary<string, string> Words;   //Contains keys and associated phrases and words
        public static dynamic R;                            //The resource object to refer to when getting text for the program
        public static string LanguageXMLDir;                //The full path to the file name. Set at initialization.
        public static List<string> languages;               //List of supported languages. Gets automatically added based on the contents of the "languages" folder in the project.
        private static bool MachineTranslated = false;      //Whether or not the translation was done by a machine. Gets set by a variable in each xml file called "MachineTranslated"
        public static string currentLanguage;               //The current set language
        public static string currentLanguageCode;           //An international ISO for language codes. Used for the culture (Set culture language) function. Read directly from language xml

        /// <summary>
        /// Initializes words. Gets initialized at Program.cs. As early as possible.
        /// </summary>
        public static void Init(string language)
        {
            //Initialize some variables
            XDocument xmlDoc;

            //Set current language
            currentLanguage = language;

            //Initializes the languages array.
            SetLanguagesList();

            //Set the directory to the xml (Make sure to change property in files to "always copy to directory" for this to work)
            LanguageXMLDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "languages", $"{language}.xml");

            //Initializes the resource object
            R = new ExpandoObject();

            //Do XML stuff. Parse through it and add each element to the R object.
            xmlDoc = XDocument.Load(LanguageXMLDir);
            foreach(XElement element in xmlDoc.Descendants("words"))
            {
                string key = element.Attribute("name").Value;
                string value = element.Value;

                Add(R, "Text", key, value);
            }

            //Read if it was machine translated (Note: Always make sure that it's the first <option> in the document)
            MachineTranslated = bool.Parse(xmlDoc.Descendants("option").ElementAt(0).Value);
            currentLanguageCode = xmlDoc.Descendants("option").ElementAt(1).Value;

            //Set system language for the current thread in the app.
            SetCulture();
        }

        //Adds a chain of objects that can be used throughout the program by using Lang.Thing.KeyName
        private static void Add(dynamic target, string wordName, string key, string value)
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

        //Initializes and sets the languages array by listing all the xmls in the "languages" folder
        private static void SetLanguagesList()
        {
            //Initialize varaibles for reading the languages folder. Reads every xml file there as its own language
            languages = new List<string>();
            DirectoryInfo d = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "languages", ""));
            FileInfo[] files = d.GetFiles("*.xml");

            try
            {
                //Go through each file and add it to the languages list.
                foreach (FileInfo file in files)
                    languages.Add(file.Name.Split('.')[0]);
            }
            catch
            {
                //Show an error box
                MessageBox.Show("A file in the 'languages' folder is invalid because it doesn't contain a file extension.", "Error");
            }
        }

        /// <summary>
        /// Formats a resource/string into one that can insert "vars" into placeholders like {0}.
        /// </summary>
        /// <param name="RSource">The specific R resource</param>
        /// <param name="vars">Variable elipsis</param>
        /// <returns>A formatted string with values where they should be inside of it</returns>
        public static string Format(dynamic RSource, params string[] vars)
        {
            return String.Format((string)RSource, vars);
        }

        /// <summary>
        /// Formats a resource/string into one that can insert "vars" into placeholders like {0}.
        /// Also performs a replace string function on the RSource
        /// </summary>
        /// <param name="RSource">The specific R resource</param>
        /// <param name="replace">Replace param 1</param>
        /// <param name="replaceWith">Replace param 2</param>
        /// <param name="vars">Variable elipsis</param>
        /// <returns>A formatted string with values where they should be inside of it</returns>
        public static string FormatReplace(dynamic RSource, string replace, string replaceWith, params string[] vars)
        {
            return String.Format((string)RSource.Replace(replace, replaceWith), vars);
        }

        /// <summary>
        /// Checks if the selected language was translated with machine learning or not.
        /// </summary>
        /// <returns>Whether it was translated with a machine or not</returns>
        public static bool CheckLanguageMachineTranslated()
        {
            return MachineTranslated;
        }

        /// <summary>
        /// Initializes an event handler for binder exceptions.
        /// A global exception handler for when Lang's R resource is missing (A <word> has been removed or is corrupted in one of the XMLs)
        /// </summary>
        public static void InitRuntimeBinderExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += BinderExceptionHandler;
        }

        
        /// <summary>
        /// Sets the culture of the app. This should change the language that things like an error message caused by an exception would show.
        /// </summary>
        public static void SetCulture()
        {
            //TODO Clean up
            // Load user's language choice (e.g., from a file)
            //string userLanguage = LoadUserLanguageChoice();

            // Set the UI culture based on the user's choice
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLanguageCode);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLanguageCode);
        }

        /// <summary>
        /// (Error handling)
        /// The event handler itself for the exception that's used by InitRuntimeBinderExceptionHandler() function
        /// </summary>
        /// <param name="sender">Object of what sent it</param>
        /// <param name="e">Event of what sent it</param>
        private static void BinderExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            //Event handler for RuntimeBinderExceptions. In this app, that only means the R variable. Only one that's a dynamic and could throw this exception.
            if (e.ExceptionObject is RuntimeBinderException binderException)
            {
                //Error handling stuff. To be sure that currentLang is read as empty
                string clang = Common.ReadNullString(currentLanguage);

                //Send an error message. Yes, this has to default to English since the current language is corrupt and can't be trusted to work
                //                       I know a way for this to work even if i did get the xml <word>, but i'd be afraid of recursions to this event handler if i did. So let's not.
                MessageBox.Show($"Something is wrong with the xml of the current language ({clang})\n" +
                                $"Most likely, the XML is missing a <word> or has some other corruption in it.\n" +
                                $"The app will now change the language to the default (English). So you have to restart it. Sorry about that.", "Error");

                //Change language to English before the application terminates (Due to the exception that was thrown)
                Properties.Settings.Default.Language = "English";
                Properties.Settings.Default.Save();
            }
        }
    }
}
