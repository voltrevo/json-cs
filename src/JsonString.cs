namespace Json
{
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
}
