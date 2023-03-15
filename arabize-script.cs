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

        static bool AppendToFile(string key, string value) 
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "/arabic-letters.txt";
            // Check if the key already exists in the file
            if (File.ReadLines(filePath).Any(line => line.Split(':')[1].Trim().Equals(key)))
            {
                Console.WriteLine("Error: key already exists in the file");
                return false;
            }

            using (StreamWriter sw = File.AppendText(filePath)) {
                sw.WriteLine(value + ":" + key);
            }

            Console.WriteLine("Added " +  value + " for " + key);
            return true;
        }

        public static string RemoveFromFile(string key)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "/arabic-letters.txt";
            string value = null;
            string[] lines = File.ReadAllLines(filePath);
            List<string> newLines = new List<string>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string currentKey = parts[1].Trim();
                    if (!currentKey.Equals(key))
                    {
                        newLines.Add(line);
                    }
                    else
                    {
                        value = parts[0].Trim();
                    }
                }
            }

            File.WriteAllLines(filePath, newLines);

            return value;
}

        static string Arabize(string transliteration)
        {
            Dictionary<string, string> mapping;
            try
            {
                mapping = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + "/arabic-letters.txt")
                .Select(line => line.Split(':'))
                .ToDictionary(parts => parts[1].Trim(), parts => parts[0].Trim());
            }
            catch 
            {
                return "Error: unable to parse arabic-letters.txt";
            }
            string arabic = string.Empty;
            var keys = mapping.Keys.ToArray();
            var words = transliteration.Split(' ');
            foreach (var word in words)
            {
                var letters = word.Split('_');
                foreach (var letter in letters)
                {
                    var key = FindClosestKey(keys, letter);
                    if (mapping.ContainsKey(key))
                    {
                        arabic += mapping[key];
                    }
                }
                arabic += " ";
            }

            return arabic;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (args.Length < 1){
                Console.WriteLine("Usage: arabize.exe <transliterated Arabic>");
                return;
            }
            else if (args[0].Equals("add") && args.Length == 3){
                AppendToFile(args[1], Arabize(args[2]));
                return;
            }
            else if (args[0].Equals("add-lit") && args.Length == 3){
                AppendToFile(args[1], args[2]);
                return;
            }
            else if (args[0].Equals("remove") && args.Length == 2){
                var value = RemoveFromFile(args[1]);
                if (value != null) Console.WriteLine("Removed " +  args[1] + " for " + value);
                else Console.WriteLine("Error: unable to find " +  args[1]);
                return;
            }
            else
            {
                var arabic = Arabize(string.Join(" ", args));
                Console.WriteLine("~" + arabic);
            }

            
        }
    }
}
