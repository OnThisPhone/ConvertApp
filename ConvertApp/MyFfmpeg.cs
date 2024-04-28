using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using ProgressBar = System.Windows.Forms.ProgressBar;

/***
 * Contains all the FFMPEG-specific functions
 * NOTE It's worth noting that there are three different ffmpeg parameters that are fetched by the app.
 *      1. The regular one that's set by the settings in the app (Bitrate, framerate, rotation, etc)
 *      2. The one that let's you override it for a custom ffmpeg string
 *      3. The one that happens if the user right clicks a file and converts it from there.
 */
namespace ConvertApp
{
    internal class MyFfmpeg
    {

        //Initialize some statics
        public static readonly string[] CompatibleFormats = { "mp3", "mp4", "avi", "mov", "wmv", "flv", "mpeg", "webm", "ogg" };
        public static volatile bool ThreadRunning = false;      //Indicates if the thread that Run uses is currently running
        public static Process ffmpegProcess = null;             //The process of the ffmpeg.exe
        private static string durationInfo = "";                //Used in HandleOutputData to read the first instance of "duration" in the FFMPEG output (Only encountered once). It's to determine how long the file is in time
        private static TimeSpan progressTime;                   //Used to determine how much the conversion has converted (In time). Used to determine percentage done the conversion is at.
        private static ProgressBar progressBar;                 //A progress bar control/form. Used to update the progress bar for the app. Gets initialized in the Init() function.
        private static Button convertButton;                    //The convert button for the program
        private static Form frm1Reference = null;
        public static readonly string ffmpegExePath =           //Path to the ffmpeg exe file
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg", "ffmpeg.exe");
        public static readonly string[] exeFallbackDownloads =  //Fallback urls for downloads (TODO Not implemented yet)
            {"https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip",
             "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip"};
        //public static MyFfmpegOptions options;                //Options class. Contains all the options available for convertion

        //Nab a function from one of the FFMPEG DLL files. May not be necessary, but it's nice to have it
        [DllImport("ffmpeg/avcodec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_version();

        /// <summary>
        /// Initializes the progress bar and other form controls.
        /// </summary>
        /// <param name="pb">Progress bar to use updating the conversion progress</param>
        /// <param name="cb">The button that says "convert". Since it also changes from convert to stop all the time</param>
        public static void Init(ProgressBar pb, Button cb, Form frm)
        {
            progressBar = pb;
            convertButton = cb;
            frm1Reference = frm;

            //options = new MyFfmpegOptions();
            MyFfmpegOptions.Init();
        }

        /// <summary>
        /// Gets the codec version of the avcodec in the DLL
        /// </summary>
        /// <returns></returns>
        public static int GetCodecVersion()
        {
            return avcodec_version();
        }

        /// <summary>
        /// Gets the version number of the ffmpeg exe file.
        /// Doesn't contain one, so this function effectively does nothing.
        /// </summary>
        /// <returns>Version string. Or N/A if file doesn't exist</returns>
        public static string GetExeVersion()
        {
            try
            {
                string ver = FileVersionInfo.GetVersionInfo(ffmpegExePath).FileVersion;
                return ver;
            }
            catch
            {
                return "N/A";
            }
        }

        /// <summary>
        /// Ask the user if they want to download it
        /// </summary>
        public static void PromptFFmpegDownload()
        {
            //Ask the user and store the result in a variable containing the choice
            DialogResult result;
            result = MessageBox.Show("Ffmpeg.exe must to be downloaded before this app can convert anything. Do you wish to do that now?", "Ffmpeg Required", MessageBoxButtons.YesNo);

            //Process the choices
            if(result == DialogResult.Yes)
            {
                DownloadFFmpeg();
            }
            else if(result == DialogResult.No)
            {
                MessageBox.Show("You can download ffmpeg.exe by going to the options menu or by manually placing it in the ffmpeg folder that's in the same folder as the app", "Ffmpeg Required");
            }
        }
        /// <summary>
        /// Downloads the FFmpeg exe file and puts it in the correct folder
        /// </summary>
        public static void DownloadFFmpeg()
        {
            //The location of the temp zip file and the zip object
            string zipFileUrl = "ffmpeg.zip";
            ZipArchive zipArchive = null;

            //Timer to limit updates when pinging the main thread. It causes some weird behavior otherwise.
            Timing timer = new Timing(300);

            //Set the url to the zip file
            Uri uriZip = new Uri(exeFallbackDownloads[0]);

            //Set the sync context in order to post to the main thread safely.
            var syncContext = SynchronizationContext.Current ?? new SynchronizationContext();

            //Write to console that it's downloading
            ConvertConsole.CConsole.WriteLineTo($"Downloading..\nFROM: {uriZip}");

            //Start the webclient
            using (var client = new WebClient())
            {
                //Set some listeners
                //Updates progress
                client.DownloadProgressChanged += (o, e) =>
                {
                    syncContext.Post(state =>
                    {
                        if (timer.Check())
                        {
                            //Get the percent (The percentage variable in the e event variable doesn't work)
                            //Claims the cast is unnecessary, but it ain't.
                            //Then prints it.
                            float percent = ((float)e.BytesReceived / (float)e.TotalBytesToReceive) * 100;
                            ConvertConsole.CConsole.WriteLineTo($"{percent:0}%");
                        }
                            
                    }, null);
                };

                //Fires when download is complete
                client.DownloadFileCompleted += (o, e) =>
                {
                    syncContext.Post(state =>
                    {
                        try
                        {
                            //Tell the user that the conversion was done successfully.
                            ConvertConsole.CConsole.WriteLineTo($"ffmpeg downloaded successfully");
                            ConvertConsole.CConsole.WriteLineTo($"Extracting archive...");

                            //Unpack the zip and place it where it should be
                            zipArchive = ZipFile.OpenRead(zipFileUrl);

                            //Search archive for the ffmpeg.exe file and unpack it where it should be
                            foreach(ZipArchiveEntry entry in zipArchive.Entries.Where(ev => ev.FullName.Contains("ffmpeg.exe")))
                            {
                                entry.ExtractToFile("ffmpeg/ffmpeg.exe");//entry.FullName
                            }

                            //Tell the user the action was completed
                            ConvertConsole.CConsole.WriteLineTo($"Archive extracted. Exe placed in ffmpeg folder\nDeleting archive...");

                            //Close streams
                            if (zipArchive != null)
                                zipArchive.Dispose();

                            //Delete the zip file
                            File.Delete(zipFileUrl);

                            //Tell the user the archive was deleted
                            ConvertConsole.CConsole.WriteLineTo($"Archive deleted\nApp is now ready to use!");
                        }
                        catch (Exception err)
                        {
                            ConvertConsole.CConsole.WriteLineTo($"An error occured:\n{err.Message}");

                            //Close streams
                            if (zipArchive != null)
                                zipArchive.Dispose();

                            //Delete the zip file
                            File.Delete(zipFileUrl);
                        }

                    }, null);
                };

                //Perform the download
                client.DownloadFileAsync(uriZip, zipFileUrl);
            }
        }

        /// <summary>
        /// Takes selected options from the options menu and adds them to the ffmpeg params.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string GetFFmpegArguments(string from, string to)
        {
            //If SystemIntegration parameters are set, do those instead of anything.
            if(SystemIntegration.ffmpegParameters != "")
            {
                return SystemIntegration.ffmpegParameters;//Note that input and output are set statically (Instead of dynamically with {0}{1}) and directly through the shortcut. By windows in the case for context menu changes.
            }

            //If custom parameters are active, do those for the entire thing. Otherwise, do parameters found in the options menu
            if (MyFfmpegOptions.customParametersActive)
            {
                return String.Format(MyFfmpegOptions.customParameters.Replace("\\\"", "\""), from, to);
            }
            else
            {
                //Get the parameters and formats them into an FFMPEG-compatible string.
                string formattedParams = String.Format(MyFfmpegOptions.defaultCustomParameters.Replace("\\\"", "\""), from, to);

                //Apply options strings
                formattedParams = MyFfmpegOptions.ApplyOptionsToFFmpegParams(formattedParams);

                //Returns with the full string with every option attached.
                return formattedParams;
            }
        }

        /// <summary>
        /// Terminates the conversion process. Exits ffmpeg completely when called.
        /// </summary>
        public static void Terminate()
        {
            //Kill process if it's running
            if (ffmpegProcess != null)
            {
                if (!ffmpegProcess.HasExited)
                    ffmpegProcess.Kill();
            }

            //Reset some parameters
            ThreadRunning = false;
            progressBar.Value = 0;
            convertButton.Text = Lang.R.Text.Convert;

            //Kill app if -ir was true and OverrideCLose is false
            if (SystemIntegration.InstantRun && !SystemIntegration.OverrideClose)
                Application.Exit();
        }

        /// <summary>
        /// Runs the conversion process.
        /// </summary>
        /// <param name="from">File to convert</param>
        /// <param name="to">Destination of file to convert</param>
        public static void Run(string from, string to)
        {
            //The path to the ffmpeg exe in the project
            string ffmpegPath = ffmpegExePath;

            //Check if the ffmpeg.exe exists. If not, throw an error message and leave the function
            if (!File.Exists(ffmpegPath))
            {
                MessageBox.Show(Lang.R.Text.FfmpegMissing.Replace(@"\n", "\n"), Lang.R.Text.Error);
                return;
            }

            //Arguments used for the exe file. FFMPEG arguments.
            //$"-i \"{inputFilePath}\" -vn -acodec libmp3lame -ab 192k \"{outputFilePath}\""
            //string ffmpegArguments = $"-y -i \"{from}\" \"{to}\"";
            string ffmpegArguments = GetFFmpegArguments(from, to);
            string ffmpegArgumentsUnparsed = GetFFmpegArguments("{0}", "{1}");

            //Set up the process start info
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = ffmpegArguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            //Set the convert button to say "stop"
            convertButton.Text = Lang.R.Text.Stop;

            //Initialize the main thread context that's used to post main thread functions (Like updating graphical comps)
            var syncContext = SynchronizationContext.Current ?? new SynchronizationContext();
            
            //Initialize the ffmpeg exe process
            ffmpegProcess = new Process { StartInfo = psi };
            
            //Set event handlers. Make sure to post those to the main thread
            //They get called each time the ffmpeg app updates its data.
            ffmpegProcess.OutputDataReceived += (sender, e) => syncContext.Post(state => { HandleOutputData(e.Data, syncContext); }, null);
            ffmpegProcess.ErrorDataReceived += (sender, e) => syncContext.Post(state => { HandleOutputData(e.Data, syncContext); }, null); ;

            //Write a string that shows the parameters used
            ConvertConsole.CConsole.WriteLineTo($"{Lang.R.Text.FFmpegParams}: {ffmpegArgumentsUnparsed}");

            //Write a warning/error string if any are found.
            ConvertConsole.CConsole.WriteLineTo(MyFfmpegOptions.WarnParameterErrors(ffmpegArgumentsUnparsed, false));
            ConvertConsole.CConsole.WriteLineTo("-----");

            //Start thread; run thread
            ThreadRunning = true;
            new Thread(() =>
            {
                //Start the FFmpeg process
                //Do exe process
                ffmpegProcess.Start();
                ffmpegProcess.BeginOutputReadLine();
                ffmpegProcess.BeginErrorReadLine();
                ffmpegProcess.WaitForExit();

                //Process done.
                ThreadRunning = false;
                ffmpegProcess = null;
                durationInfo = "";//Probably not needed.

                //Conversion completed. Post that to the main thread.
                syncContext.Post(state =>
                {
                    ConvertConsole.CConsole.WriteLineTo("100%");
                    ConvertConsole.CConsole.WriteLineTo(Lang.R.Text.ConversionCompleted);
                    progressBar.Value = 100;
                }, null);

                //This last part is just to make the transition with the progress bar to look a bit more elegant.
                Thread.Sleep(1000);
                syncContext.Post(state =>
                {
                    progressBar.Value = 0; 
                    convertButton.Text = Lang.R.Text.Convert;

                    Terminate();
                }, null);

            }).Start();

        }

        //Handles output data generated by FFMPEG. TODO Clean that shit up!
        static void HandleOutputData(string outputData, SynchronizationContext syncContext)
        {
            if (outputData != null)
            {
                //ConvertConsole.CConsole.WriteLineTo(outputData);

                //if (outputData.Contains("Duration:"))
                //{
                // Extract duration information from the output (you may need to customize this based on your FFmpeg version)

                if (outputData.Contains("time="))
                {
                    // Extract progress information from the output (you may need to customize this based on your FFmpeg version)
                    string progressInfo = outputData.Split(new[] { "time=" }, StringSplitOptions.None)[1].Trim().Split(' ')[0];

                    // Convert progress info to TimeSpan or other relevant format
                    if (TimeSpan.TryParse(progressInfo, out progressTime))
                    {
                        // Use progressTime as needed (e.g., print or update a progress bar)
                        //ConvertConsole.CConsole.WriteLineTo($"Progress: {progressTime}");
                    }
                }


                if (outputData.Contains("Duration:"))
                    durationInfo = outputData.Split(new[] { "Duration: " }, StringSplitOptions.None)[1].Split(',')[0].Trim();

                    //ConvertConsole.CConsole.WriteLineTo(durationInfo);

                // Parse duration string to TimeSpan
                if (TimeSpan.TryParse(durationInfo, out TimeSpan totalDuration))
                {
                    // Calculate progress based on elapsed time
                    //TimeSpan elapsedTime = DateTime.Now - startTime;
                    //double progress = elapsedTime.TotalMilliseconds;

                    // Calculate time remaining as a percentage
                    //double timeRemainingPercentage = 100 - (progress * 100);

                    // Display time remaining percentage
                    
                    long MilliDurationInfo = Common.ConvertTimeStringToMilliseconds(durationInfo);
                    float Percent = ((float)progressTime.TotalMilliseconds / (float)MilliDurationInfo) * 100;

                    //ConvertConsole.CConsole.WriteLineTo($"{durationInfo} - {progressTime}");

                    //Write and show percent
                    if (Percent > 0 && Percent < 99)
                    {
                        syncContext.Post(state =>
                        {
                            ConvertConsole.CConsole.WriteLineTo($"{Percent:F0}%");
                            progressBar.Value = (int)Percent;
                        }, null);
                    }
                }
            }
        }
    }
}
