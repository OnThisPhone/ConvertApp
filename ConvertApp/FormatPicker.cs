using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertApp
{
    public partial class FormatPicker : Form
    {
        public static string FormatPicked = "";
        public FormatPicker()
        {
            InitializeComponent();

            //Set default format for both the combo box and the variable
            CmbPicker.Text = MyFfmpeg.CompatibleFormats[0];
            FormatPicked = MyFfmpeg.CompatibleFormats[0];

            //Set compatible formats as set in the MyFfmpeg class
            foreach (string entry in MyFfmpeg.CompatibleFormats)
                CmbPicker.Items.Add(entry);

            //Set default dialog result to cancel.
            DialogResult = DialogResult.Cancel;

            //Set the eventhandler/listener for the accept button. Accepts the result if the result is valid.
            BtnAccept.Click += new EventHandler((o, e) =>
            {
                string result = CmbPicker.Text;

                if(result != "")
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("No format selected.");
                }
            });

            SetControls();
        }

        //Sets control translations
        private void SetControls()
        {

        }

        //The eventhandler/listener for the picker combo box. Fires whenever the user changes it.
        private void CmbPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormatPicked = ((ComboBox)sender).Text;
        }
    }
}
