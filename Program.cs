using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Json
{
    public enum Type {StringMap, Array, String, Number};

    public abstract class JsonValue
    {
        public abstract Type GetJsonType();

        public JsonValue this[string key]
        {
            get { return ((JsonObject)this).data[key]; }
            set { ((JsonObject)this).data[key] = value; }
        }

        public JsonValue this[int index]
        {
            get { return ((JsonArray)this).data[index]; }
            set { ((JsonArray)this).data[index] = value; }
        }

        #region Cast to data

        public static explicit operator Dictionary<string, JsonValue>(JsonValue value)
        {
            return ((JsonObject)value).data;
        }

        public static explicit operator List<JsonValue>(JsonValue value)
        {
            return ((JsonArray)value).data;
        }

        public static explicit operator string(JsonValue value) { return ((JsonString)value).data; }
        public static explicit operator double(JsonValue value) { return ((JsonNumber)value).data; }

        public static explicit operator Dictionary<string, string>(JsonValue value)
        {
            return ((Dictionary<string, JsonValue>)value).ToDictionary(
                item => item.Key,
                item => (string)item.Value
            );
        }

        public static explicit operator Dictionary<string, double>(JsonValue value)
        {
            return ((Dictionary<string, JsonValue>)value).ToDictionary(
                item => item.Key,
                item => (double)item.Value
            );
        }

        public static explicit operator List<string>(JsonValue value)
        {
            return ((List<JsonValue>)value).ConvertAll(v => (string)v);
        }

        public static explicit operator List<double>(JsonValue value)
        {
            return ((List<JsonValue>)value).ConvertAll(v => (double)v);
        }

        #endregion

        #region Cast from data

        public static implicit operator JsonValue(string data)
        {
            return new JsonString(data);
        }

        public static implicit operator JsonValue(double data)
        {
            return new JsonNumber(data);
        }

        #endregion

        public abstract string ToString(string indent = "", string accumulatedIndent = "");
    }

    public class JsonString : JsonValue
    {
        public override Type GetJsonType() { return Type.String; }
        public string data { get; set; }
        public JsonString(string data) { this.data = data; }

        public override string ToString(string indent = "", string accumulatedIndent = "")
        {
            // TODO: escaping
            return $"\"{data}\"";
        }

        public static string StaticToString(string s)
        {
            return (new JsonString(s)).ToString();
        }
    }

    public class JsonNumber : JsonValue
    {
        public override Type GetJsonType() { return Type.Number; }
        public double data { get; set; }
        public JsonNumber(double data) { this.data = data; }

        public override string ToString(string indent = "", string accumulatedIndent = "")
        {
            return this.data.ToString();
        }
    }

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

    class Program
    {
        static void Main(string[] args)
        {
            JsonValue v = new JsonObject()
            {
                {"x", 1},
                {"y", "foo"},
                {"primes", new JsonArray() {2, 3, 5, 7, 11}},
            };

            Console.WriteLine(v.ToString("    "));
        }
    }
}
