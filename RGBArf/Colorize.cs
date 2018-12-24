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
            // Returns a string with VT100 color codes added.
            // Color spectrum is generated using parameters stored in ColorParameters object
            // adapted from https://krazydad.com/tutorials/makecolors.php

            string rainbowString = "";

            float phaseOffset;
            if (cparam.EnablePhaseShift)
            {
                phaseOffset = cparam.PhaseOffset * cparam.PhaseShift;
                cparam.PhaseShift++;
            }
            else
            {
                phaseOffset = cparam.PhaseOffset;
            }
                    
            for (var i = 0; i < str.Length; ++i)
            {
                double red = Math.Sin(cparam.FrequencyRed * i + cparam.PhaseRed + phaseOffset) * cparam.Width + cparam.Center;
                double green = Math.Sin(cparam.FrequencyGreen * i + cparam.PhaseGreen + phaseOffset) * cparam.Width + cparam.Center;
                double blue = Math.Sin(cparam.FrequencyBlue * i + cparam.PhaseBlue + phaseOffset) * cparam.Width + cparam.Center;

                rainbowString += "\u001b[38;2;" + (int)red + ";" + (int)green + ";" + (int)blue + "m" + str[i];
            }

            return rainbowString;
        }

        public static void SetColorPalleteRange(int option, ColorParameters cparam)
        {
            switch (option)
            {
                case 1: //light pastel
                    ColorParameterPresets.LightPastel(cparam);
                    break;
                case 2: // dark pastel
                    ColorParameterPresets.DarkPastel(cparam);
                    break;
                case 3:
                    ColorParameterPresets.LightGray(cparam);
                    break;
                default:
                    Console.WriteLine("Invalid pallete selection.");
                    break;
            }

        }
    }

    class ColorParameters
    {
        public int LineWidth { get; set; } = 80;
        public float FrequencyRed { get; set; }
        public float FrequencyGreen { get; set; }
        public float FrequencyBlue { get; set; }
        public float PhaseRed { get; set; } = 2F;
        public float PhaseGreen { get; set; } = 0F;
        public float PhaseBlue { get; set; } = 4F;
        public float PhaseOffset { get; set; } = 0.5F;
        public bool EnablePhaseShift { get; set; } = true;
        public int PhaseShift { get; set; } = 1;
        public int Center { get; set; } = 128;
        public int Width { get; set; } = 127;

        public ColorParameters()
        {
            FrequencyRed = (float)Math.PI * 2 / LineWidth;
            FrequencyGreen = (float)Math.PI * 2 / LineWidth;
            FrequencyBlue = (float)Math.PI * 2 / LineWidth;
        }
    }

    class ColorParameterPresets
    {
        public static void LightPastel(ColorParameters cparam)
        {
            cparam.Center = 230;
            cparam.Width = 25;
        }

        public static void DarkPastel(ColorParameters cparam)
        {
            cparam.Center = 200;
            cparam.Width = 55;
        }

        public static void LightGray(ColorParameters cparam)
        {
            cparam.Center = 200;
            cparam.Width = 55;
            cparam.PhaseRed = 0F;
            cparam.PhaseGreen = 0F;
            cparam.PhaseBlue = 0F;

        }
    }
}
