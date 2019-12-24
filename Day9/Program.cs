using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle03.txt");

            decimal relativeBase = 0;

            decimal runCycle(string contents, decimal input)
            {

                decimal[] array = contents.Split(",").Select(decimal.Parse).ToArray();
                Array.Resize(ref array, 1000000);

                int[] OptCode(int optCode)
                {
                    int[] returnVal = new int[4] { optCode % 100, 0, 0, 0 };

                    int length = optCode.ToString().Length;
                    switch (length)
                    {
                        case 1:
                        case 2:
                            break;
                        case 3:
                            returnVal[1] = (optCode / 100) % 10;
                            break;
                        case 4:
                            returnVal[1] = (optCode / 100) % 10;
                            returnVal[2] = (optCode / 1000) % 10;
                            break;
                        case 5:
                            returnVal[1] = (optCode / 100) % 10;
                            returnVal[2] = (optCode / 1000) % 10;
                            returnVal[3] = (optCode / 10000) % 10;
                            break;
                    }
                    return returnVal;
                }

                int pos = 0;
                bool NotEnd = false;
                while (!NotEnd)
                {
                    int[] optCode = OptCode((int)array[pos]);
                    decimal val1 = 0, val2 = 0, val3 = 0;

                    switch (optCode[1])
                    {
                        case 0:
                            val1 = array[(int)array[pos + 1]];
                            break;
                        case 1:
                            val1 = array[pos + 1];
                            break;
                        case 2:
                            val1 = array[(int)array[pos + 1] + (int)relativeBase];
                            break;
                        default:
                            break;
                    }

                    switch (optCode[2])
                    {
                        case 0:
                            val2 = array[(int)array[pos + 2]];
                            break;
                        case 1:
                            val2 = array[pos + 2];
                            break;
                        case 2:
                            val2 = array[(int)array[pos + 2] + (int)relativeBase];
                            break;
                        default:
                            break;
                    }

                    switch (optCode[3])
                    {
                        case 0:
                            val3 = array[pos + 3];
                            break;
                        case 2:
                            val3 = (int)array[pos + 3] + (int)relativeBase;
                            break;
                        default:
                            break;
                    }

                    switch (optCode[0])
                    {
                        case 1:
                            array[(int)val3] = val1 + val2;
                            pos = (pos + 4);
                            break;
                        case 2:
                            array[(int)val3] = val1 * val2;
                            pos = (pos + 4);
                            break;
                        case 3:
                            if (optCode[1] == 2)
                            {
                                array[(int)relativeBase + (int)array[pos + 1]] = input;// value
                            }
                            else if (optCode[1] == 1)
                            {
                                array[(int)array[pos + 1]] = input;// value
                            }
                            pos = (pos + 2);
                            break;
                        case 4:
                            Console.WriteLine("," + val1);
                            pos = (pos + 2);
                            break;
                        case 5:
                            pos = (int)(val1 != 0 ? val2 : pos + 3);
                            break;
                        case 6:
                            pos = (int)(val1 == 0 ? val2 : pos + 3);
                            break;
                        case 7:
                            array[(int)val3] = (val1 < val2 ? 1 : 0);
                            pos = (pos + 4);
                            break;
                        case 8:
                            array[(int)val3] = (val1 == val2 ? 1 : 0);
                            pos = (pos + 4);
                            break;
                        case 9:
                            relativeBase += val1;
                            pos = (pos + 2);
                            break;
                        case 99:
                            NotEnd = true;
                            break;
                        default:
                            Console.WriteLine(optCode[0]);
                            break;
                    }
                }

                return input;
            }

            runCycle(contentsMain, 2);
        }
    }
}
