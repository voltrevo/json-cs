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
                    prevEntry = JsonString.StaticToString(key) + ':' + value.ToString();
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
                        JsonString.StaticToString(key) +
                        ": " +
                        value.ToString(indent, nextIndent)
                    );
                }

                result += prevEntry + '\n' + accumulatedIndent + '}';
            }

            return result;
        }
    }
}
