using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Json
{
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

            JsonValue v2 = JsonValue.FromString(@"{
                ""point"": {""x"": 2.18, ""y"": -3.7},
                ""squares"": [1, 4, 9, 16, 25]
            }");

            JsonArray squares = (JsonArray)v2["squares"];
            squares.Add(36);

            Console.WriteLine(v.ToString("    "));
            Console.WriteLine(v2.ToString("    "));
        }
    }
}
