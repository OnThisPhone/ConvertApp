using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertApp
{
    internal class ConvertConsole
    {
        public static bool autoScroll = true;       //Determines if the auto scroll when the console reaches the bottom should be on or not.
        public static ConvertConsole CConsole;      //Gets initialized in the Form1 class.
        string content;                             //The entire history as one long string.
        object view;                                //The control that was used to put text into
        object parentView;                          //The control's parent. For scrollbar handling. This can be null. Only supports Panel for now.

        //Constructors
        public ConvertConsole(object view)
        {
            content = "";
            this.view = view;
        }
        public ConvertConsole(object view, object parentView)
        {
            content = "";
            this.view = view;
            this.parentView = parentView;
        }

        /// <summary>
        /// Sets the auto scroll static variable.
        /// </summary>
        /// <param name="val">The new value for it</param>
        public void SetAutoscroll(bool val)
        {
            autoScroll = val;
        }

        /// <summary>
        /// Toggles the auto scroll. If it's on, it gets turned off and if it's off it gets turned on.
        /// </summary>
        public void ToggleAutoScroll()
        {
            autoScroll = !autoScroll;   //This should be a static method, no?
        }

        /// <summary>
        /// Sets the text size of the console's font. Leave the parameter empty and it uses the Properties.Settings.Default.ConsoleTextSize
        /// </summary>
        public void SetTextSize()
        {
            if(Properties.Settings.Default.ConsoleTextSize != "" && Properties.Settings.Default.ConsoleTextSize != null)
                SetTextSize(Common.ParseFontSizeFromSettings());
        }
        public void SetTextSize(float size)
        {
            ((Label)view).Font = Common.LoadEmbededFont("assets.fonts.SplineSansMono.ttf", size);
        }

        /// <summary>
        /// Clears the content of the console.
        /// </summary>
        public void Clear()
        {
            content = "";
            if (view != null)
            {
                if (view.GetType() == typeof(Label))
                {
                    //Invoke is to make sure that if this is called from a different thread, it'll work anyway.
                    try
                    {
                        if (((Label)view).InvokeRequired)
                            ((Label)view).Invoke(new Action(() => ((Label)view).Text = ""));
                        else
                            ((Label)view).Text = "";
                    }
                    catch(Exception e)
                    {
                        Debug.Print(e.ToString());
                    }

                    //Invalidate the control/view/form element. Appears to fix the strange invalid argument bug.
                    ((Label)view).Invalidate();
                }
            }
        }

        /// <summary>
        /// Same as WriteTo but it adds a new line at the end.
        /// </summary>
        /// <param name="input">Text to add</param>
        public void WriteLineTo(string input)
        {
            WriteTo(input + "\n");
        }
        /// <summary>
        /// Writes to console to a supported "view"/"windows form"
        /// </summary>
        /// <param name="input">Text to add</param>
        public void WriteTo(string input)
        {
            //Add to the content of the console
            content += input;

            //Add the text. Should probably be a "Control" type to cover all of it. But this works
            //Invoke means if this is called from a different thread, it'll still work.
            if (((Label)view).InvokeRequired)
                ((Label)view).Invoke(new Action(() => ((Label)view).Text = content));
            else
                ((Label)view).Text = content;

            /*if (view.GetType() == typeof(Label))
                ((Label)view).Text = content;*/

            //Scroll to the bottom of the parent
            if(parentView != null && autoScroll)
            {
                if(parentView.GetType() == typeof(Panel))
                {
                    ((Panel)parentView).VerticalScroll.Value = ((Panel)parentView).VerticalScroll.Maximum;
                    ((Panel)parentView).Invalidate();
                }
            }
        }
    }
}
