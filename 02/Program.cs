using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace _02
{
    class Program
    {
        static void Main(string[] args)
        {
            var rg=new Regex(@"^(?<min>\d+)-(?<max>\d+) (?<letter>.): (?<password>.*)$");
            var lines=File.ReadAllLines("input.txt")
                          .Select(x=>rg.Match(x))
                          .Select(x=> new Line(
                                int.Parse(x.Groups["min"].Value),
                                int.Parse(x.Groups["max"].Value),
                                char.Parse(x.Groups["letter"].Value),
                                x.Groups["password"].Value)
                            );
            
            var count=lines.Count(x=>Check(x));
            Console.WriteLine($"count {count}");
            var count2=lines.Count(x=>Check2(x));
            Console.WriteLine($"count2 {count2}");
        }

        static bool Check(Line line) 
        {
            var c=line.Password.Count(c=>c==line.Letter);
            return c>=line.Min && c<=line.Max;
        }

        static bool Check2(Line line) 
        {
            var c1=line.Password[line.Min-1]==line.Letter;
            var c2=line.Password[line.Max-1]==line.Letter;
            return c1 ^ c2;
        }
    }

    internal class Line
    {
        public int Min { get; }
        public int Max { get; }
        public char Letter { get; }
        public string Password { get; }

        public Line(int min, int max, char letter, string password)
        {
            Min = min;
            Max = max;
            Letter = letter;
            Password = password;
        }

        public override bool Equals(object obj)
        {
            return obj is Line other &&
                   Min == other.Min &&
                   Max == other.Max &&
                   Letter == other.Letter &&
                   Password == other.Password;
        }

        public override int GetHashCode()
        {
            int hashCode = 75545093;
            hashCode = hashCode * -1521134295 + Min.GetHashCode();
            hashCode = hashCode * -1521134295 + Max.GetHashCode();
            hashCode = hashCode * -1521134295 + Letter.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            return hashCode;
        }
    }
}
