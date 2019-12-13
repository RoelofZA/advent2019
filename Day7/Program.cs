using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");
            string copyContents = contentsMain;
            //contentsMain = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";
            //contentsMain = "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10";
            decimal[] code = new decimal[5] { 9, 8, 7, 6, 5 };
            code = new decimal[5] { 9, 7, 8, 5, 6 };
            int[] posG = {0, 0, 0, 0, 0};
            bool[] inputTriggered = {true,true,true,true,true};

            decimal high = 0;

            decimal[] array = contentsMain.Split(",").Select(decimal.Parse).ToArray();

            decimal[][] amps = new decimal[5][];
            amps[0] = (decimal[])array.Clone();
            amps[1] = (decimal[])array.Clone();
            amps[2] = (decimal[])array.Clone();
            amps[3] = (decimal[])array.Clone();
            amps[4] = (decimal[])array.Clone();
            //int[] currentPos = {0,0,0,0,0};

            int[] OptCode(int optCode){
                    int[] returnVal = new int[3] { optCode % 10, 0, 0 };

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
                    }
                    return returnVal;
                }

            List<decimal> runCycle(int amp, ref List<decimal> inputs, ref List<decimal> output)
            {
                decimal[] array = amps[amp-1];
                //decimal input1 = code[amp-1];
                
                decimal outputVal = 0;
                //List<decimal> list = new List<decimal>();

                void Recursive(int pos, ref List<decimal> input2, ref List<decimal> output)
                {
                    int[] optCode = OptCode((int)array[pos]);

                    decimal val1 = 0, val2 = 0;
                    if (optCode[0] != 4 && optCode[0] != 3 && optCode[0] != 9)
                    {
                        val1 = (optCode[1] == 0 ? array[(int)array[pos + 1]] : array[pos + 1]);
                        val2 = (optCode[2] == 0 ? array[(int)array[pos + 2]] : array[pos + 2]);
                    }

                    switch (optCode[0])
                    {
                        case 1:
                            array[(int)array[pos + 3]] = val1 + val2;
                            Recursive(pos + 4, ref input2, ref output);
                            break;
                        case 2:
                            array[(int)array[pos + 3]] = val1 * val2;
                            Recursive(pos + 4, ref input2, ref output);
                            break;
                        case 3:
                            //if (!inputTriggered[amp-1] && output.Count>0)
                            //    return;
                            //array[(int)array[pos + 1]] = input2[0];//inputTriggered[amp-1] ? input1 : input2; // value
                            if (input2.Count>0) {
                                array[(int)array[pos + 1]] = input2[0];
                                input2.RemoveAt(0);
                                }
                                
                            inputTriggered[amp-1] = false;
                            Recursive(pos + 2, ref input2, ref output);
                            break;
                        case 4:

                            outputVal = (optCode[1] == 0 ? array[(int)array[pos + 1]] : array[pos + 1]);
                            output.Add(outputVal);
                            input2.Clear();

                            high = high<outputVal?outputVal:high;
                            
                            //if (outputVal!=0)
                            //    runCycle(contentsMain, input1, outputVal);
                            //Console.WriteLine(outputVal);
                            posG[amp-1] = pos + 2;
                            //runCycle((amp>=5?1:amp+1), outputVal);
                            //Console.WriteLine((decimal)array[(int)array[pos+1]]);
                            //currentPos = pos + 2;
                            //return;
                            //Recursive(pos + 2, ref input2, ref output);
                            break;
                        case 5:
                            pos = (int)(val1 > 0 ? val2 : pos + 3);
                            Recursive(pos, ref input2, ref output);
                            break;
                        case 6:
                            pos = (int)(val1 == 0 ? val2 : pos + 3);
                            Recursive(pos, ref input2, ref output);
                            break;
                        case 7:
                            array[(int)array[pos + 3]] = (val1 < val2 ? 1 : 0);
                            Recursive(pos + 4, ref input2, ref output);
                            break;
                        case 8:
                            array[(int)array[pos + 3]] = (val1 == val2 ? 1 : 0);
                            Recursive(pos + 4, ref input2, ref output);
                            break;
                        case 9:
                        case 99:
                            posG[amp-1] = 0;
                            if (amp==1) 
                            {
                                input2.Clear();
                                output.Clear();
                            }
                            //runCycle((amp>=5?1:amp+1), outputVal);
                            break;
                    }
                }

                Recursive(posG[amp-1], ref inputs, ref output);

                return inputs;
            }

            //decimal maxVal = 0;

            // for (int x = 0; x < 5; x++)
            // {
            //     for (int y = 0; y < 5; y++)
            //     {
            //         if (x == y) continue;
            //         for (int z = 0; z < 5; z++)
            //         {
            //             if (z == y || z == x) continue;
            //             for (int v = 0; v < 5; v++)
            //             {
            //                 if (v == z || v == y || v == x) continue;
            //                 for (int b = 0; b < 5; b++)
            //                 {
            //                     if (b == z || b == y || b == x || b == v) continue;
            //                     code = new decimal[5] { x, y, z, v, b };
            //                     var ret1 = runCycle(contentsMain, code[0], 0);
            //                     var ret2 = runCycle(contentsMain, code[1], ret1);
            //                     var ret3 = runCycle(contentsMain, code[2], ret2);
            //                     var ret4 = runCycle(contentsMain, code[3], ret3);
            //                     var ret5 = runCycle(contentsMain, code[4], ret4);
            //                     //Console.WriteLine(ret5);
            //                     if (maxVal < ret5)
            //                         maxVal = ret5;
            //                 }
            //             }
            //         }
            //     }
            // }

            
            //maxVal = 0;
            //decimal baseval = -1;
            //int count = 0;

            

            

            for (int x = 5; x < 10; x++)
            {
                for (int y = 5; y < 10; y++)
                {
                    if (x == y) continue;
                    for (int z = 5; z < 10; z++)
                    {
                        if (z == y || z == x) continue;
                        for (int v = 5; v < 10; v++)
                        {
                            if (v == z || v == y || v == x) continue;
                            for (int b = 5; b < 10; b++)
                            {
                                if (b == z || b == y || b == x || b == v) continue;

                                posG = new int[] {0, 0, 0, 0, 0};
                                amps[0] = (decimal[])array.Clone();
                                amps[1] = (decimal[])array.Clone();
                                amps[2] = (decimal[])array.Clone();
                                amps[3] = (decimal[])array.Clone();
                                amps[4] = (decimal[])array.Clone();

                                var ret1 = new List<decimal>() {x, 0};
                                var ret2 = new List<decimal>() {y};
                                var ret3 = new List<decimal>() {z};
                                var ret4 = new List<decimal>() {v};
                                var ret5 = new List<decimal>() {b};

                                bool chk = true;
                                while(chk) {
                                    if (ret1.Count==0 && ret2.Count==0 && ret3.Count==0 && ret4.Count==0 && ret5.Count==0) chk = false;
                                    if (ret1.Count>0) runCycle(1, ref ret1, ref ret2 );
                                    if (ret2.Count>0) runCycle(2, ref ret2, ref ret3 );
                                    if (ret3.Count>0) runCycle(3, ref ret3, ref ret4 );
                                    if (ret4.Count>0) runCycle(4, ref ret4, ref ret5 );
                                    if (ret5.Count>0) runCycle(5, ref ret5, ref ret1 );
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine(high);
        }
    }
}
