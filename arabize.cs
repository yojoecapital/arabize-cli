using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Arabize
{
    static class Program
    {
        static readonly string lettersFileName = "letters.xml";
        static readonly string macrosFileName = "macros.xml";

        static string LettersFilePath{
            get{
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, lettersFileName);
            }
        }

        static string MacrosFilePath{
            get{
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, macrosFileName);
                if (!File.Exists(filePath))
                {
                    XDocument xml = new XDocument(new XElement("root"));
                    xml.Save(filePath);
                }
                return filePath;
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

        static bool Add(string key, string value) 
        {
            XDocument xml = XDocument.Load(MacrosFilePath);
    
            // Check if key already exists in XML
            if (xml.Descendants("row").Any(row => row.Attribute("key").Value == key))
                return false;
            else if (XDocument.Load(LettersFilePath).Root.Elements().Any(row => row.Attribute("key").Value == key))
                return false;
            
            XElement newRow = new XElement("row",
                              new XAttribute("key", key),
                              new XAttribute("value", value.Trim()));
            
            xml.Element("root").Add(newRow);
            xml.Save(MacrosFilePath);
            return true;
        }

        public static string Remove(string key)
        {
            XDocument xml = XDocument.Load(MacrosFilePath);
    
            var rowToRemove = xml.Descendants("row").FirstOrDefault(x => (string)x.Attribute("key") == key);            
            if (rowToRemove != null)
            {
                string value = (string)rowToRemove.Attribute("value"); 
                rowToRemove.Remove(); 
                xml.Save(MacrosFilePath); 
                return value; // return value attribute of removed row element
            }
            else return null;
        }  

        static string Arabize(string transliteration)
        {
            Dictionary<string, string> mapping;
            try
            {
                mapping = XDocument.Load(LettersFilePath).Root.Elements()
                .ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value);
                var macros = XDocument.Load(MacrosFilePath);
                foreach (var row in macros.Root.Elements())
                    mapping[row.Attribute("key").Value] = row.Attribute("value").Value;
            }
            catch 
            {
                return null;
            }
            var arabic = new List<string>();
            var words = transliteration.Split(' ');
            foreach (var word in words)
            {
                var letters = word.Split('_');
                var arabicWord = string.Empty;
                foreach (var letter in letters)
                {
                    var key = FindClosestKey(mapping, letter);
                    if (mapping.ContainsKey(key))
                    {
                        arabicWord += mapping[key];
                    }
                }
                arabic.Add(arabicWord);
            }

            return string.Join(" ", arabic);
        }

        static Dictionary<string, string> GetMacros()
        {
            try{
                return XDocument.Load(MacrosFilePath).Root.Elements()
                    .ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value);
            }
            catch {
                return null;
            }
        }

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (args.Length < 1){
                Console.WriteLine("Usage: arabize.exe <transliterated Arabic>");
                return;
            }
            else if (args[0].Equals("macros") && args.Length == 1){
                var macros = GetMacros();
                if (macros == null) Console.WriteLine("Error: unable to parse mappings");
                else {
                    foreach (var key in macros.Keys)
                        Console.WriteLine(key + " \u2192 " + macros[key]);
                }
            }
            else if (args[0].Equals("add") && args.Length == 3){
                var arabic = Arabize(args[2]);
                if (arabic == null) Console.WriteLine("Error: unable to parse mappings");
                else {
                    if (Add(args[1], arabic)){
                        Clipboard.SetText(arabic);
                        Console.WriteLine("Added " + arabic + " for " + args[1]);
                    } else Console.WriteLine("Error: key already exists");
                }
            }
            else if (args[0].Equals("add-lit") && args.Length == 3){
                if (Add(args[1], args[2])){
                    Clipboard.SetText(args[2]);
                    Console.WriteLine("Added " + args[2] + " for " + args[1]);
                } else Console.WriteLine("Error: key already exists");
            }
            else if (args[0].Equals("remove") && args.Length == 2){
                var value = Remove(args[1]);
                if (value != null) Console.WriteLine("Removed " +  args[1] + " for " + value);
                else Console.WriteLine("Error: unable to find " +  args[1]);
            }
            else
            {
                var arabic = Arabize(string.Join(" ", args));
                if (arabic == null) Console.WriteLine("Error: unable to parse mappings");
                else {
                    Clipboard.SetText(arabic);
                    Console.WriteLine("Copied: " + arabic);
                }
            }
        }
    }
}
