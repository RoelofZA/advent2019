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
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle3.txt");
            int oreTotal = 0;

            Hashtable ht = new Hashtable();

            //Build hashtable
            foreach (string item in contentsMain.Split("\n"))
            {
                string[] splitStr = item.Split(" => ");
                ht.Add(splitStr[1].Split(" ")[1],splitStr);
            }

            Hashtable sumTableZ = new Hashtable();
            Hashtable elementHash = new Hashtable();
            Hashtable excess = new Hashtable();

            string BuildCalc(string nameElement, int multiply = 1) {
                string formula = "";
                string[] splitStr = (string[])ht[nameElement];

                
                //Console.WriteLine(String.Join(",",splitStr));
                
                //Console.WriteLine(splitStr[0]);
                formula = splitStr[0];

                if (!formula.Contains("ORE"))
                    Console.WriteLine($"{splitStr[1].Split(" ")[0]} {nameElement} = {formula} Multiply {multiply}");

                decimal diff = decimal.Parse(splitStr[1].Split(" ")[0]);
                multiply = (int)Math.Ceiling((decimal)multiply / diff);
                
                string[] formulaSplit = formula.Split(", ");

                for (int i = 0; i < formulaSplit.Length; i++)
                {
                    string subItem = formulaSplit[i].Split(" ")[1];

                    string[] str = formulaSplit[i].Split(" ");
                    int val = int.Parse(str[0])*multiply;

                    if (subItem=="ORE"){              
                        oreTotal += val;
                        continue;
                    }
                        

                    // Add to Part List
                    
                    
                    sumTableZ[str[1]] = sumTableZ.ContainsKey(str[1])?(int)sumTableZ[str[1]]+val:val;

                    string tmp = BuildCalc(subItem, val);

                    //Console.WriteLine($"{str[1]} {val}  -- {subItem} -- {(tmp.EndsWith("ORE")?"ORE":"")} {sumTableZ[str[1]]}");
                    
                    formulaSplit[i] = tmp.EndsWith("ORE")?formulaSplit[i]: tmp;
                    
                }

                return String.Join(",",formulaSplit);
            }

            string BuildCalc2(string nameElement, int multiply = 1, bool goDeep = false) {
                string[] splitStr = (string[])ht[nameElement];
                string formula = splitStr[0], newFormula = "";
                string[] formulaSplit = formula.Split(", ");
                int multiOld = multiply;
            
                for (int i = 0; i < formulaSplit.Length; i++)
                {
                    string[] str = formulaSplit[i].Split(" ");
                    string subItem = str[1];
                    int val = int.Parse(str[0])*multiply;

                    if (subItem != "ORE") {
                        if (!((string[])ht[subItem])[0].Contains("ORE")) {
                            newFormula += BuildCalc2(subItem, val, true);
                        }
                        else {
                            newFormula +=  $"{subItem} {val}, ";
                            elementHash[subItem] = elementHash.ContainsKey(subItem)?(int)elementHash[subItem]+val:val;
                        }
                    }
                    else
                    {
                        newFormula +=  $"{subItem} {val}, ";
                    }
                }
                return newFormula;
            }



            string baseFormula = BuildCalc2("FUEL", goDeep: true);
            Console.WriteLine(baseFormula);
            Hashtable sumTable = new Hashtable();

            int totalOre = 0;

            foreach (var item in elementHash.Keys)
            {
                string[] totalSplit = elementHash[item].ToString().Split(" ");
                string[] splitStr = (string[])ht[item];

                decimal xx = Math.Ceiling(decimal.Parse(totalSplit[0]) / decimal.Parse(splitStr[1].Split(" ")[0]));
                totalOre += (int)xx*int.Parse(splitStr[0].Split(" ")[0]);
            }
            

            Console.WriteLine (totalOre);
        }
    }
}
