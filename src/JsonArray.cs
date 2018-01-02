using System.Collections;
using System.Collections.Generic;

namespace Json
{
    public class JsonArray : JsonValue, IEnumerable<JsonValue>
    {
        public override Type GetJsonType() { return Type.Array; }
        public List<JsonValue> data { get; set; }
        public JsonArray() { this.data = new List<JsonValue>(); }
        public JsonArray(List<JsonValue> data) { this.data = data; }

        IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public void Add(JsonValue value) { this.data.Add(value); }

        public override string ToString(string indent = "", string accumulatedIndent = "")
        {
            if (this.data.Count == 0)
            {
                return "[]";
            }

            string result = "[";

            if (indent == "")
            {
                string prevEntry = "";

                foreach (JsonValue v in this)
                {
                    if (prevEntry != "") { result += prevEntry + ','; }
                    prevEntry = v.ToString();
                }

                result += prevEntry + ']';
            }
            else
            {
                result += "\n";
                string nextIndent = accumulatedIndent + indent;
                string prevEntry = "";

                foreach (JsonValue v in this)
                {
                    if (prevEntry != "") { result += prevEntry + ",\n"; }
                    prevEntry = nextIndent + v.ToString(indent, nextIndent);
                }

                result += prevEntry + '\n' + accumulatedIndent + ']';
            }

            return result;
        }

        public static new (JsonArray, int) FromString(string jsonString, int pos)
        {
            if (jsonString[pos] != '[')
            {
                throw new System.FormatException("Json array must start with '['");
            }

            pos++;

            JsonArray result = new JsonArray();
            pos = SkipWhitespace(jsonString, pos);
            bool haveComma = false;

            while (true)
            {
                pos = SkipWhitespace(jsonString, pos);

                if (!haveComma && jsonString[pos] == ']')
                {
                    pos++;
                    break;
                }

                JsonValue curr;
                (curr, pos) = JsonValue.FromString(jsonString, pos);

                result.Add(curr);
                pos = SkipWhitespace(jsonString, pos);

                char c = jsonString[pos];
                pos++;

                if (c == ']')
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

        public static new JsonArray FromString(string jsonString)
        {
            return CheckedParse(jsonString, FromString);
        }
    }
}
