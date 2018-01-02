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

        private static Dictionary<char, char> unescapeCodes = new Dictionary<char, char>()
        {
            {'b', '\b'},
            {'f', '\f'},
            {'n', '\n'},
            {'r', '\r'},
            {'t', '\t'},
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

        public static new JsonString FromString(string jsonString, int pos)
        {
            if (jsonString[pos] != '"') {
                throw new System.FormatException("Json string must start with '\"'");
            }

            pos++;

            string result = "";
            bool escaped = false;

            while (true)
            {
                if (pos >= jsonString.Length)
                {
                    throw new System.FormatException("Unterminated string");
                }

                char c = jsonString[pos];
                pos++;

                if (escaped)
                {
                    result += CollectionExtensions.GetValueOrDefault(unescapeCodes, c, c);
                    escaped = false;
                }
                else
                {
                    if (c == '"')
                    {
                        break;
                    }

                    if (c == '\\')
                    {
                        escaped = true;
                    }
                    else
                    {
                        result += c;
                    }
                }
            }

            return new JsonString(result);
        }
    }
}
