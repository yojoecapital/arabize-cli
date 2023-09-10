using ArabizeCore.Utilities;
using CliFramework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ArabizeCore.Managers
{
    internal class ArabizeCoreFileManager : FileManager
    {
        public static string LettersFilePath
        {
            get => GetFilePath("letters.xml");
        }

        private Dictionary<string, string> letters;
        public Dictionary<string, string> Letters
        {
            get
            {
                letters ??= GetDictionaryXml(LettersFilePath);
                if (letters == null) PrettyConsole.PrintError("Could not parse letters XML file.");
                return letters;
            }
        }

        public static string DiacriticsFilePath
        {
            get => GetFilePath("diacritics.xml");
        }

        private Dictionary<string, string> diacritics;
        public Dictionary<string, string> Diacritics
        {
            get
            {
                diacritics ??= GetDictionaryXml(DiacriticsFilePath);
                if (diacritics == null) PrettyConsole.PrintError("Could not parse diacritics XML file.");
                return diacritics;
            }
        }

        public static string SettingsFilePath
        {
            get => GetDictionaryFilePath("settings.json");
        }

        private Settings settings;
        public Settings Settings
        {
            get
            {
                settings ??= GetObject<Settings>(SettingsFilePath);
                if (settings == null)
                {
                    PrettyConsole.PrintError("Could not parse settings JSON file.");
                    return null;
                }
                else return settings;
            }
            set
            {
                settings = value;
                SetObject(SettingsFilePath, settings, Formatting.Indented);
            }
        }

        public string MacrosFilePath
        {
            get
            {
                var settings = Settings;
                if (settings != null && !string.IsNullOrEmpty(settings.macrosPath))
                    return GetDictionaryFilePath(settings.macrosPath) ?? GetDictionaryFilePath("macros.json");
                else return GetDictionaryFilePath("macros.json");
            }
        }

        private Dictionary<string, string> macros;
        public Dictionary<string, string> Macros
        {
            get
            {
                macros ??= GetDictionary(MacrosFilePath);
                if (macros == null) PrettyConsole.PrintError("Could not parse macros JSON file.");
                return macros;
            }
            private set
            {
                macros = value;
                SetDictionary(MacrosFilePath, macros);
            }
        }

        public bool AddMacro(string key, string value)
        {
            var macros = Macros;
            var letters = Letters;
            if (macros != null && letters != null)
            {
                if (macros.ContainsKey(key) || letters.ContainsKey(key)) 
                    return false;
                macros[key] = value;
                Macros = macros;
                return true;
            }
            else return false;
        }

        public string RemoveMacro(string key)
        {
            var macros = Macros;
            if (macros != null && macros.ContainsKey(key))
            {
                var value = macros[key];
                macros.Remove(key);
                Macros = macros;
                return value;
            }
            else return null;
        }
    }
}
