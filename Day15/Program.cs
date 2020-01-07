using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ImageMagick;
using SkiaSharp;


namespace Day15
{
    // This code is ugly, needs to be refactored.
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle01.txt");

            Part01(contentsMain);
        }

        public static void Part01(string contents)
        {

            decimal[] array = contents.Split(",").Select(decimal.Parse).ToArray();
            decimal relativeBase = 0;

            runCycle(array, ref relativeBase, 1);

        }

        public static void DrawBoard(Hashtable ht, PointF current, string displayText = "0") {
            // crate a surface
			var info = new SKImageInfo(500, 500);
			using (var surface = SKSurface.Create(info))
			{
				// the the canvas and properties
				var canvas = surface.Canvas;

				// make sure the canvas is blank
				canvas.Clear(SKColors.White);

				// draw some text
				var paint = new SKPaint
				{
					Color = SKColors.Black,
					IsAntialias = true,
					Style = SKPaintStyle.Fill
				};

                SKPoint coord;

                foreach (PointF point in ht.Keys)
                {
                    var colour = SKColors.Purple;

                    switch ((decimal)ht[point])
                    {
                        case 0:
                            colour = SKColors.Red;
                            break;
                        case 1:
                            colour = SKColors.Green;
                            break;
                        case 3:
                            colour = SKColors.Yellow;
                            break;
                        default:
                            colour = SKColors.Purple;
                            break;
                    }
                    paint.Color = colour;
                    coord = new SKPoint(250 + (int)point.X * 10, 250 - (int)point.Y * 10);
                    canvas.DrawRect(coord.X, coord.Y, 9, -9, paint);
                }

                paint.Color = SKColors.Blue;
                coord = new SKPoint(250 + (int)current.X * 2, 250 - (int)current.Y * 2);
                canvas.DrawRect(coord.X, coord.Y, 1, -1, paint);


                paint = new SKPaint
				{
					Color = SKColors.Black,
					IsAntialias = true,
					Style = SKPaintStyle.Fill,
					TextAlign = SKTextAlign.Left,
					TextSize = 24
				};

                coord = new SKPoint(10, 20);
                canvas.DrawText(displayText, coord, paint);

				// save the file
				using (var image = surface.Snapshot())
				using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
				using (var stream = File.OpenWrite("output.png"))
				{
					data.SaveTo(stream);
				}
            }
        }

        public static void ReleaseOxygen(Hashtable ht, PointF current, PointF oxygen)
        {
            decimal[,] array = new decimal[200, 200];
            foreach (PointF point in ht.Keys)
            {
                array[100 + (int)point.X, 100 - (int)point.Y] = (decimal)ht[point];
            }

            Queue oxygenCurrent = new Queue();
            oxygenCurrent.Enqueue(new PointF(100 + oxygen.X, 100 - oxygen.Y));
            int countMinutes = 0;

            while (oxygenCurrent.Count > 0)
            {
                Queue oxygenWorker= new Queue();
                 
                while (oxygenCurrent.Count > 0)
                {
                    oxygenWorker.Enqueue(oxygenCurrent.Dequeue());
                }
                
                countMinutes++;
                while (oxygenWorker.Count > 0)
                {
                    PointF searchPos = (PointF)oxygenWorker.Dequeue();
                    PointF adjusted = new PointF(searchPos.X, searchPos.Y + 1);

                    if (array[(int)searchPos.X, (int)searchPos.Y + 1] == 1)
                    {
                        array[(int)searchPos.X, (int)searchPos.Y + 1] = 3;
                        oxygenCurrent.Enqueue(adjusted);
                        ht[new PointF(adjusted.X - 100, 100 - adjusted.Y)] = 3M;
                    }
                    adjusted = new PointF(searchPos.X, searchPos.Y - 1);
                    if (array[(int)searchPos.X, (int)searchPos.Y - 1] == 1)
                    {
                        array[(int)searchPos.X, (int)searchPos.Y - 1] = 3;
                        oxygenCurrent.Enqueue(adjusted);
                        ht[new PointF(adjusted.X - 100, 100 - adjusted.Y)] = 3M;
                    }
                    adjusted = new PointF(searchPos.X - 1, searchPos.Y);
                    if (array[(int)searchPos.X - 1, (int)searchPos.Y] == 1)
                    {
                        array[(int)searchPos.X - 1, (int)searchPos.Y] = 3;
                        oxygenCurrent.Enqueue(adjusted);
                        ht[new PointF(adjusted.X - 100, 100 - adjusted.Y)] = 3M;
                    }
                    adjusted = new PointF(searchPos.X + 1, searchPos.Y);
                    if (array[(int)searchPos.X + 1, (int)searchPos.Y] == 1)
                    {
                        array[(int)searchPos.X + 1, (int)searchPos.Y] = 3;
                        oxygenCurrent.Enqueue(adjusted);
                        ht[new PointF(adjusted.X - 100, 100 - adjusted.Y)] = 3M;
                    }
                }
                DrawBoard(ht, current, countMinutes.ToString());
            }
            DrawBoard(ht, current, countMinutes.ToString());
            Console.WriteLine($"Minutes - {countMinutes-1}");
        }

        public static decimal runCycle(decimal[] array, ref decimal relativeBase, decimal input)
        {
            Array.Resize(ref array, 1000000);
            Stack lastDirection = new Stack();
            Hashtable ht = new Hashtable();

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
            PointF current = new PointF(0, 0);
            PointF oxygen = new PointF(0, 0);
            ht[current] = (decimal)1;

            lastDirection.Push(input);

            DrawBoard(ht, current);
            bool back = false;

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

                        //tb[current]
                        var tmpSwap = current;
                        switch (input)
                        {
                            case 1:
                                tmpSwap.Y++;
                                break;
                            case 2:
                                tmpSwap.Y--;
                                break;
                            case 3:
                                tmpSwap.X--;
                                break;
                            case 4:
                                tmpSwap.X++;
                                break;
                            default:
                                Console.WriteLine($"Input - {input}");
                                break;
                        }

                        ht[tmpSwap] = val1;
                        if (val1 > 0) current = tmpSwap; ;
                        if (ht.Keys.Count % 20 == 0) DrawBoard(ht, current);

                        switch (val1)
                        {
                            case 0:
                                break;
                            case 1:
                                if (!back)
                                    lastDirection.Push(input);
                                else
                                {
                                    back = false;
                                }
                                break;
                            case 2:
                                Console.WriteLine($"Oxyen Found: {lastDirection.Count}");
                                oxygen = current;
                                if (!back)
                                    lastDirection.Push(input);
                                else
                                {
                                    back = false;
                                }
                                break;
                        }

                        if (!ht.ContainsKey(new PointF(current.X, current.Y + 1)))
                        {
                            input = 1;
                        }
                        else if (!ht.ContainsKey(new PointF(current.X, current.Y - 1)))
                        {
                            input = 2;
                        }
                        else if (!ht.ContainsKey(new PointF(current.X - 1, current.Y)))
                        {
                            input = 3;
                        }
                        else if (!ht.ContainsKey(new PointF(current.X + 1, current.Y)))
                        {
                            input = 4;
                        }
                        else
                        {
                            //Console.WriteLine($"Dead End");
                            if (lastDirection.Count == 0)
                            {
                                DrawBoard(ht, current);
                                ReleaseOxygen(ht, current, oxygen);
                                return 0;
                            }
                            input = (decimal)lastDirection.Pop();
                            switch (input)
                            {
                                case 1:
                                    input = 2;
                                    break;
                                case 2:
                                    input = 1;
                                    break;
                                case 3:
                                    input = 4;
                                    break;
                                case 4:
                                    input = 3;
                                    break;
                            }
                            back = true;
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

            return input;
        }

    }
}
