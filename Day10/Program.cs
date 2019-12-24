using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Diagnostics;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle04.txt");

            string[] lines = contentsMain.Split("\n");
            List<PointF> points = new List<PointF>(); 

            // Get all Points
            for (int y = 0; y < lines.Length; y++)
            {
                for (int i = 0; i < lines[y].Length; i++)
                {
                    if (lines[y][i]=='#')
                    {
                        points.Add(new PointF(i, y));
                    }
                }
            }

            int highestNumber = 0;


            foreach (PointF current in points){
                Hashtable ht = new Hashtable();
                // Start Calculating
                foreach (PointF item in points)
                {
                    if (item == current)
                        continue;

                    ht[angleOf(current, item)] = item;
                }
                //Console.WriteLine($"{current} {ht.Keys.Count}");
                if (highestNumber < ht.Keys.Count) highestNumber = ht.Keys.Count;
            }
            Console.WriteLine(highestNumber);

            //Lazer {X=30, Y=34}
            Hashtable htCurrent = new Hashtable();
            PointF centre = new PointF(30, 34);
            //PointF centre = new PointF(11, 13);
            // Start Calculating
            foreach (PointF item in points)
            {
                if (item == centre)
                    continue;

                Double angle = angleOf(centre, item);

                if (!htCurrent.ContainsKey(angle)){
                    htCurrent[angle] = new List<PointF>();
                }

                ((List<PointF>)htCurrent[angle]).Add(item);
            }

            List<double> sortOrder = new List<double>();
            foreach (var item in htCurrent.Keys.Cast<double>().Where(dd=>dd<=90).OrderByDescending(d=>d))
            {
                sortOrder.Add(item);
            }
            foreach (var item in htCurrent.Keys.Cast<double>().Where(dd=>dd<=360 && dd>90).OrderByDescending(d=>d))
            {
                sortOrder.Add(item);
            }

            int itemsKilled = 0;
            while(htCurrent.Count>0) 
            {
                foreach (var item in sortOrder)
                {
                    if (!htCurrent.ContainsKey(item))
                        continue;

                    Hashtable search = new Hashtable();
                    ((List<PointF>)htCurrent[item]).ForEach(x=>search.Add(ManDist(centre,x),x));
                    Console.WriteLine(search[search.Keys.Cast<double>().OrderBy(d=>d).First()]);
                    ((List<PointF>)htCurrent[item]).Remove((PointF)search[search.Keys.Cast<double>().OrderBy(d=>d).First()]);
                    if (((List<PointF>)htCurrent[item]).Count==0)
                    {
                        htCurrent.Remove(item);
                    }
                    itemsKilled++;
                    if (itemsKilled==200) {
                        Console.WriteLine("Target");
                    }
                }
                

            }
        }

        //{X=30, Y=34}


        /**
        * Work out the angle from the x horizontal winding anti-clockwise 
        * in screen space. 
        * 
        * The value returned from the following should be 315. 
        * <pre>
        * x,y -------------
        *     |  1,1
        *     |    \
        *     |     \
        *     |     2,2
        * </pre>
        * @param p1
        * @param p2
        * @return - a double from 0 to 360
        */
        public static double angleOf(PointF p1, PointF p2)
        {
            // NOTE: Remember that most math has the Y axis as positive above the X.
            // However, for screens we have Y as positive below. For this reason, 
            // the Y values are inverted to get the expected results.
            double deltaY = (p1.Y - p2.Y);
            double deltaX = (p2.X - p1.X);
            double result = RadianToDegree(Math.Atan2(deltaY, deltaX));
            return (result < 0) ? (360d + result) : result;
        }

        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private static double ManDist(PointF a, PointF b) {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y-b.Y);
        }

    }
}
