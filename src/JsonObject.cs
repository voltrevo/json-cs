using System.Collections;
using System.Collections.Generic;

namespace Json
{
    public class JsonObject : JsonValue, IEnumerable<KeyValuePair<string, JsonValue>>
    {
        public override Type GetJsonType() { return Type.StringMap; }
        public Dictionary<string, JsonValue> data { get; set; }
        public JsonObject() { this.data = new Dictionary<string, JsonValue>(); }
        public JsonObject(Dictionary<string, JsonValue> data) { this.data = data; }

        IEnumerator<KeyValuePair<string, JsonValue>> IEnumerable<KeyValuePair<string, JsonValue>>.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public void Add(string key, JsonValue value)
        {
            this.data.Add(key, value);
        }

        public override string ToString(string indent = "", string accumulatedIndent = "")
        {
            if (this.data.Count == 0)
            {
                return "{}";
            }

            string result = "{";

            if (indent == "")
            {
                string prevEntry = "";

                foreach (var (key, value) in this)
                {
                    if (prevEntry != "") { result += prevEntry + ','; }
                    prevEntry = $"\"{JsonString.Escape(key)}\":{value.ToString()}";
                }

                result += prevEntry + '}';
            }
            else
            {
                result += '\n';
                string nextIndent = accumulatedIndent + indent;
                string prevEntry = "";

                foreach (var (key, value) in this)
                {
                    if (prevEntry != "") { result += prevEntry + ",\n"; }

                    prevEntry = (
                        nextIndent +
                        '\"' +
                        JsonString.Escape(key) +
                        "\": " +
                        value.ToString(indent, nextIndent)
                    );
                }

                result += prevEntry + '\n' + accumulatedIndent + '}';
            }

            return result;
        }

        public static new (JsonObject, int) FromString(string jsonString, int pos)
        {
            if (jsonString[pos] != '{')
            {
                throw new System.FormatException("Json array must start with '{'");
            }

            pos++;

            JsonObject result = new JsonObject();
            pos = SkipWhitespace(jsonString, pos);
            bool haveComma = false;

            while (true)
            {
                pos = SkipWhitespace(jsonString, pos);

                if (!haveComma && jsonString[pos] == '}')
                {
                    pos++;
                    break;
                }

                JsonString currKey;
                (currKey, pos) = JsonString.FromString(jsonString, pos);
                pos = SkipWhitespace(jsonString, pos);

                if (jsonString[pos] != ':')
                {
                    throw new System.FormatException($"Expected ':' at position {pos}");
                }

                pos++;
                pos = SkipWhitespace(jsonString, pos);

                JsonValue currValue;
                (currValue, pos) = JsonValue.FromString(jsonString, pos);

                result.Add((string)currKey, currValue);

                pos = SkipWhitespace(jsonString, pos);
                char c = jsonString[pos];
                pos++;

                if (c == '}')
                {
                    break;
                }

                if (c == ',')
                {
                    haveComma = true;
                    continue;
                }

                throw new System.FormatException($"Unexpected character {c} at position {pos}");
            }

            return (result, pos);
        }

        public static new JsonObject FromString(string jsonString)
        {
            return CheckedParse(jsonString, FromString);
        }
    }
}
