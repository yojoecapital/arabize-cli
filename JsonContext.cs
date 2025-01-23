using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArabizeCli
{
    [JsonSerializable(typeof(Dictionary<string, string>))]
    public partial class JsonContext : JsonSerializerContext
    {
        public static Dictionary<string, string> Get(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize(json, Default.DictionaryStringString);
            }
            catch
            {
                throw new Exception($"Could not read or parse JSON file at '{path}'");
            }
        }
    }
}