using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ArabizeCli.Commands
{
    public static partial class ArabizeHandler
    {
        public static void Handle(string[] args)
        {
            var macrosPath = Path.Join(Defaults.configurationPath, Defaults.macrosFileName);

            // seperate the input arguments
            var inputWords = args.SelectMany(arg => WhitespaceRegex().Split(arg)
                .Where(word => !string.IsNullOrEmpty(word)));

            // read standard input
            if (Console.IsInputRedirected)
            {
                inputWords = inputWords.Concat(WhitespaceRegex().Split(Console.In.ReadToEnd()));
            }
            Dictionary<string, string> macros = null;
            if (File.Exists(macrosPath))
            {
                try
                {
                    string json = File.ReadAllText(macrosPath);
                    macros = JsonSerializer.Deserialize(json, JsonContext.Default.DictionaryStringString);
                }
                catch
                {
                    macros = null;
                }
            }
            args = [.. inputWords];
            var arabic = new List<string>(args.Length);
            foreach (var word in args) arabic.Add(ArabizeCliWord(word, macros));
            Console.WriteLine(string.Join(' ', arabic));
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();

        private static int ComputeLevenshteinDistance(string s, string t)
        {
            int m = s.Length;
            int n = t.Length;
            int[,] d = new int[m + 1, n + 1];
            for (int i = 0; i <= m; i++) d[i, 0] = i;
            for (int j = 0; j <= n; j++) d[0, j] = j;
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

        private static string ArabizeCliWord(string word, Dictionary<string, string> macros)
        {
            var stringBuilder = new StringBuilder();
            var splitWords = word.Split('-');
            foreach (var splitWord in splitWords)
            {
                foreach (var token in SplitWithDelimiters(splitWord, Defaults.diacritics.Keys))
                {
                    var key = TrimForDiacritic(token, out string diacritic);
                    if (macros != null && macros.TryGetValue(key, out var value))
                    {
                        stringBuilder.Append(ArabizeCliWord(value, null));
                        stringBuilder.Append(diacritic);
                    }
                    else
                    {
                        var letter = FindLetter(key);
                        stringBuilder.Append(letter);
                        stringBuilder.Append(diacritic);
                    }
                }
            }
            return stringBuilder.ToString();
        }

        private static IEnumerable<string> SplitWithDelimiters(string input, IEnumerable<string> delimiters)
        {
            int split;
            while ((split = IndexOfFirstDelimiters(input, delimiters)) != -1)
            {
                yield return input[..split];
                input = input[split..];
            }
            if (!string.IsNullOrEmpty(input)) yield return input;
        }

        private static int IndexOfFirstDelimiters(string input, IEnumerable<string> delimiters)
        {
            int start = 0;
            while (start < input.Length)
            {
                foreach (string delimiter in delimiters)
                {
                    if (input[start..].StartsWith(delimiter))
                        return start + delimiter.Length;
                }
                start++;
            }
            return -1;
        }

        private static string TrimForDiacritic(string letter, out string diacritic)
        {
            diacritic = null;
            foreach (var key in Defaults.diacritics.Keys)
            {
                if (letter.EndsWith(key))
                {
                    diacritic = Defaults.diacritics[key];
                    if (!letter.EndsWith(key)) return letter;
                    else return letter[..letter.LastIndexOf(key)];
                }
            }
            return letter;
        }

        private static string FindLetter(string key)
        {
            int minDistance = int.MaxValue;
            string closestKey = string.Empty;
            foreach (var s in Defaults.letters.Keys)
            {
                int distance = ComputeLevenshteinDistance(s, key);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestKey = s;
                }
            }
            return Defaults.letters[closestKey];
        }

    }
}