using ArabizeCore.Managers;
using CliFramework;
using System;

namespace ArabizeCore
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ArabizeCoreFileManager fileManager = new();
            CommandManager commandManager = new(fileManager);
            Repl repl = new();
            repl.AddCommand(
                args => args.Length == 1 && (args[0].Equals("macros") || args[0].Equals("m")),
                commandManager.ListMacros,
                "macros (m)",
                "List all macros with their key-value mappings."
            );
            repl.AddCommand(
                args => args.Length == 1 && (args[0].Equals("letters") || args[0].Equals("l")),
                commandManager.ListLetters,
                "letters (l)",
                "List all transliterated letters and their Unicode mappings."
            );
            repl.AddCommand(
                args => args.Length == 1 && (args[0].Equals("diacritics") || args[0].Equals("d")),
                commandManager.ListDiacritics,
                "diacritics (d)",
                "List all transliterated diacritics and their Unicode mappings."
            );
            repl.AddCommand(
                args => args.Length == 3 && (args[0].Equals("add") || args[0].Equals("a")),
                commandManager.AddMacro,
                "add (a) [key] [value]",
                "Add a new macro where the value is Arabized."
            );
            repl.AddCommand(
                args => args.Length == 3 && (args[0].Equals("add-lit") || args[0].Equals("al")),
                commandManager.AddMacroLiteral,
                "add-lit (al) [key] [value]",
                "Add a new macro where the value is literal."
            );
            repl.AddCommand(
                args => args.Length == 2 && (args[0].Equals("remove") || args[0].Equals("rm") || args[0].Equals("r")),
                commandManager.RemoveMacro,
                "remove (rm) [key]",
                "Remove an existing macro."
            );
            repl.AddCommand(
                args => args.Length == 1 && (args[0].Equals("open") || args[0].Equals("o")),
                commandManager.OpenSettings,
                "open (o)",
                "Open the settings JSON file."
            );
            repl.AddCommand(
                args => args.Length == 2 && (args[0].Equals("open") || args[0].Equals("o")) && (args[1].Equals("macros") || args[1].Equals("m")),
                commandManager.OpenMacros,
                "open macros",
                "Open the macros JSON file."
            );
            repl.AddCommand(
                args => args.Length > 0,
                commandManager.Arabize,
                "[text]",
                "Arabize [text]."
            );
            repl.Run(args, true);
        }
    }
}