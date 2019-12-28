using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
            
namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle02.txt");

            Part1(contentsMain);
            Part2(contentsMain);
        }

        public static void Part1(string contentsMain) {
            double relativeBase = 0;
            List<Tile> tiles = new List<Tile>();

            double runCycle(string contents, double input)
            {

                double[] array = contents.Split(",").Select(double.Parse).ToArray();
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
                        default:
                            Console.WriteLine($"Unknown length: {length}");
                            break;
                    }
                    return returnVal;
                }

                int pos = 0;
                bool NotEnd = false;
                int countSegment = 0;
                Tile tmpTile = null;
                while (!NotEnd)
                {
                    int[] optCode = OptCode((int)array[pos]);
                    double val1 = 0, val2 = 0, val3 = 0;

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

                    if (optCode[0]!=99)
                    {
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

                            switch (countSegment)
                            {
                                case 0:
                                    tmpTile = new Tile(val1, 0, 0);
                                    countSegment++;
                                    break;
                                case 1:
                                    tmpTile.Y = val1;
                                    countSegment++;
                                    break;
                                case 2:
                                    tmpTile.TileId = (int)val1;
                                    tiles.Add(tmpTile);
                                    countSegment=0;
                                    break;
                                default:
                                    Console.WriteLine($"Error - {countSegment}");
                                    break;
                            }
                            //Console.WriteLine("," + val1);
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

            runCycle(contentsMain, 1);

            PrintTiles(tiles);
        }

        public static void Part2(string contentsMain) {
            double relativeBase = 0;
            List<Tile> tiles = new List<Tile>();
            //double[] board = new double(42,30);
            PointF ball = new PointF(0, 0);
            PointF paddle = new PointF(0, 0);

            double runCycle(string contents, double input)
            {
                double[] array = contents.Split(",").Select(double.Parse).ToArray();
                array[0]=2;
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
                        default:
                            Console.WriteLine($"Unknown length: {length}");
                            break;
                    }
                    return returnVal;
                }

                int pos = 0;
                bool NotEnd = false;
                int countSegment = 0, score = 0;
                Tile tmpTile = null;
                while (!NotEnd)
                {
                    int[] optCode = OptCode((int)array[pos]);
                    double val1 = 0, val2 = 0, val3 = 0;

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

                    if (optCode[0]!=99)
                    {
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
                            //PrintTiles(tiles);

                            if (ball.X > paddle.X) {
                                input = 1;
                            } else if (ball.X < paddle.X) {
                                input = -1;
                            } else {
                                input = 0;
                            }

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

                            switch (countSegment)
                            {
                                case 0:
                                    tmpTile = new Tile(val1, 0, 0);
                                    countSegment++;
                                    break;
                                case 1:
                                    tmpTile.Y = val1;
                                    countSegment++;
                                    break;
                                case 2:
                                    tmpTile.TileId = (int)val1;
                                    if (tmpTile.TileId==4) ball = new PointF((float)tmpTile.X, (float)tmpTile.Y);
                                    if (tmpTile.TileId==3) paddle = new PointF((float)tmpTile.X, (float)tmpTile.Y);
                                    //tiles.Add(tmpTile);

                                    if (tmpTile.X == -1 && tmpTile.Y == 0)
                                        score=(int)val1;

                                    countSegment=0;
                                    break;
                                default:
                                    Console.WriteLine($"Error - {countSegment}");
                                    break;
                            }
                            //Console.WriteLine("," + val1);
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

                Console.WriteLine($"Score = {score}");
                
                return input;
            }

            runCycle(contentsMain, 1);

            
        }

        public static void PrintTiles(List<Tile> tiles){
            double y = 0;
            int blockTiles = 0;
            foreach(Tile tile in tiles){
                if (tile.Y != y) {
                    Console.WriteLine();
                    y=tile.Y;
                }

                string strValue = "";
                switch (tile.TileId)
                {
                    case 0: //Wall
                        strValue=" ";
                        break;
                    case 1: //Wall
                        strValue="#";
                        break;
                    case 2: //Block
                        strValue="B";
                        blockTiles++;
                        break;
                    case 3: //Paddle
                        strValue="=";
                        break;
                    case 4: //Ball
                        strValue="*";
                        break;
                    default:
                        strValue=tile.TileId.ToString();
                        break;
                }
                Console.Write(strValue);
            }
            Console.WriteLine();
            Console.WriteLine($"blockTiles = {blockTiles}");
            Console.WriteLine();
        }

        public class Tile{
            public Tile(double x, double y, int tileId){
                this.X=x;
                this.Y=y;
                this.TileId=tileId;
            }
            public double X { get; set; }
            public double Y { get; set; }
            public int TileId { get; set; }

        }
    }
}
