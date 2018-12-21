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
        static ColorParameters cparam = new ColorParameters();

        static void writeConsoleOutput()
        {
            string s;
            while ((s = Console.ReadLine()) != null)
            {
                Console.Write(Colorize.Rainbow(s, cparam));
                Console.WriteLine("\u001b[0m"); // set color back to normal
            }
            return;
        }

        static void ParseArgs(string[] args)
        {
            bool show_help = false;

            var p = new OptionSet()
            {
                { "h|help",  "Show this message and exit",
                    v => show_help = v != null },
                { "p=|pallete=", "Choose color pallete range\n" +
                                "1 - Light Pastel\n" +
                                "2 - Dark Pastel",
                    (int v) => Colorize.SetColorPalleteRange(v, cparam)},
            };

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

            return;
        }

        static void Main(string[] args)
        {
            // Sets up VT100 console and exits with an error code if not supported
            ConsoleEx.InitConsole();

            // Parse and handle command line parameters
            ParseArgs(args);

            // If piped inout is detected, process it and write it
            if (ConsoleEx.IsInputRedirected)
            {
                writeConsoleOutput();
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
