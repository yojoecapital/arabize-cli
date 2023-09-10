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
            Repl repl = new()
            {
                pagifyHelp = fileManager.Settings.macrosPerPage
            };
            repl.AddCommand(
                args => args.Length == 1 && (args[0].Equals("macros") || args[0].Equals("m")),
                commandManager.ListMacros
            );
            repl.AddCommand(
                args => args.Length == 1 && (args[0].Equals("letters") || args[0].Equals("l")),
                commandManager.ListLetters
            );
            repl.AddCommand(
                args => args.Length == 1 && (args[0].Equals("diacritics") || args[0].Equals("d")),
                commandManager.ListDiacritics
            );
            repl.AddCommand(
                args => args.Length == 3 && (args[0].Equals("add") || args[0].Equals("a")),
                commandManager.AddMacro
            );
            repl.AddCommand(
                args => args.Length == 3 && (args[0].Equals("add-lit") || args[0].Equals("al")),
                commandManager.AddMacroLiteral
            );
            repl.AddCommand(
                args => args.Length == 2 && (args[0].Equals("remove") || args[0].Equals("rm") || args[0].Equals("r")),
                commandManager.RemoveMacro
            );
            repl.AddCommand(
                args => args.Length == 1 && (args[0].Equals("open") || args[0].Equals("o")),
                commandManager.OpenSettings
            );
            repl.AddCommand(
                args => args.Length == 2 && (args[0].Equals("open") || args[0].Equals("o")) && (args[1].Equals("macros") || args[1].Equals("m")),
                commandManager.OpenMacros
            );
            repl.AddCommand(
                _ => true,
                commandManager.Arabize
            );
            repl.Run(args, true);
        }
    }
}