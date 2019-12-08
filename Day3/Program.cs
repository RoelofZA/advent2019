using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Drawing;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");
            Queue Line1Actions = new Queue();
            Queue Line2Actions = new Queue();

            Hashtable Line1Points = new Hashtable();
            Hashtable Line2Points = new Hashtable();


            //contentsMain = "R8,U5,L5,D3\nU7,R6,D4,L4";
            //contentsMain = "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83";
            //contentsMain = "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7";

            //int[,] array = new int[20000, 20000];
            //int[,] array = new int[500,300];
            int centerX = 0;
            int centerY = 0;
            int lowest = 0;

            //foreach (string item in contentsMain.Split("\n"))
            string[] item = contentsMain.Split("\n");
            string[] subItem1 = item[0].Split(",");
            string[] subItem2 = item[1].Split(",");

            int currentX = centerX;
            int currentY = centerY;

            int currentXX = centerX;
            int currentYY = centerY;

            // build stacks
            for (int q = 0; q < (subItem1.Length>subItem2.Length?subItem1.Length:subItem2.Length); q++)
            {
                if (subItem1.Length > q) AddStack(ref Line1Actions, subItem1[q]);
                if (subItem2.Length > q) AddStack(ref Line2Actions, subItem2[q]);
            }

            //Build Arrays
            int moveCount = 0;
            while (Line1Actions.Count > 0 || Line2Actions.Count > 0) {
                moveCount++;
                if (Line1Actions.Count > 0)
                {
                    switch (Line1Actions.Dequeue()) {
                        case "R":
                            Line1Points[new Point(++currentX, currentY)] = moveCount;
                            break;
                        case "L":
                            Line1Points[new Point(--currentX, currentY)] = moveCount;
                            break;
                        case "U":
                            Line1Points[new Point(currentX, ++currentY)] = moveCount;
                            break;
                        case "D":
                            Line1Points[new Point(currentX, --currentY)] = moveCount;
                            break;
                    }
                }

                if (Line2Actions.Count > 0)
                {
                    switch (Line2Actions.Dequeue()) {
                        case "R":
                            Line2Points[new Point(++currentXX, currentYY)] = moveCount;
                            break;
                        case "L":
                            Line2Points[new Point(--currentXX, currentYY)] = moveCount;
                            break;
                        case "U":
                            Line2Points[new Point(currentXX, ++currentYY)] = moveCount;
                            break;
                        case "D":
                            Line2Points[new Point(currentXX, --currentYY)] = moveCount;
                            break;
                    }
                }

                if (Line1Points.ContainsKey(new Point(currentXX, currentYY)))
                {
                    int val = (int)Line1Points[new Point(currentXX, currentYY)] + moveCount;
                    if (lowest>val || lowest == 0) lowest = val;
                    Console.WriteLine($"Line1 {Line1Points[new Point(currentXX, currentYY)]} {moveCount} {val}");
                }

                if (Line2Points.ContainsKey(new Point(currentX, currentY)))
                {
                    int val = (int)Line2Points[new Point(currentX, currentY)] + moveCount;
                    if (lowest>val || lowest == 0) lowest = val;
                    Console.WriteLine($"Line2 {Line2Points[new Point(currentX, currentY)]} {moveCount} {val}");
                }

            }

            Console.WriteLine(lowest);

            void AddStack(ref Queue queue, string Item) {
                int x = int.Parse(Item.Replace(Item.Substring(0,1), ""));
                for (int work = 1; work <= x; work++)
                {
                    queue.Enqueue(Item.Substring(0,1));
                }
            }

            
        }
    }
}
