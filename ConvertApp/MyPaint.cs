using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

/***
 * 
 */
namespace ConvertApp
{
    internal class MyPaint
    {
        public class Segment
        {
            public string Text { get; set; }
            public Color Color { get; set; }
            public bool newLine { get; set; }
        }

        public List<Segment> FormatTooltip(string input)
        {
            List<Segment> segments = new List<Segment>();

            // Regular expression to match color codes like %RED or %WHITE
            Regex colorRegex = new Regex(@"%(\w+)");

            // Split the input string based on color codes
            string[] parts = colorRegex.Split(input);
            parts = input.Split('\n');

            // Default color
            Color currentColor = Color.Black;

            //Makes sure that it doesn't add a new line on the first line.
            bool firstNewLine = false;

            // Process each part and create segments
            foreach (string part in parts)
            {
                /*if (colorRegex.IsMatch(part))
                {
                    // If the part matches the color code pattern, try to parse the color code
                    string colorCode = colorRegex.Match(part).Groups[1].Value.ToUpper();
                    currentColor = ParseColorCode(colorCode);
                }
                else
                {*/

                //Apply some colors
                //Give the first line named "Will perform:" a nice orange color
                if(!firstNewLine)
                    currentColor = Color.OrangeRed;
                if(part.Contains("VAL"))
                    currentColor = Color.Purple;


                //A normal adding of a segment.
                if (!part.Contains("<=") && !part.Contains("\""))
                    segments.Add(new Segment { Text = part, Color = currentColor, newLine = firstNewLine });
                else if(part.Contains("<="))
                {
                    //Newline split.
                    //This splits the explanation and the actual statement and changes the color of the explanation to gray.
                    string[] nlsplit = part.Split('<');
                    segments.Add(new Segment { Text = nlsplit[0], Color = currentColor, newLine = true });
                    segments.Add(new Segment { Text = "<" + nlsplit[1], Color = Color.Silver, newLine = false });
                }
                else if (part.Contains("\""))
                {
                    //Newline split.
                    //Makes comments colored.
                    string[] nlsplit = part.Split('"');
                    segments.Add(new Segment { Text = nlsplit[0], Color = currentColor, newLine = true });
                    segments.Add(new Segment { Text = "\"" + nlsplit[1] + "\"", Color = Color.Orange, newLine = false });
                }

                firstNewLine = true;

                //Set default color each time.
                currentColor = Color.Black;
                //}
            }

            return segments;
        }

        private Color ParseColorCode(string colorCode)
        {
            // Map color codes to actual colors (you can extend this)
            switch (colorCode)
            {
                case "RED":
                    return Color.Red;
                case "PURPLE":
                    return Color.Purple;
                // Add more color codes as needed
                default:
                    return Color.Black; // Default color
            }
        }
    }
}
