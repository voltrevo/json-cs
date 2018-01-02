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
    }
}
