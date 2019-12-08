using System;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");
            //contents = "1,1,1,4,99,5,6,0,99";

            decimal runCycle(string contents, decimal x, decimal y){

                decimal[] array = contents.Split(",").Select(decimal.Parse).ToArray();

                array[1] = x;
                array[2] = y;

                void Recursive(int pos){

                    switch (array[pos])
                    {
                        case 1:
                            array[(int)array[pos+3]] = array[(int)array[pos+1]] + array[(int)array[pos+2]];
                            Recursive(pos+4);
                            break;
                        case 2:
                            array[(int)array[pos+3]] = array[(int)array[pos+1]] * array[(int)array[pos+2]];
                            Recursive(pos+4);
                            break;
                        case 99:
                            return;
                    }
                }
                Recursive(0);
                Console.WriteLine();
                Console.Write(array[0].ToString() + " ");
                return array[0];
            }
            
            //Recursive(0);

            //Console.WriteLine();
/* 19690720
27250724 
14020675  */
            //Console.Write(array[0].ToString() + " ");


            // find best x
            decimal x = 99;
            decimal valx=19690720;
            decimal minX = 0, maxY=99;
            
            decimal recSearch(decimal x, decimal lastVal)
            {
                decimal guess = runCycle(contentsMain, 71, x);

                if (guess == valx || guess == lastVal){
                    return x;
                }
                else if (guess > valx)
                {
                    maxY = x;
                    return recSearch( x - Math.Round((maxY-minX)/2), guess );
                }
                else
                {
                    minX = x;
                    return recSearch( x + Math.Round((maxY-minX)/2), guess );
                }
            }

            //runCycle(contentsMain, 12, 2);
            Console.Write(recSearch(x,0));
            /* foreach(var val in array) {
                Console.Write(val.ToString() + " ");
            } */
        }
    }
}
