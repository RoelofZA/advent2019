using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;


namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            //Miner(File.ReadAllText(Environment.CurrentDirectory + "/puzzle1.txt"));
            //Miner(File.ReadAllText(Environment.CurrentDirectory + "/puzzle2.txt"));
            //Miner(File.ReadAllText(Environment.CurrentDirectory + "/puzzle3.txt"));
            //Miner(File.ReadAllText(Environment.CurrentDirectory + "/puzzle4.txt"));
            //Miner(File.ReadAllText(Environment.CurrentDirectory + "/puzzle5.txt"));
            Miner(File.ReadAllText(Environment.CurrentDirectory + "/puzzle6.txt"));
        }

        public static void Miner(string contentsMain) {

            Hashtable ht = new Hashtable();
            Hashtable elementHash = new Hashtable();
            Hashtable spare = new Hashtable();

            //Build hashtable of Functions
            foreach (string item in contentsMain.Split("\n"))
            {
                string[] splitStr = item.Split(" => ");
                ht.Add(splitStr[1].Split(" ")[1],splitStr);
            }

            string BuildFormula(string nameElement, int multiply=1) {
                string[] splitStr= (string[])ht[nameElement];

                if (splitStr==null)
                    return nameElement;

                string newFormula = splitStr[0];
                string[] formulaSplit = newFormula.Split(", ");

                    formulaSplit = newFormula.Split(", ");
                    newFormula = "";
                    for (int i = 0; i < formulaSplit.Length; i++)
                    {
                        string[] str = formulaSplit[i].Split(" ");
                        string subItem = str[1];
                        int val = int.Parse(str[0]) * multiply;

                        if (!str.Contains("ORE")) {
                             string[] subElement = (string[])ht[subItem];
                             string[] subElementB = subElement[1].Split(" ");

                             if (subElement[0].Contains("ORE")){
                                newFormula +=  $"{val} {subElementB[1]} + ";
                                elementHash[subElementB[1]] = elementHash.ContainsKey(subElementB[1])?(int)elementHash[subElementB[1]]+val:val;
                             }
                             else{
                                if(!spare.ContainsKey(subItem))
                                    spare[subItem] = 0;

                                int spares = (int)spare[subItem];

                                if (val>spares)
                                {
                                    int divResult = Math.DivRem(val, int.Parse(subElementB[0].ToString()), out int resVal);
                                    
                                    if (resVal!=0){
                                        if (resVal<=spares)
                                        {
                                            spare[subItem] = (int)spare[subItem] - resVal;
                                        }
                                        else
                                        {
                                            divResult++;
                                            resVal = (divResult*int.Parse(subElementB[0].ToString())) - (val-spares);
                                            spare[subItem] = resVal;      
                                        }                           
                                    }
                                    newFormula +=  $"{BuildFormula(subItem, divResult)}";
                                }
                                else
                                {
                                    spare[subItem] = (int)spare[subItem] - val;
                                }
                             }
                        }
                        else
                        {
                            newFormula +=  $"{subItem} {val}, ";
                        }
                    }
                return newFormula;
            }

            decimal totalOre = 0, counter = 0;

            while(totalOre<1000000000000) 
            {
                counter++;
                elementHash = new Hashtable();
                string baseFormula = BuildFormula("FUEL");
                //Console.WriteLine(baseFormula);
                Hashtable sumTable = new Hashtable();

                foreach (var item in elementHash.Keys)
                {
                    string[] totalSplit = elementHash[item].ToString().Split(" ");
                    string[] splitStr = (string[])ht[item];

                    string splitKey = splitStr[1].Split(" ")[1];
                    if(!spare.ContainsKey(splitKey))
                        spare[splitKey] = 0;

                    int spares = (int)spare[splitKey];

                    int divResult = Math.DivRem(int.Parse(totalSplit[0]), int.Parse(splitStr[1].Split(" ")[0]), out int resVal);
                                    
                    if (resVal!=0){
                        if (resVal<=spares)
                        {
                            spare[splitKey] = (int)spare[splitKey] - resVal;
                        }
                        else
                        {
                            divResult++;
                            resVal = (divResult*int.Parse(splitStr[1].Split(" ")[0])) - (int.Parse(totalSplit[0])-spares);
                            spare[splitKey] = resVal;      
                        }                           
                    }

                    totalOre += (int)divResult*int.Parse(splitStr[0].Split(" ")[0]);
                }
            }

            Console.WriteLine($" {totalOre} - {counter}");
            Console.WriteLine($" -1000000000000 - {counter}");
        }
    }
}
