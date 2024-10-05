using System;
using System.Collections.Generic;
using System.Text;

internal class Program
{
    static readonly Dictionary<string, string> diacritics = new()
    {
        { ".@''", "\u0655\u064B" }, // hamza below fathatan
        { ".@__", "\u0655\u064D" }, // hamza below kasratan
        { ".@%%", "\u0655\u064C" }, // hamza below dammatan
        { ".@#", "\u0655\u0652" },  // hamza below sukun
        { ".@'", "\u0655\u064E" },  // hamza below fatha
        { ".@_", "\u0655\u0650" },  // hamza below kasra
        { ".@%", "\u0655\u064F" },  // hamza below damma
        { "@''", "\u0654\u064B" },  // hamza above fathatan
        { "@__", "\u0654\u064D" },  // hamza above kasratan
        { "@%%", "\u0654\u064C" },  // hamza above dammatan
        { ".@", "\u0655" },         // hamza below
        { "@#", "\u0654\u0652" },   // hamza above sukun
        { "@'", "\u0654\u064E" },   // hamza above fatha
        { "@_", "\u0654\u0650" },   // hamza above kasra
        { "@%", "\u0654\u064F" },   // hamza above damma
        { "''", "\u064B" },         // fathatan
        { "__", "\u064D" },         // kasratan
        { "%%", "\u064C" },         // dammatan
        { "$'", "\u0651\u064E" },   // shadda fatha
        { "$%", "\u0651\u064F" },   // shadda damma
        { "$_", "\u0651\u0650" },   // shadda kasra
        { "@", "\u0654" },          // hamza above
        { "~", "\u0653" },          // maddah above
        { "$", "\u0651" },          // shadda
        { "'", "\u064E" },          // fatha
        { "_", "\u0650" },          // kasra
        { "%", "\u064F" },          // damma
        { "#", "\u0652" }           // sukun
    };

    static readonly Dictionary<string, string> letters = new()
    {
        { "alif", "ا" },        // Alif
        { "ba", "ب" },          // Ba
        { "ta", "ت" },          // Ta
        { "tha", "ث" },         // Tha
        { "jeem", "ج" },        // Jeem
        { "ha", "ح" },          // Ha
        { "kha", "خ" },         // Kha
        { "dal", "د" },         // Dal
        { "thal", "ذ" },        // Thal
        { "ra", "ر" },          // Ra
        { "zay", "ز" },         // Zay
        { "seen", "س" },        // Seen
        { "sheen", "ش" },       // Sheen
        { "sad", "ص" },         // Sad
        { "dad", "ض" },         // Dad
        { "tta", "ط" },         // Tta
        { "dha", "ظ" },         // Dha
        { "ayn", "ع" },         // Ayn
        { "ghayn", "غ" },       // Ghayn
        { "fa", "ف" },          // Fa
        { "qaf", "ق" },         // Qaf
        { "kaf", "ك" },         // Kaf
        { "lam", "ل" },         // Lam
        { "meem", "م" },        // Meem
        { "noon", "ن" },        // Noon
        { "haa", "ه" },         // Haa
        { "waw", "و" },         // Waw
        { "ya", "ي" },          // Ya
        { "hamza", "ء" },       // Hamza
        { "wasla", "\u0651" },  // Wasla (&#1649;)
        { "marbuta", "ة" },     // Ta Marbuta
        { "maksura", "ى" },     // Alif Maksura
        { "(?)", "◌" }          // Placeholder or unknown symbol
    };

    static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length == 1 && (args[0].Equals("--help") || args[0].Equals("-h")))
        {
            foreach (var pair in diacritics) Console.WriteLine($"{pair.Key} → {pair.Value}");
            foreach (var pair in letters) Console.WriteLine($"{pair.Key} → {pair.Value}");
            Console.WriteLine("Example: ya%waw-seen%fa → يُوسُف");
            return;
        }

        var diacriticsKeys = diacritics.Keys;
        var arabic = new List<string>();
        foreach (var word in args)
        {
            var tokens = word.Split('-');
            var arabicWord = string.Empty;
            foreach (var token in tokens) foreach (var splitLetter in SplitWithDelimiters(token, diacriticsKeys))
            {
                var key = FindClosestKey(letters, TrimForDiacritic(splitLetter, out string? diacritic));
                if (key != null) arabicWord += letters[key] + diacritic;
            }
            arabic.Add(arabicWord);
        }
        if (arabic.Count > 0) Console.WriteLine(string.Join(" ", arabic));
    }

    private static IEnumerable<string> SplitWithDelimiters(string input, IEnumerable<string> delimiters)
    {
        int split;
        while ((split = IndexOfFirstDelimiters(input, delimiters)) != -1)
        {
            yield return input.Substring(0, split);
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

    private static string TrimForDiacritic(string letter, out string? diacritic)
    {
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

    private static string? FindClosestKey(Dictionary<string, string> mapping, string key)
    {
        int minDistance = int.MaxValue;
        string? closestKey = null;

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

    private static int ComputeLevenshteinDistance(string s, string t)
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
}