using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace ArabizeCli.Commands
{
    public static class EditHandler
    {
        public static void Handle()
        {
            string text;
            Directory.CreateDirectory(Defaults.configurationPath);
            var macrosPath = Path.Join(Defaults.configurationPath, Defaults.macrosFileName);
            var editor = Environment.GetEnvironmentVariable("EDITOR")
                ?? Environment.GetEnvironmentVariable("VISUAL")
                ?? GetDefaultEditor();
            try
            {
                if (string.IsNullOrEmpty(editor)) throw new Exception("No text editor found. Set the EDITOR environment variable");
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = editor,
                        Arguments = macrosPath,
                        UseShellExecute = false
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            catch
            {
                throw new Exception($"Could not start '{editor}'");
            }
            text = File.ReadAllText(macrosPath);
            var macros = JsonSerializer.Deserialize(text, JsonContext.Default.DictionaryStringString);
            Console.WriteLine($"{macros.Count} macro(s).");
        }

        private static string GetDefaultEditor()
        {
            if (OperatingSystem.IsWindows()) return "notepad";
            if (OperatingSystem.IsLinux()) return "nano";
            if (OperatingSystem.IsMacOS()) return "vim";
            return null;
        }
    }
}