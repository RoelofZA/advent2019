using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");

            List<int[]> list = new List<int[]>();
            char[] charArray = contentsMain.ToArray();
            int imageLength = 25*6;

            int cnt = charArray.Length/imageLength;
            int lowest0 = 0, sumNum = 0;;

            for (int i = 0; i < cnt; i++)
            {
                int[] imageSet = new int[imageLength];
                int cnt0 = 0, cnt1 = 0, cnt2 = 0;
                for (int x = 0; x < imageLength; x++)
                {
                    imageSet[x] = int.Parse(charArray[x+(i*imageLength)].ToString());
                    switch (imageSet[x])
                    {
                        case 0:
                            cnt0++;
                            break;
                        case 1:
                            cnt1++;
                            break;
                        case 2:
                            cnt2++;
                            break;
                    }
                    
                }
                list.Add(imageSet);
                if (cnt0<lowest0 || lowest0 == 0)
                {
                    lowest0 = cnt0;
                    sumNum = cnt1*cnt2;
                }
            }

            Console.WriteLine($"{lowest0} {sumNum}");

            int[] finalImage = new int[imageLength];
            for (int x = 0; x < imageLength; x++) {
                finalImage[x]=list[0][x];
            }

            foreach (var imageSet in list)
            {
                for (int x = 0; x < imageLength; x++) {
                    if (finalImage[x]==2) {
                        finalImage[x] = imageSet[x];
                    }
                }
            }

            Console.WriteLine();
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 25; x++)
                {
                    Console.Write(finalImage[x + (y*25)]==0?" ":"X");
                }
                Console.WriteLine();
            }
        }
    }
}
