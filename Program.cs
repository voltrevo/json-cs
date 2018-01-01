﻿using System;
using System.Collections.Generic;

namespace JsonParser
{
    abstract public class Value
    {
        public enum Type {StringMap, Array, String, Number};
        Type type;

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
    }

    public class StringValue : Value
    {
        public string data { get; set; }
        public StringValue(string data) { this.data = data; }
    }

    public class NumberValue : Value
    {
        public double data { get; set; }
        public NumberValue(double data) { this.data = data; }
    }

    public class ArrayValue : Value
    {
        public List<Value> data { get; set; }
        public ArrayValue(List<Value> data) { this.data = data; }
    }

    public class StringMapValue : Value
    {
        public Dictionary<string, Value> data { get; set; }
        public StringMapValue(Dictionary<string, Value> data) { this.data = data; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
