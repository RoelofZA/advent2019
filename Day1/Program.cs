using System;
using System.IO;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            string contents = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");
            
            decimal totalSum = 0;

            decimal CalcMass(string mass) {
                decimal temp = decimal.Parse(mass);
                temp = Math.Floor(temp / 3) - 2;
                return temp;
            }

            decimal CalcMassRecursive(decimal mass) {
                if (mass <= 0 )
                    return 0;

                decimal val = CalcMass(mass.ToString());
                decimal totalval = val>0?val + CalcMassRecursive(val):0;
                return totalval;
            }
            
            Console.WriteLine(CalcMassRecursive(12));
            Console.WriteLine(CalcMassRecursive(14));
            Console.WriteLine(CalcMassRecursive(1969));
            Console.WriteLine(CalcMassRecursive(100756));

            foreach(string line in contents.Split("\n")){
                //Console.WriteLine(CalcMass(line));
                totalSum += CalcMassRecursive(decimal.Parse(line));
            }

            Console.WriteLine(totalSum);
        }
    }
}
