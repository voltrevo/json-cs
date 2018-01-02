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

            Console.WriteLine(v.ToString("    "));
        }
    }
}
