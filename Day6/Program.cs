using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");
            //contentsMain = "";
            Hashtable locations = new Hashtable();

            foreach (string line in contentsMain.Split("\n")) {
                
                string[] splitString = line.Split(")");
                //string pointA = line.Substring(0,3), pointB = line.Substring(4,3), action = line.Substring(3,1);
                string pointA = splitString[0], pointB = splitString[1];

                Celestial itemA, itemB;
                
                itemA = locations.ContainsKey(pointA)? (Celestial)locations[pointA] : new Celestial(pointA);
                itemB = locations.ContainsKey(pointB)? (Celestial)locations[pointB] : new Celestial(pointB);

                itemB.Parent = itemA;

                locations[pointA] = itemA;
                locations[pointB] = itemB;
            }

            int CountCelestial(Celestial item) {
                if (item == null)
                    return 0;

                return 1 + CountCelestial(item.Parent);
            }

            int totalCount = 0;
            // Count Part

            void AnswerA() {
                foreach(Celestial item in locations.Values)
                {
                    totalCount += CountCelestial(item.Parent);
                }
            }

            void AnswerB() {
                //YOU and SAN

                var you = (Celestial)locations["YOU"];
                var san = (Celestial)locations["SAN"];

                Hashtable youHash = new Hashtable();

                void NameCelestial(Celestial item, int orbits, bool checkName) {
                    if (item == null)
                        return;

                    if (checkName)
                    {
                        
                        if (youHash.ContainsKey(item.Name)){
                            Console.WriteLine();
                            Console.WriteLine($"XXXXX {item.Name} {orbits} {youHash[item.Name]} {(int)youHash[item.Name]+orbits}");
                            return;
                        }
                        else
                        {
                            orbits++;
                            Console.Write($"{item.Name} {orbits}-");
                        }
                            
                    }
                    else
                    {
                        youHash.Add(item.Name, ++orbits);
                        Console.Write($"{item.Name} {orbits}-");
                    }

                    NameCelestial(item.Parent, orbits, checkName);
                }

                //Build collection of all YOUs orbits
                Console.WriteLine();
                NameCelestial(you.Parent.Parent, 0, false);

                Console.WriteLine();
                //Build same for SAN, however check if it is already in YOUs
                NameCelestial(san.Parent, 0, true);

                // Count YOUs till shared item.

            }

            AnswerB();

            Console.WriteLine($"{totalCount}");

            
        }
    }

    class Celestial {
        public Celestial(string name) {
            this.Name = name;
        }
        public string Name { get; set; }
        public Celestial Parent { get; set; }
        public List<Celestial> Children { get; set; } = new List<Celestial>();
    }
}
