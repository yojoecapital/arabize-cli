using ArabizeCore.Utilities;
using CliFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ArabizeCore.Managers
{
    internal class CommandManager
    {
        private readonly ArabizeCoreFileManager fileManager;

        public CommandManager(ArabizeCoreFileManager fileManager) =>
            this.fileManager = fileManager;

        public void ListMacros(string[] args)
        {
            var macros = fileManager.Macros;
            if (macros != null)
            {
                if (args.Length == 1) PrettyConsole.PrintPagedListOneLine(macros, fileManager.Settings.macrosPerPage);
                else
                {
                    string sortBy;
                    IEnumerable<KeyValuePair<string, string>> sorted;
                    if (args[1].Equals("by-key"))
                    {
                        sortBy = string.Join(" ", args.Skip(2));
                        sorted = FileManager.SortDictionaryByKeyText(macros, sortBy);
                    }
                    else if (args[1].Equals("by-value"))
                    {
                        // FIXME arabize
                        sortBy = string.Join(" ", args.Skip(2));
                        sorted = FileManager.SortDictionaryByValueText(macros, sortBy);
                    }
                    else
                    {
                        sortBy = string.Join(" ", args.Skip(1));
                        sorted = FileManager.SortDictionaryByKeyText(macros, sortBy);
                    }
                    PrettyConsole.PrintPagedListOneLine(sorted, fileManager.Settings.macrosPerPage, header: "Sorting by: \"" + sortBy + "\"");
                }
            }
        }

        public void ListLetters(string[] _)
        {
            var letters = fileManager.Letters;
            if (letters != null) PrettyConsole.PrintList(letters);
        }

        public void ListDiacritics(string[] _)
        {
            var diacritics = fileManager.Diacritics;
            if (diacritics != null) PrettyConsole.PrintList(diacritics);
        }

        public string Arabize(string transliteration)
        {
            var mapping = fileManager.Letters;
            var macros = fileManager.Macros;
            var diacritics = fileManager.Diacritics;
            if (mapping != null && macros != null && diacritics != null)
            {
                macros.ToList().ForEach(macro => mapping[macro.Key] = macro.Value);
                var diacriticsKeys = diacritics.Keys;
                var arabic = new List<string>();
                var words = transliteration.Split(' ');
                foreach (var word in words)
                {
                    var letters = word.Split('_');
                    var arabicWord = string.Empty;
                    foreach (var letter in letters) foreach (var splitLetter in Helpers.SplitWithDelimiters(letter, diacriticsKeys))
                    {
                        var key = Helpers.FindClosestKey(mapping, Helpers.TrimForDiacritic(splitLetter, diacritics, out string diacritic));
                        if (mapping.ContainsKey(key))
                            arabicWord += mapping[key] + diacritic;
                    }
                    arabic.Add(arabicWord);
                }
                return string.Join(" ", arabic);
            }
            else return string.Empty;
        }


        public void AddMacro(string[] args)
        {
            var arabic = Arabize(args[2]);
            if (string.IsNullOrEmpty(arabic))
                PrettyConsole.PrintError("Could not add value.");
            else AddMacroLiteral(args[1], arabic);
        }

        private void AddMacroLiteral(string key, string value)
        {
            var diacritics = fileManager.Diacritics;
            if (diacritics != null)
            {
                if (Helpers.ContainsDelimiter(key, diacritics, out string badSubstring))
                    PrettyConsole.PrintError("Key should not contain the delimiter \"" + badSubstring + "\".");
                else if (fileManager.AddMacro(key, value))
                {
                    Clipboard.SetText(value);
                    PrettyConsole.PrintKeyValue(key, value);
                }
                else PrettyConsole.PrintError("Could not add key.");
            }
        }

        public void AddMacroLiteral(string[] args) =>
            AddMacroLiteral(args[1], args[2]);

        public void RemoveMacro(string[] args)
        {
            var key = args[1];
            var value = fileManager.RemoveMacro(key);
            if (value != null) PrettyConsole.PrintKeyValue(key, value, '\u2260');
            else PrettyConsole.PrintError("Could not remove key.");
        }

        public void OpenSettings(string[] _)
        {
            var path = ArabizeCoreFileManager.SettingsFilePath;
            ProcessStartInfo psi = new()
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(psi);
            Console.WriteLine(path);
        }

        public void OpenMacros(string[] _)
        {
            var path = fileManager.MacrosFilePath;
            ProcessStartInfo psi = new()
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(psi);
            Console.WriteLine(path);
        }

        public void Arabize(string[] args)
        {
            var arabic = Arabize(string.Join(" ", args));
            if (!string.IsNullOrEmpty(arabic))
            {
                Clipboard.SetText(arabic);
                Console.WriteLine(arabic);
            }
        }
    }
}