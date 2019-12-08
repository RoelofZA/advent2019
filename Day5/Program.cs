using System;
using System.IO;
using System.Linq;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");

            //contentsMain = "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99";

            decimal runCycle(string contents, decimal input){

                decimal[] array = contents.Split(",").Select(decimal.Parse).ToArray();

                //array[1] = x;
                //array[2] = y;

                int[] OptCode(int optCode) {
                    int[] returnVal = new int[3] {optCode % 10, 0, 0};

                    int length = optCode.ToString().Length;
                    switch (length)
                    {
                        case 1:
                        case 2:
                            break;
                        case 3:
                            returnVal[1] = (optCode/100) % 10;
                            break;
                        case 4:
                            returnVal[1] = (optCode/100) % 10;
                            returnVal[2] = (optCode/1000) % 10;
                            break;
                    }
                    return returnVal;
                }

                void Recursive(int pos){

                    int[] optCode = OptCode((int)array[pos]);

                    decimal val1 = 0, val2 = 0;
                    if (optCode[0] != 4 && optCode[0] != 3 && optCode[0] != 9) {
                        val1 = (optCode[1]==0?array[(int)array[pos+1]]:array[pos+1]);
                        val2 = (optCode[2]==0?array[(int)array[pos+2]]:array[pos+2]);
                    }

                    switch (optCode[0])
                    {
                        case 1:
                            array[(int)array[pos+3]] = val1 + val2;
                            Recursive(pos+4);
                            break;
                        case 2:
                            array[(int)array[pos+3]] = val1 * val2;
                            Recursive(pos+4);
                            break;
                        case 3:
                            array[(int)array[pos+1]] = input;// value
                            Recursive(pos+2);
                            break;
                        case 4:
                            Console.WriteLine((optCode[1]==0?array[(int)array[pos+1]]:array[pos+1]));
                            //Console.WriteLine((decimal)array[(int)array[pos+1]]);
                            Recursive(pos+2);
                            break;
                        case 5:
                            pos = (int)(val1>0?val2:pos+3);
                            Recursive(pos);
                            break;
                        case 6:
                            pos = (int)(val1==0?val2:pos+3);
                            Recursive(pos);
                            break;
                        case 7:
                            array[(int)array[pos+3]] = (val1<val2?1:0);
                            Recursive(pos+4);
                            break;
                        case 8:
                            array[(int)array[pos+3]] = (val1==val2?1:0);
                            Recursive(pos+4);
                            break;
                        case 99:
                            return;
                    }
                }

                Recursive(0);

                return input;
            }

            Console.WriteLine($"{runCycle(contentsMain, 5)}");
            //Console.WriteLine($"{runCycle(contentsMain, 8)}");
            //Console.WriteLine($"{runCycle(contentsMain, 9)}");
        }

        
    }
}
