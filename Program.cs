using System;
using System.Collections.Generic;
using System.Linq;

namespace Json
{
    public enum Type {StringMap, Array, String, Number};

    public abstract class JsonValue
    {
        public abstract Type GetJsonType();

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

        protected abstract string ToStringImpl();

        public override string ToString() { return this.ToStringImpl(); }
    }

    public class JsonString : JsonValue
    {
        public override Type GetJsonType() { return Type.String; }
        public string data { get; set; }
        public JsonString(string data) { this.data = data; }

        protected override string ToStringImpl()
        {
            // TODO: escaping
            return $"\"{data}\"";
        }

        public static string ToStringExternal(string s)
        {
            return (new JsonString(s)).ToString();
        }
    }

    public class JsonNumber : JsonValue
    {
        public override Type GetJsonType() { return Type.Number; }
        public double data { get; set; }
        public JsonNumber(double data) { this.data = data; }

        protected override string ToStringImpl()
        {
            return this.data.ToString();
        }
    }

    public class JsonArray : JsonValue
    {
        public override Type GetJsonType() { return Type.Array; }
        public List<JsonValue> data { get; set; }
        public JsonArray(List<JsonValue> data) { this.data = data; }

        protected override string ToStringImpl()
        {
            string result = "[";
            bool first = true;

            foreach (JsonValue v in this.data) {
                if (!first) { result += ','; }
                result += v.ToString();
                first = false;
            }

            result += "]";

            return result;
        }
    }

    public class JsonObject : JsonValue
    {
        public override Type GetJsonType() { return Type.StringMap; }
        public Dictionary<string, JsonValue> data { get; set; }
        public JsonObject(Dictionary<string, JsonValue> data) { this.data = data; }

        protected override string ToStringImpl()
        {
            string result = "{";
            bool first = true;

            foreach (var (key, value) in this.data)
            {
                if (!first) { result += ','; }
                result += JsonString.ToStringExternal(key) + ':' + value.ToString();
                first = false;
            }

            result += '}';

            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            JsonValue v = new JsonObject(new Dictionary<string, JsonValue>()
            {
                {"x", 1},
                {"y", "foo"},
                {"primes", new JsonArray(new List<JsonValue>()
                {
                    2,
                    3,
                    5,
                    7,
                    11,
                })},
            });

            Console.WriteLine(v.ToString());
        }
    }
}
