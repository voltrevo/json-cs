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
    }
}
