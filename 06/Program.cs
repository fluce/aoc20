using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _06
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test 1 : "+CalcYesCount(new[] {"abcx","abcy","abcz"}));
            Console.WriteLine("Test 2 : "+CalcYesCount2(new[] {"abcx","abcy","abcz"}));

            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").ToArray();

            var currentRecord=new List<string>();
            int totalCount=0;
            int totalCount2=0;

            foreach(var line in lines)
            {
                if (string.IsNullOrEmpty(line)) {
                    totalCount+=CalcYesCount(currentRecord);
                    totalCount2+=CalcYesCount2(currentRecord);
                    currentRecord.Clear();
                } else
                    currentRecord.Add(line);
            }
            totalCount+=CalcYesCount(currentRecord);
            totalCount2+=CalcYesCount2(currentRecord);
            Console.WriteLine("Count : "+totalCount);
            Console.WriteLine("Count2 : "+totalCount2);
        }

        static int CalcYesCount(IEnumerable<string> list)
        {
            return list.SelectMany(x=>x).Distinct().Count();
        }

        static int CalcYesCount2(IEnumerable<string> list)
        {
            IEnumerable<char> intersection=null;
            foreach(var s in list)
                if (intersection==null) intersection=s;
                else intersection=intersection.Intersect(s);
            var c=intersection.Count();
            //Console.WriteLine($"{string.Join(",",list)} => {c}");
            return c;
        }


    }
}
