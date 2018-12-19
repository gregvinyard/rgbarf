using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RGBArf
{
    class Program
    {
        // from tomorz "Enable VT100 for the current console window from .NET Core"
        // https://gist.github.com/tomzorz/6142d69852f831fb5393654c90a1f22e

        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        static string Rainbow(string str, int phase = 0)
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


        static void Main(string[] args)
        {
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                return;
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                Console.ReadKey();
                return;
            }

            string s;
            int phase = 0;
            while ((s = Console.ReadLine()) != null)
            {
                Console.Write(Rainbow(s, phase));
                Console.WriteLine("\u001b[0m"); // set color back to normal
                phase++;
            }
            
        }
    }

}
