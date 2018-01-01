using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonParser
{
    public enum Type {StringMap, Array, String, Number};

    public abstract class Value
    {
        public abstract Type GetJsonType();

        public static explicit operator Dictionary<string, Value>(Value value)
        {
            return ((StringMapValue)value).data;
        }

        public static explicit operator List<Value>(Value value)
        {
            return ((ArrayValue)value).data;
        }

        public static explicit operator string(Value value) { return ((StringValue)value).data; }
        public static explicit operator double(Value value) { return ((NumberValue)value).data; }

        public static explicit operator Dictionary<string, string>(Value value)
        {
            return ((Dictionary<string, Value>)value).ToDictionary(
                item => item.Key,
                item => (string)item.Value
            );
        }

        public static explicit operator Dictionary<string, double>(Value value)
        {
            return ((Dictionary<string, Value>)value).ToDictionary(
                item => item.Key,
                item => (double)item.Value
            );
        }

        public static explicit operator List<string>(Value value)
        {
            return ((List<Value>)value).ConvertAll(v => (string)v);
        }

        public static explicit operator List<double>(Value value)
        {
            return ((List<Value>)value).ConvertAll(v => (double)v);
        }

        protected abstract string ToStringImpl();

        public string ToString() { return this.ToStringImpl(); }
    }

    public class StringValue : Value
    {
        public override Type GetJsonType() { return Type.String; }
        public string data { get; set; }
        public StringValue(string data) { this.data = data; }

        protected override string ToStringImpl()
        {
            // TODO: escaping
            return $"\"{data}\"";
        }

        public static string ToStringExternal(string s)
        {
            return (new StringValue(s)).ToString();
        }
    }

    public class NumberValue : Value
    {
        public override Type GetJsonType() { return Type.Number; }
        public double data { get; set; }
        public NumberValue(double data) { this.data = data; }

        protected override string ToStringImpl()
        {
            return this.data.ToString();
        }
    }

    public class ArrayValue : Value
    {
        public override Type GetJsonType() { return Type.Array; }
        public List<Value> data { get; set; }
        public ArrayValue(List<Value> data) { this.data = data; }

        protected override string ToStringImpl()
        {
            string result = "[";
            bool first = true;

            foreach (Value v in this.data) {
                if (!first) { result += ','; }
                result += v.ToString();
                first = false;
            }

            result += "]";

            return result;
        }
    }

    public class StringMapValue : Value
    {
        public override Type GetJsonType() { return Type.StringMap; }
        public Dictionary<string, Value> data { get; set; }
        public StringMapValue(Dictionary<string, Value> data) { this.data = data; }

        protected override string ToStringImpl()
        {
            string result = "{";
            bool first = true;

            foreach (var (key, value) in this.data)
            {
                if (!first) { result += ','; }
                result += StringValue.ToStringExternal(key) + ':' + value.ToString();
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
            Console.WriteLine("Hello World!");
        }
    }
}
