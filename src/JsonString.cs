using System.Collections.Generic;

namespace Json
{
    public class JsonString : JsonValue
    {
        public override Type GetJsonType() { return Type.String; }
        public string data { get; set; }
        public JsonString(string data) { this.data = data; }

        private static Dictionary<char, string> escapeCodes = new Dictionary<char, string>()
        {
            {'\b', "\\b"},
            {'\f', "\\f"},
            {'\n', "\\n"},
            {'\r', "\\r"},
            {'\t', "\\t"},
            {'\"', "\\\""},
            {'\\', "\\\\"},
        };

        public static string Escape(string s)
        {
            string result = "";

            foreach (char c in s)
            {
                result += CollectionExtensions.GetValueOrDefault(escapeCodes, c, $"{c}");
            }

            return result;
        }

        public override string ToString(string indent = "", string accumulatedIndent = "")
        {
            return $"\"{Escape(this.data)}\"";
        }
    }
}
