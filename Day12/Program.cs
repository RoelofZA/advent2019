using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;
            
namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculate(File.ReadAllText(Environment.CurrentDirectory + "/puzzle01.txt"), 3000);
            Calculate(File.ReadAllText(Environment.CurrentDirectory + "/puzzle02.txt"), 3000);
            Calculate(File.ReadAllText(Environment.CurrentDirectory + "/puzzle03.txt"), 1000000000);
        }

        public static void Calculate(string contents, int count) {
            List<Point3D> point = new List<Point3D>();
            List<Point3D> velocity = new List<Point3D>();
            Hashtable historyX = new Hashtable();
            Hashtable historyY = new Hashtable();
            Hashtable historyZ = new Hashtable();
            
            foreach (string item in contents.Split("\n"))
            {
                point.Add(PopulateList(item));
                velocity.Add(new Point3D(0, 0, 0));
            }

            var iCount = 0;
            bool xDone = false, yDone = false, zDone = false;
            double foundX = 0, foundY = 0, foundZ = 0;
            while(iCount<count) 
            {
                for (int i = 0; i < point.Count; i++)
                {
                    Point3D current = point[i];
                    Point3D currentVelocity = velocity[i];
                    for (int s = 0; s < point.Count; s++)
                    {
                        if (s==i)
                            continue;

                        Point3D flux = point[s];
                        
                        currentVelocity.X += flux.X>current.X?1:flux.X<current.X?-1:0;
                        currentVelocity.Y += flux.Y>current.Y?1:flux.Y<current.Y?-1:0;
                        currentVelocity.Z += flux.Z>current.Z?1:flux.Z<current.Z?-1:0;
                    }
                }

                //Apply Changes
                string checkSumX = "", checkSumY = "", checkSumZ = "";
                
                for (int i = 0; i < point.Count; i++) {
                    Point3D current = point[i];
                    Point3D currentVelocity = velocity[i];
                    current.Apply(currentVelocity);
                    checkSumX += $"{current.X},{currentVelocity.X},";
                    checkSumY += $"{current.Y},{currentVelocity.Y},";
                    checkSumZ += $"{current.Z},{currentVelocity.Z},";
                    //current.CheckSum(currentVelocity);
                    
                }
                if (!xDone) 
                {
                    xDone  = DoCheck(historyX, checkSumX, iCount, "X", ref foundX);
                }
                if (!yDone) 
                {
                    yDone  = DoCheck(historyY, checkSumY, iCount, "Y", ref foundY);
                }
                if (!zDone) 
                {
                    zDone  = DoCheck(historyZ, checkSumZ, iCount, "Z", ref foundZ);
                }

                if (xDone && yDone && zDone)
                {
                    FindRepeat(foundX, foundY, foundZ);
                    return;
                }
                

                //Snapshot


                iCount++;
            }

            //Count
            double totalSum = 0;
            for (int i = 0; i < point.Count; i++)
            {
                double sumAmount = point[i].Sum() * velocity[i].Sum();
                totalSum+= sumAmount;
            }

            Console.WriteLine($"Total: {totalSum}");
        }

        public static void FindRepeat(double foundX, double foundY, double foundZ) {
            double highNumber = Math.Max(Math.Max(foundX, foundY), foundZ);
            double lowNumber1 = foundX==highNumber?foundY:foundX;
            double lowNumber2 = foundY==highNumber || lowNumber1 == foundY?foundZ:foundY;

            bool found = false;
            int iCounter = 1;
            while(!found) {
                iCounter++;
                double highNumberCalc = highNumber*iCounter;
                if (highNumberCalc % lowNumber1 == 0 && highNumberCalc % lowNumber2 == 0){
                    Console.WriteLine($"Match Found - {iCounter*highNumber}");
                    found = true;
                    return;
                }
            }

        }

        public static bool DoCheck(Hashtable history, string checkSum, int iCount, string indicator, ref double found){
            if (history.ContainsKey(checkSum)){
                Console.WriteLine($"Match Found {indicator} - {iCount}");
                found = iCount;
                return true;
            }
            else{
                history.Add(checkSum, iCount);
            }
            return false;
        }

        public static Point3D PopulateList(string contents) {
            
            var pattern = @"<x=(.+)?, y=(.+)?, z=(.+)?>";
            var match = Regex.Match(contents, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return new Point3D(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
            }
            return null;
        }

        public class Point3D {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z  { get; set; }

            public Point3D(double x, double y, double z){
                this.X = x;
                this.Y = y;
                this.Z = z;
            }

            public Point3D(string x, string y, string z){
                this.X = double.Parse(x);
                this.Y = double.Parse(y);
                this.Z = double.Parse(z);
            }

            public double Sum() {
                return Math.Abs(this.X)+Math.Abs(this.Y)+Math.Abs(this.Z);
            }

            public void Apply(Point3D velocity) {
                this.X += velocity.X;
                this.Y += velocity.Y;
                this.Z += velocity.Z;
            }

            public string CheckSum(Point3D velocity) {
                return $"{this.X},{this.Y},{this.Z},{velocity.X},{velocity.Y},{velocity.Z},";
            }
        }
    }
}
