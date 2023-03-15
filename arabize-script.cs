using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace ArabicTransliterator
{
    class Program
    {
        public static string FindClosestKey(string[] keys, string key)
        {
            int minDistance = int.MaxValue;
            string closestKey = null;

            foreach (string s in keys)
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

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: arabize.exe <transliterated Arabic>");
                return;
            }

            Dictionary<string, string> mapping;
            try
            {
                mapping = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + "/arabic-letters.txt")
                .Select(line => line.Split(':'))
                .ToDictionary(parts => parts[1].Trim(), parts => parts[0].Trim());
            }
            catch 
            {
                Console.WriteLine("Error: unable to parse arabic-letters.txt");
                return;
            }
            var keys = mapping.Keys.ToArray();

            var transliteration = string.Join(" ", args);

            var words = transliteration.Split(' ');
            foreach (var word in words)
            {
                var letters = word.Split('_');
                foreach (var letter in letters)
                {
                    var key = FindClosestKey(keys, letter);
                    if (mapping.ContainsKey(key))
                    {
                        Console.Write(mapping[key]);
                    }
                }
                Console.Write(" ");
            }

            Console.WriteLine();
        }
    }
}
