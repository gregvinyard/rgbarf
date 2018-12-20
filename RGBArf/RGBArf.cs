using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NDesk.Options;

namespace RGBArf
{
    class RGBArf
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

        static string Rainbow(string str, float phase = 0F)
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

        static void writeConsoleOutput()
        {
            string s;
            float phaseShift = 0.5F;
            float phase = phaseShift;
            while ((s = Console.ReadLine()) != null)
            {
                Console.Write(Rainbow(s, phase));
                Console.WriteLine("\u001b[0m"); // set color back to normal
                phase += phaseShift;
            }
            return;
        }

        static void Main(string[] args)
        {
            bool show_help = false;

            var p = new OptionSet()
            {
                { "h|help",  "show this message and exit",
                  v => show_help = v != null },
            };

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

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("rgbarf: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `rgbarf --help' for more information.");
                return;
            }

            if (show_help)
            {
                ShowHelp(p);
                return;
            }

            // bool inputRedirected = ConsoleEx.IsInputRedirected;

            if (ConsoleEx.IsInputRedirected)
            {
                writeConsoleOutput();
            }
            else
            {
                ShowHelp(p);
                return;
            }
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Make console applications more fun and colorful");
            Console.WriteLine();
            Console.WriteLine("Usage: [console application] | RGBARF");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }

}
