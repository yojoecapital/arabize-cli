using System.Collections.Generic;
using System;
using System.Linq;

namespace ArabizeCore.Utilities
{
    internal static class Helpers
    {
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

        public static string TrimForDiacritic(string letter, Dictionary<string, string> diacritics, out string diacritic)
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

        public static int IndexOfFirstDelimiters(string input, IEnumerable<string> delimiters)
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

        public static IEnumerable<string> SplitWithDelimiters(string input, IEnumerable<string> delimiters)
        {
            int split;
            while ((split = IndexOfFirstDelimiters(input, delimiters)) != -1)
            {
                yield return input.Substring(0, split);
                input = input[split..];
            }
            if (!string.IsNullOrEmpty(input)) yield return input;
        }

        public static bool ContainsAndAssign(string input, string target, out string substring)
        {
            substring = null;
            if (input.Contains(target))
            {
                substring = target;
                return true;
            }
            return false;
        }

        public static bool ContainsDelimiter(string input, Dictionary<string, string> diacritics, out string substring)
        {
            string tmp = null;
            substring = null;
            if (diacritics.Keys.Any(diacritic => ContainsAndAssign(input, diacritic, out tmp)))
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
    }
}
