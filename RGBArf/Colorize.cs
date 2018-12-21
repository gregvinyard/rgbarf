using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBArf
{
    class Colorize
    {
        public static string Rainbow(string str, float phase = 0F)
        {
            // adapted from https://krazydad.com/tutorials/makecolors.php

            string rainbowString = "";

            int center = 128;
            int width = 127;
            double frequency = Math.PI * 2 / 80; // str.Length;
            for (var i = 0; i < str.Length; ++i)
            {
                double red = Math.Sin(frequency * i + 2 + phase) * width + center;
                double green = Math.Sin(frequency * i + 0 + phase) * width + center;
                double blue = Math.Sin(frequency * i + 4 + phase) * width + center;

                rainbowString += "\u001b[38;2;" + (int)red + ";" + (int)green + ";" + (int)blue + "m" + str[i];
            }

            return rainbowString;
        }
    }
}
