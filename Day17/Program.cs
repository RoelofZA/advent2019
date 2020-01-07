using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle01.txt");

            //var list = runCycle(contentsMain, 1);

            //BuildRoute(list, contentsMain);
            Part02(contentsMain);

        }

        public static List<PointF> runCycleNormal(string contents, decimal input, Queue inputQueue)
        {
            decimal relativeBase = 0;
            decimal[] array = contents.Split(",").Select(decimal.Parse).ToArray();
            Array.Resize(ref array, 1000000);
            StringBuilder sb = new StringBuilder();
            List<PointF> track = new List<PointF>();
            int x = 0, y = 0, maxX = 0, maxY = 0;

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
            bool inputDequeued = false;
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

                if (optCode[0] != 99) {

                

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
                    inputDequeued = true;
                        if (optCode[1] == 2)
                        {
                            array[(int)relativeBase + (int)array[pos + 1]] = (int)inputQueue.Dequeue();// value
                        }
                        else if (optCode[1] == 0)
                        {
                            array[(int)array[pos + 1]] = (int)inputQueue.Dequeue();;// value
                        }
                        pos = (pos + 2);
                        break;
                    case 4:
                        if ((int)val1 < 1000)
                            Console.Write($"{Convert.ToChar((int)val1)}");
                        else
                        {
                            Console.Write($"{(int)val1}");
                        }
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

            return track;
        }


        public static List<PointF> runCycle(string contents, decimal input)
        {
            decimal relativeBase = 0;
            decimal[] array = contents.Split(",").Select(decimal.Parse).ToArray();
            Array.Resize(ref array, 1000000);
            StringBuilder sb = new StringBuilder();
            List<PointF> track = new List<PointF>();
            int x = 0, y = 0, maxX = 0, maxY = 0;

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
                        else if (optCode[1] == 0)
                        {
                            array[(int)array[pos + 1]] = input;// value
                        }
                        pos = (pos + 2);
                        break;
                    case 4:
                        //Console.Write(Convert.ToChar((int)val1));
                        sb.Append(Convert.ToChar((int)val1).ToString());

                        if (val1 == 10) {
                            y++;
                            maxX = x;
                            x=0;
                            maxY = y;
                        }
                        else
                        {
                            if (val1 == 35 || val1 == 94) {
                                track.Add(new PointF(x, y));
                            }
                            x++;
                        }

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

            Calc(track, maxX, maxY);

            return track;
        }

        public static void Calc(List<PointF> points, int maxX, int maxY) {
            int sumAmount = 0;
            foreach (var point in points)
            {
                if ( 
                    (
                        Convert.ToInt32(points.Contains(new PointF(point.X+1, point.Y))) + 
                        Convert.ToInt32(points.Contains(new PointF(point.X-1, point.Y))) + 
                        Convert.ToInt32(points.Contains(new PointF(point.X, point.Y+1))) +
                        Convert.ToInt32(points.Contains(new PointF(point.X, point.Y-1)))
                    ) > 2 ){
                    sumAmount += (int)(point.X * point.Y);
                }
                
            }
            Console.WriteLine($"Sum of Points = {sumAmount}");
        }

        public static void BuildRoute(List<PointF> track, string contents = ""){

            bool moveFound = true;
            var point = track[0];
            string currDir = "U";

            StringBuilder sb = new StringBuilder();

            while(moveFound) {
                var tmpDir = "";
                bool inverted = false;
                if (currDir == "U" || currDir == "D"){
                    inverted = currDir == "D";
                    if (track.Contains(new PointF(point.X+1, point.Y)))
                    {
                        tmpDir = "R";
                    } else if (track.Contains(new PointF(point.X-1, point.Y)))
                    {
                        tmpDir = "L";
                    } else {
                        Console.WriteLine("Oops");
                        break;
                    }
                    currDir = tmpDir;

                    if (inverted) {
                        tmpDir = tmpDir=="R"?"L":"R";
                    }
                } 
                else if (currDir == "R" || currDir == "L")
                {
                    inverted = currDir == "L";
                    if (track.Contains(new PointF(point.X, point.Y+1)))
                    {
                        tmpDir = "R";
                    } else if (track.Contains(new PointF(point.X, point.Y-1)))
                    {
                        tmpDir = "L";
                    } else {
                        Console.WriteLine("Oops");
                        break;
                    }
                    currDir = tmpDir=="R"?"D":"U";

                    if (inverted) {
                        tmpDir = tmpDir=="R"?"L":"R";
                    }
                }

                int change = 1;

                switch (currDir)
                {
                    case "U":
                        while ( track.Contains(new PointF(point.X, point.Y-1)) ) {
                            change++;
                            point = new PointF(point.X, point.Y-1);
                        }
                        break;
                    case "D":
                        while ( track.Contains(new PointF(point.X, point.Y+1)) ) {
                            change++;
                            point = new PointF(point.X, point.Y+1);
                        }
                        break;
                    case "R":
                        while ( track.Contains(new PointF(point.X+1, point.Y)) ) {
                            change++;
                            point = new PointF(point.X+1, point.Y);
                        }
                        break;
                    case "L":
                        while ( track.Contains(new PointF(point.X-1, point.Y)) ) {
                            change++;
                            point = new PointF(point.X-1, point.Y);
                        }
                        break;

                    default:
                        return;
                }

                Console.Write($"{tmpDir},{change},");
                sb.Append($"{tmpDir},{change},");
            }
            Console.WriteLine();
            var pathStr = sb.ToString();
            var splitPath = pathStr.Split(",");
            var iteration = splitPath.Length-2;
            List<string> groups = new List<string>();
            List<string> groupStart = new List<string>();
            List<string> groupEnd = new List<string>();
            List<string> groupMid = new List<string>();

            Console.WriteLine("XXX  " + pathStr);

            for (int i = 21; i >= 3 ; i--)
            {
                for (int u = 0; u < splitPath.Length-1-i; u++)
                {
                    var tmpSearch = string.Join(",", splitPath[u..(u+i)]) + ",";
                    //Console.WriteLine(tmpSearch);

                    int Count = Regex.Matches(pathStr, Regex.Escape(tmpSearch)).Count;
                    if (Count>1 && !groups.Contains(tmpSearch)) {
                        //Console.WriteLine($"{tmpSearch} =\tLength {tmpSearch.Length}\tCnt {Count} ");
                        groups.Add(tmpSearch);

                        if (pathStr.StartsWith(tmpSearch))
                            groupStart.Add(tmpSearch);
                        else if (pathStr.EndsWith(tmpSearch))
                             groupEnd.Add(tmpSearch);
                        else
                            groupMid.Add(tmpSearch);
                    }
                }
            }

            foreach (var itemStart in groupStart)
            {
                foreach (var itemMid in groupMid)
                {
                    foreach (var itemEnd in groupEnd)
                    {
                        string testStr = pathStr.Replace(itemStart, "A").Replace(itemMid, "B").Replace(itemEnd, "C");
                        if (!testStr.Contains("R") && !testStr.Contains("L")) {
                            Console.WriteLine($"{testStr}\t{itemStart}\t{itemMid}\t{itemEnd}");
                        }
                    }
                }
            }


            
        }

        public static void Part02(string contentsMain) {
            string prog = "A,B,A,B,C,B,C,A,B,C";
            string[] funcs = {prog, "R,4,R,10,R,8,R,4", "R,10,R,6,R,4", "R,4,L,12,R,6,L,12"};
            //ABABCBCABC	R,5,R,11,R,9,R,5,	R,11,R,7,R,5,	R,5,L,13,R,7,L,13,

            // Convert to Ascii and put in queue
            Queue inputQueue = new Queue();
            foreach (var item in funcs)
            {
                foreach (var charItem in item)
                {
                    inputQueue.Enqueue((int)charItem);
                }
                //inputQueue.Enqueue((int)',');
                inputQueue.Enqueue((int)10);
            }

            inputQueue.Enqueue((int)'n');
            inputQueue.Enqueue((int)10);

            // Update the Array
            contentsMain = "2" + contentsMain.Substring(1,contentsMain.Length-1);

            // Run program
            runCycleNormal(contentsMain, 1, inputQueue);
        }
    }
}
