using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArabizeCli
{
    [JsonSerializable(typeof(Dictionary<string, string>))]
    public partial class JsonContext : JsonSerializerContext { }
}