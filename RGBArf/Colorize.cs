using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBArf
{
    static class Colorize
    {
        public static string Rainbow(string str, ColorParameters cparam)
        {
            // adapted from https://krazydad.com/tutorials/makecolors.php

            string rainbowString = "";

            float phase = (cparam.EnablePhaseShift ? cparam.Phase * cparam.PhaseShift : cparam.Phase);
            int center = cparam.Center;
            int width = cparam.Width;
            double frequency = Math.PI * 2 / cparam.LineWidth;
            for (var i = 0; i < str.Length; ++i)
            {
                double red = Math.Sin(frequency * i + 2 + phase) * width + center;
                double green = Math.Sin(frequency * i + 0 + phase) * width + center;
                double blue = Math.Sin(frequency * i + 4 + phase) * width + center;

                rainbowString += "\u001b[38;2;" + (int)red + ";" + (int)green + ";" + (int)blue + "m" + str[i];
            }

            cparam.PhaseShift++;

            return rainbowString;
        }

        public static void SetColorPalleteRange(int option, ColorParameters cparam)
        {
            switch (option)
            {
                case 1: //light pastel
                    cparam.Center = 230;
                    cparam.Width = 25;
                    break;
                case 2: // dark pastel
                    cparam.Center = 200;
                    cparam.Width = 55;
                    break;
                default:
                    break;
                
            }

        }
    }

    class ColorParameters
    {
        public float Phase { get; set; } = 0.5F;
        public bool EnablePhaseShift { get; set; } = true;
        public int PhaseShift { get; set; } = 1;
        public int Center { get; set; } = 128;
        public int Width { get; set; } = 127;
        public int LineWidth { get; set; } = 80;
    }
}
