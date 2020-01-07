using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Diagnostics;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle04.txt");

            double[] basePattern = { 0, 1, 0, -1 };

            //Part01(basePattern, contentsMain.Select(x => double.Parse(x.ToString())).ToArray(), 2);
            Part02(basePattern, contentsMain.Select(x => double.Parse(x.ToString())).ToList(), 100, 10000, int.Parse(contentsMain.Substring(0,7)));

        }

        public static double[] Trouble(double[] input) {
            double[] newSequence = new double[input.Length];
            double currentDigit = 0;

            for (int i = input.Length-1; i >=0; i--)
            {
                currentDigit = (currentDigit + input[i]) % 10;
                newSequence[i] = currentDigit;
            } 

            /*
                def calc_part_two(data_sequence):
                    new_sequence = [0 for i in range(len(data_sequence))]
                    cur_digit_value = 0
                    # Fill new sequence from back to front
                    for cur_digit in range(len(data_sequence)-1, -1, -1):
                        cur_digit_value = (cur_digit_value + data_sequence[cur_digit]) % 10
                        new_sequence[cur_digit] = cur_digit_value
                    return new_sequence
            */

            return newSequence;
        }

        public static double[] Phase(double[] basePattern, double[] input, bool final, int offset = 0)
        {
            double[] basePatternAdjusted = basePattern;
            List<double> baseAdj = new List<double>();
            double[] result = new double[input.Length];
            double iteration = offset;

            for (double r = result.Length; r > offset; r--)
            {
                iteration = r+1;
                baseAdj.Clear();
                while (baseAdj.Count <= result.Length)
                {
                    foreach (var item in basePattern)
                    {
                        for (int i = 0; i < iteration; i++)
                        {
                            baseAdj.Add(item);
                        }
                    }
                }

                baseAdj.RemoveAt(0);
                for (double i = input.Length; i > offset+iteration-1; i--)
                {
                    if (baseAdj[(int)i]!=0){

                        // if (i % 32 == 0)
                        // {
                        //     Console.WriteLine($"XXX 32 - {i} Total: {result[(int)r]}");
                        // }

                        if (input[(int)i]!=0) {
                            result[(int)r] = (result[(int)r] + (input[(int)i] * baseAdj[(int)i])) % 10;
                            ///Console.WriteLine($"XXX - {input[(int)i] * baseAdj[(int)i]} Total: {result[(int)r]}");
                        }
                    }
                    else
                    {
                        i += iteration-1;
                    }
                }
                //Console.WriteLine($"XXX Final - {r} - {result[(int)r]}");
            }

            for (double r = 0; r < result.Length; r++)
            {
                result[(int)r] = Math.Abs(result[(int)r] % 10);
            }

            if (final)
            {
                string location = "";
                for (int i = 0; i < 7; i++)
                {
                    location += result[i].ToString();
                }

                for (int i = 0; i < 7; i++)
                {
                    result[i] = result[i + int.Parse(location)];
                }
            }

            return result;
        }

        public static void Part01(double[] basePattern, double[] input, int phases)
        {
            for (int i = 0; i < phases; i++)
            {
                input = Phase(basePattern, input, false);
            }
            Console.WriteLine();
            for (double r = 0; r < 8; r++)
            {
                if (r < 8) Console.Write($" {input[(int)r]}");

            }
        }

        public static void Part02(double[] basePattern, List<double> inputList, int phases, int repeat, int offset)
        {
            double[] input = inputList.ToArray();
            if (repeat > 1)
            {
                int halfLength = input.Length;
                Array.Resize(ref input, input.Length * repeat);
                
                for (int i = halfLength; i < input.Length; i++)
                {
                    input[i] = input[i % halfLength];
                }
            }


            inputList = input.ToList();
            inputList.RemoveRange(0, offset);
            input = inputList.ToArray();

            Console.WriteLine();
            for (int i = 0; i < phases; i++)
            {
                input = Trouble(input);
            }
            for (double r = 0; r < 8; r++)
            {
                if (r<8) Console.Write($"{input[(int)r]}");
            }
        }
    }
}
