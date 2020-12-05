using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _05
{
    class Program
    {
        static void Main(string[] args)
        {
            DecodeBoardingPass("BFFFBBFRRR");
            DecodeBoardingPass("FFFBBBFRRR");
            DecodeBoardingPass("BBFFBBFRLL");

            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").ToArray();

            var decoded=lines.Select(DecodeBoardingPass).ToList();
            Console.WriteLine(decoded.Max(x=>x.Item3));

            decoded.Select(x=>x.Item3).OrderBy(x=>x).Aggregate(-100,
                (previous,current)=>{ 
                    if (previous==current-2) {
                        Console.WriteLine($"Found : {current-1}"); 
                    }
                    return current; 
                });

        }

        static (int,int,int) DecodeBoardingPass(string entry)
        {
            var bin=entry.Replace("F","0").Replace("B","1").Replace("L","0").Replace("R","1");
            var seatId=Convert.ToInt32(bin,2);
            var row=seatId>>3;
            var column=seatId & 0b111;
            Console.WriteLine($"{entry}: row {row}, column {column}, seat ID {seatId}");
            return (row,column,seatId);
        }
    }
}
