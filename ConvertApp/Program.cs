using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

/***
 * Copyright OnThisPhone 2024
 * Use this application and source code as you wish.
 * Just give me attribution. Enjoy.
 * Uses .NET 4.7.2
 * Designed in about a week's time.
 * HELLO, LEXICON!
 * 
 * Intended to be portable. Hence the smol window and the fact that the registry isn't placed at install.
 * 
 * ***********
 * 
 * TODO Add a proper app icon in the properties for the project.
 * TODO Add functionality to some convert parameters in the options menu.
 * TODO Parameters from settings (Like rotation and what not) when converting through exe parameters.
 * TODO Make so that clearing console invalidates the rect that draws the border around the console section of the form.
 * TODO Clean up the MyFfmpeg code (The bottom of it)
 * TODO The console still blits "completed" and "100%" if the conversion gets stopped.
 * TODO ffmpeg override string in options menu.
 * TODO The binder exception in the lang class only fires if the app is in debug.
 * TODO Test for conversions done on paths that contains . (App might not be able to process those properly. Didn't think that paths could have that character in them. Aside from the file extention or file name, that is)
 *      App might be able to run properly anyway, but it's worth checking.
 * TODO There are a few useless event handler functions lying about that needs cleaning.
 * 
 * NOTE Ideally, the app should just set all the lang values from the English file and default to that
 *      so that if a translation is missing from the set language, it will just use the English word. You know, instead of throwing an error.
 *      Not sure if i'll add this, but it's worth noting as a safer and better implementation
 * NOTE Got a little spaghetti when all the conversion functions were made in the Form1 file. Oh well.
 *      
 * NOTE The app could do with some better error handling with ffmpeg itself.
 *      The MyFfmpeg class has a HandleOutputData where you can start doing that.
 *      
 * Translate stuff in the "Will perform" thing for conversion.
	Translate in MyFfmpeg too. New functions there.
        Translate a bunch of classes. Make sure to go through all of it.
	Download FFMpeg.exe in options menu
	Some new stuff in the constructor in Form1.cs too
	Integration menu has some translations too
	SystemIntegration class
            Clean up in the BtnConvert switch case in the Form1.cs file.
            Clean the end of the MyFfmpeg file. HandleOutputData is dirty.
            Clean up and comment on MyPaint
            Make so that the console actually applies the proper font.

 * NOTE If conversion gets done from the SystemIntegration, it will throw some weird warning messages in the console
 *      The parameters in when hovering on the convert button also act a bit strange then.

    
    DEBUG EXE PARAMS (These can be changed by going to Right click ConvertApp/Project name -> Properties -> Debug:
        -context debugprojectproperties -ffmpeg -y -i \"C:\Users\Jojo\Music\Wait for Sleep.avi\" -ffmpegend -ir true -oc false -op false

 */

namespace ConvertApp
{
    internal static class Program
    {
        //Used to refer to the main form statically. As a reference.
        public static Form mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Set a global exception handler for when Lang's R resource is missing (A <word> has been removed or is corrupted in one of the XMLs)
            Lang.InitRuntimeBinderExceptionHandler();

            //Read the language setting string and initialize an error message
            //And yes, it defaults to English, seeing as the language cannot possibly be set at this point.
            string lang = Properties.Settings.Default.Language;
            string strangeError =
                    $"A language XML file that was previously present and selected" +
                    $" ({Properties.Settings.Default.Language}) is no longer in the languages folder." +
                    $"\nThe app will now default to English and if this issue persists, it means the English.xml is" +
                    $" gone too and needs to be redownloaded. The app won't start without it.";

            //If the variable in the settings is not initialized, default to English
            if (lang == "" || lang == null)
            {
                lang = "English";//Default the language to English.
            }

            //Apply what language to use
            try
            {
                //Tries to initialize the language
                Lang.Init(lang);
            }
            catch
            {
                //Show an error message if something goes wrong during initialization. 
                MessageBox.Show(strangeError, "Error");
                
                //Try again, but make the app select English.
                Properties.Settings.Default.Language = "English";
                Lang.Init("English");
            }

            //Parse app parameters. Determines what will happen next
            SystemIntegration.ParseArgs(args);

            //Only do following actions if the app isn't run with the insta run parameter. Run it if it it's not active though.
            //This will prevent a settings file from being created in the path where a right click context menu conversion happened.
            if (!SystemIntegration.InstantRun || !SystemIntegration.Active)
            {
                //Load custom storage. Used alongside the .NET settings/storage functionality.
                AppStorage.Update();

                //Check if exe location of the app has changed. Good for knowing if the convert option will work in Windows Explorer.
                AppIntegrity.CheckExeLocation();

                //Check application resources, assets, images, whatever. If they exist or not.
                AppIntegrity.ValidateFileLocations();
            }

            //Initialize and open the main form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new FrmMain();
            Application.Run(mainForm);
        }
    }
}
