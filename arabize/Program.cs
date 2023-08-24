using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace Arabize
{
    static class Program
    {
        static readonly string lettersFileName = "letters.xml";
        static readonly string diacriticsFileName = "diacritics.xml";
        static readonly string macrosFileName = "macros.json";

        static string LettersFilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, lettersFileName); }
        }

        static string DiacriticsFilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, diacriticsFileName); }
        }

        static string MacrosFilePath
        {
            get
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, macrosFileName);
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "{}");
                }
                return filePath;
            }
        }

        static Dictionary<string, string> Macros
        {
            get {
                string json = File.ReadAllText(MacrosFilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            set {
                string json = JsonConvert.SerializeObject(value);
                File.WriteAllText(MacrosFilePath, json);
            }
        }

        static Dictionary<string, string> Diacritics
        {
            get { return GetDictionary(DiacriticsFilePath); }
        }

        static Dictionary<string, string> Letters
        {
            get { return GetDictionary(LettersFilePath); }
        }

        static Dictionary<string, string> GetDictionary(string filePath)
        {
            try
            {
                return XDocument.Load(filePath).Root.Elements()
                    .ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value);
            }
            catch
            {
                return null;
            }
        }

        public static string FindClosestKey(Dictionary<string, string> mapping, string key)
        {
            int minDistance = int.MaxValue;
            string closestKey = null;

            foreach (var s in mapping.Keys)
            {
                int distance = ComputeLevenshteinDistance(s, key);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestKey = s;
                }
            }

            return closestKey;
        }

        public static int ComputeLevenshteinDistance(string s, string t)
        {
            int m = s.Length;
            int n = t.Length;
            int[,] d = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
            {
                d[i, 0] = i;
            }

            for (int j = 0; j <= n; j++)
            {
                d[0, j] = j;
            }

            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= m; i++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[m, n];
        }

        static bool AddMacro(string key, string value)
        {
            var mapping = Macros;

            // Check if key already exists in XML
            if (mapping.ContainsKey(key))
                return false;
            else if (XDocument.Load(LettersFilePath).Root.Elements().Any(row => row.Attribute("key").Value == key))
                return false;

            mapping[key] = value.Trim();

            Macros = mapping;
            return true;
        }

        public static string RemoveMacro(string key)
        {
            var mapping = Macros;

            if (mapping.ContainsKey(key))
            {
                var value = mapping[key];
                mapping.Remove(key);
                Macros = mapping;
                return value;
            }
            else return null;
        }

        static string TrimForDiacritic(string letter, out string diacritic)
        {
            var diacritics = Diacritics;
            diacritic = null;
            foreach (var key in diacritics.Keys)
            {
                if (letter.EndsWith(key))
                {
                    diacritic = diacritics[key];
                    if (!letter.EndsWith(key)) return letter;
                    else return letter.Remove(letter.LastIndexOf(key));
                }
            }
            return letter;
        }

        static int IndexOfFirstDelimiters(string input, IEnumerable<string> delimiters)
        {
            int start = 0;
            while (start < input.Length)
            {
                foreach (string delimiter in delimiters)
                {
                    if (input.Substring(start).StartsWith(delimiter))
                        return start + delimiter.Length;
                }
                start++;
            }
            return -1;
        }

        static IEnumerable<string> SplitWithDelimiters(string input, IEnumerable<string> delimiters)
        {
            int split;
            while ((split = IndexOfFirstDelimiters(input, delimiters)) != -1)
            {
                yield return input.Substring(0, split);
                input = input.Substring(split);
            }
            if (!string.IsNullOrEmpty(input)) yield return input;
        }

        static string Arabize(string transliteration)
        {
            Dictionary<string, string> mapping;
            try
            {
                mapping = Letters;
                var macros = Macros;
                macros.ToList().ForEach(x => mapping.Add(x.Key, x.Value));
            }
            catch
            {
                return null;
            }
            var diacriticsKeys = Diacritics.Keys;
            var arabic = new List<string>();
            var words = transliteration.Split(' ');
            foreach (var word in words)
            {
                var letters = word.Split('_');
                var arabicWord = string.Empty;
                foreach (var letter in letters) foreach (var splitLetter in SplitWithDelimiters(letter, diacriticsKeys))
                    {
                        var key = FindClosestKey(mapping, TrimForDiacritic(splitLetter, out string diacritic));
                        if (mapping.ContainsKey(key))
                        {
                            arabicWord += mapping[key] + diacritic;
                        }
                    }
                arabic.Add(arabicWord);
            }

            return string.Join(" ", arabic);
        }

        static bool ContainsAndAssign(string input, string target, out string substring)
        {
            substring = null;
            if (input.Contains(target))
            {
                substring = target;
                return true;
            }
            return false;
        }

        static bool ContainsDelimiter(string input, out string substring)
        {
            string tmp = null;
            substring = null;
            if (Diacritics.Keys.Any(diacritic => ContainsAndAssign(input, diacritic, out tmp)))
            {
                substring = tmp;
                return true;
            }
            else if (input.Contains("_"))
            {
                substring = "_";
                return true;
            }
            else if (input.Contains(" "))
            {
                substring = " ";
                return true;
            }
            else return false;
        }

        private static void ProcessArgs(string[] args)
        {
            if (args.Length == 1 && (args[0].Equals("macros") || args[0].Equals("m")))
            {
                var macros = Macros;
                if (macros == null) Console.WriteLine("Error: unable to parse mappings");
                else
                {
                    if (macros.Count == 0) Console.WriteLine("<empty>");
                    foreach (var key in macros.Keys)
                        Console.WriteLine(key + " \u2192 " + macros[key]);
                }
            }
            else if (args.Length == 1 && (args[0].Equals("letters") || args[0].Equals("l")))
            {
                var letters = Letters;
                if (letters == null) Console.WriteLine("Error: unable to parse mappings");
                else
                {
                    foreach (var key in letters.Keys)
                        Console.WriteLine(key + " \u2192 " + letters[key]);
                }
            }
            else if (args.Length == 3 && (args[0].Equals("add") || args[0].Equals("a")))
            {
                var arabic = Arabize(args[2]);
                if (arabic == null) Console.WriteLine("Error: unable to parse mappings");
                else
                {
                    var key = args[1];
                    string badSubstring;
                    if (ContainsDelimiter(key, out badSubstring)) Console.WriteLine("Error: key should not contain the delimiter \"" + badSubstring + "\"");
                    else if (AddMacro(key, arabic))
                    {
                        if (!string.IsNullOrEmpty(arabic))
                        {
                            Clipboard.SetText(arabic);
                            Console.WriteLine(key + " \u2192 " + arabic);
                        }
                        else Console.WriteLine("Error: empty buffer");
                    }
                    else Console.WriteLine("Error: key already exists");
                }
            }
            else if (args.Length == 3 && (args[0].Equals("add-lit") || args[0].Equals("al")))
            {
                var arabic = args[2];
                var key = args[1];
                string badSubstring;
                if (ContainsDelimiter(key, out badSubstring)) Console.WriteLine("Error: key should not contain the delimiter \"" + badSubstring + "\"");
                else if (AddMacro(key, arabic))
                {
                    if (!string.IsNullOrEmpty(arabic))
                    {
                        Clipboard.SetText(arabic);
                        Console.WriteLine(key + " \u2192 " + arabic);
                    }
                    else Console.WriteLine("Error: empty buffer");
                }
                else Console.WriteLine("Error: key already exists");
            }
            else if (args.Length == 2 && (args[0].Equals("remove") || args[0].Equals("r")))
            {
                var key = args[1];
                var value = RemoveMacro(key);
                if (value != null) Console.WriteLine(key + " \u2260 " + value);
                else Console.WriteLine("Error: unable to find " + key);
            }
            else if (args.Length == 1 && (args[0].Equals("open") || args[0].Equals("o")))
            {
                System.Diagnostics.Process.Start(MacrosFilePath);
                Console.WriteLine(MacrosFilePath);
            }
            else if (args.Length == 1 && (args[0].Equals("help") || args[0].Equals("h")))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  macros            - List all macros");
                Console.WriteLine("  open              - Open the macros JSON file");
                Console.WriteLine("  add [key] [value] - Add a new macro where value is arabized");
                Console.WriteLine("  remove [key]      - Add a new macro where value is literal");
                Console.WriteLine("  clear             - Clear console screen");
                Console.WriteLine("  quit              - Exit program");
            }
            else
            {
                var arabic = Arabize(string.Join(" ", args));
                if (arabic == null) Console.WriteLine("Error: unable to parse mappings");
                else
                {
                    if (!string.IsNullOrEmpty(arabic))
                    {
                        Clipboard.SetText(arabic);
                        Console.WriteLine(arabic);
                    }
                    else Console.WriteLine("Error: empty buffer");
                }
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            if (args.Length > 0)
                ProcessArgs(args);
            else
            {
                string input;
                while (true)
                {
                    Console.Write("> ");
                    input = Console.ReadLine().Trim();
                    if (input.Equals("cls") || input.Equals("clear"))
                    {
                        Console.Clear();
                        continue;
                    }
                    else if (input.Equals("q") || input.Equals("quit")) return;
                    var argsArray = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    ProcessArgs(argsArray);
                }
            }
        }
    }
}
