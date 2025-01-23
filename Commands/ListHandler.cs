using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ArabizeCli.Arguments;

namespace ArabizeCli.Commands
{
    public static class ListHandler
    {
        public static void Handle(ListOption option)
        {
            switch (option)
            {
                case ListOption.M:
                    var macrosPath = Path.Join(Defaults.configurationPath, Defaults.macrosFileName);
                    if (!File.Exists(macrosPath)) return;
                    var macros = JsonContext.Get(macrosPath);
                    foreach (var macro in macros)
                    {
                        Console.WriteLine($"{macro.Key} → {macros.Values}");
                    }
                    return;
                case ListOption.D:
                    foreach (var diacritic in Defaults.diacritics)
                    {
                        Console.WriteLine($"{diacritic.Key} → {diacritic.Value}");
                    }
                    return;
                case ListOption.L:
                    foreach (var letter in Defaults.letters)
                    {
                        Console.WriteLine($"{letter.Key} → {letter.Value}");
                    }
                    return;
            }
        }
    }
}