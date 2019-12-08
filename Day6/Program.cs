using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");

            Console.WriteLine($"{contentsMain}");
        }
    }
}
