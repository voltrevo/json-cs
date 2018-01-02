using System.Collections.Generic;
using System.Globalization;

namespace Json
{
    public class JsonNumber : JsonValue
    {
        public override Type GetJsonType() { return Type.Number; }
        public double data { get; set; }
        public JsonNumber(double data) { this.data = data; }

        public override string ToString(string indent = "", string accumulatedIndent = "")
        {
            return this.data.ToString();
        }

        private static HashSet<char> allowedExtraChars = new HashSet<char>()
        {
            '+',
            '-',
            'e',
            '.',
        };

        private static int SkipAllowedChars(string jsonString, int pos)
        {
            while (true)
            {
                if (pos >= jsonString.Length)
                {
                    return pos;
                }

                char c = jsonString[pos];

                if ('0' <= c && c <= '9' || allowedExtraChars.Contains(c))
                {
                    pos++;
                }
                else
                {
                    return pos;
                }
            }
        }

        public static new (JsonNumber, int) FromString(string jsonString, int pos)
        {
            int endPos = SkipAllowedChars(jsonString, pos);

            var value = new JsonNumber(double.Parse(
                jsonString.Substring(pos, endPos - pos),
                NumberStyles.Float
            ));

            return (value, endPos);
        }

        public static new JsonNumber FromString(string jsonString)
        {
            return CheckedParse(jsonString, FromString);
        }
    }
}
