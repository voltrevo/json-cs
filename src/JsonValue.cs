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

        private static HashSet<char> whitespaceChars = new HashSet<char>()
        {
            ' ',
            '\t',
            '\n',
            '\r',
        };

        protected static int SkipWhitespace(string jsonString, int pos)
        {
            while (pos < jsonString.Length && whitespaceChars.Contains(jsonString[pos]))
            {
                pos++;
            }
            
            return pos;
        }

        public static (JsonValue, int) FromString(string jsonString, int pos)
        {
            char c = jsonString[pos];

            if (c == '{') { return JsonObject.FromString(jsonString, pos); }
            if (c == '[') { return JsonArray.FromString(jsonString, pos); }
            if (c == '"') { return JsonString.FromString(jsonString, pos); }
            if ((c == '-') || ('0' <= c && c <= '9')) { return JsonNumber.FromString(jsonString, pos); }

            throw new System.FormatException($"Unexpected character {c} at position {pos}");
        }

        protected static T CheckedParse<T>(string jsonString, System.Func<string, int, (T, int)> parse)
        {
            int pos = SkipWhitespace(jsonString, 0);
            T value;
            (value, pos) = parse(jsonString, 0);
            pos = SkipWhitespace(jsonString, pos);

            if (pos != jsonString.Length)
            {
                throw new System.FormatException($"Unexpected character {jsonString[pos]} at position {pos}");
            }

            return value;
        }

        public static JsonValue FromString(string jsonString)
        {
            return CheckedParse(jsonString, FromString);
        }
    }
}
