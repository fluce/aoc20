using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _09
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").Select(long.Parse).ToArray();

            int preambleLength=25;
            long number=-1;
            for(int i=preambleLength+1;i<lines.Length;i++) {
                var (check,firsti,secondi)=CheckIfSumOf2(lines[i],lines[(i-preambleLength)..(i)]);
                if (!check) {
                    Console.WriteLine($" => Found ! {i} => {lines[i]}");
                    number=lines[i];
                    break;
                } else {
                    Console.WriteLine($" => {lines[i]}={lines[i-preambleLength+firsti]}+{lines[i-preambleLength+secondi]}");
                }
            }
            var (first,second)=FindContiguous(number,lines);
            var result=lines[first..second].Min()+lines[first..second].Max();
            Console.WriteLine($"{first},{second} => {result}");
        }

        static (bool,int,int) CheckIfSumOf2(long number, Span<long> numbers)
        {
            Console.Write($"{number} - {string.Join(",",numbers.ToArray().Select(x=>x.ToString()))}");
            for(int i=0;i<numbers.Length;i++)
                for(int j=i+1;j<numbers.Length;j++)
                    if (number==numbers[j]+numbers[i])
                        return (true,i,j);
            return (false,-1,-1);
        }

        static (int,int) FindContiguous(long number, long[] numbers)
        {
            for(int i=0;i<numbers.Length;i++)
                for(int j=i+1;j<numbers.Length;j++)
                    if (numbers.Skip(i).Take(j-i).Sum()==number) {
                        return (i,j);
                    }
            return (-1,-1);
        }
    }
}
