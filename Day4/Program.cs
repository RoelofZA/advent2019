using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentsMain = File.ReadAllText(Environment.CurrentDirectory + "/puzzle.txt");

            int min = 128392, max = 643281;
            /*
            It is a six-digit number.
            The value is within the range given in your puzzle input. 128392-643281
            Two adjacent digits are the same (like 22 in 122345).
            Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
            */

            bool Check(int number) {
                //int length = number.ToString().Length;
                int currentVal = number % 10;
                bool hasDup = false;
                for (int num = 4; num>=0; num--){
                    int chkNum = (number/(int)(Math.Pow(10,5-num))%10);
                    if (chkNum <= currentVal) {
                        if (chkNum == currentVal)
                            hasDup = true;
                        currentVal = chkNum;
                    }
                    else
                    {
                        return false;
                    }
                }
                return hasDup;
            }

            bool CheckEx(int number) {
                //int length = number.ToString().Length;
                int currentVal = number % 10;
                Stack stack = new Stack();
                bool hasDup = false;
                for (int num = 4; num>=0; num--){
                    int chkNum = (number/(int)(Math.Pow(10,5-num))%10);
                    if (chkNum <= currentVal) {
                        if (chkNum == currentVal){
                            hasDup = true;
                            stack.Push(chkNum);
                        }
                        currentVal = chkNum;
                    }
                    else
                    {
                        return false;
                    }
                }

                bool hasCombo = false;
                while (stack.Count>0 && !hasCombo){

                    MatchCollection matches = Regex.Matches(number.ToString(), stack.Pop().ToString());
                    hasCombo = matches.Count == 2;
                }
                //if (matches.Count == 0)
                return hasCombo;
            }

            int passwordPossibleCnt = 0;
            for (int i = min; i <= max; i++)
            {
                if (CheckEx(i)){
                    Console.WriteLine(i);
                    passwordPossibleCnt++;
                }
            }

            Console.WriteLine($"{passwordPossibleCnt}");
        }
    }
}
