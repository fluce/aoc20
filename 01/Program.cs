using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _01
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers=File.ReadAllLines("input.txt").Select(x=>int.Parse(x));

            var (n1,n2)=Get2NumbersWhoseSumIs(numbers.OrderBy(x=>x).ToList(),2020);

            Console.WriteLine($"{n1} x {n2} = {n1*n2}");

            var (m1,m2,m3)=Get3NumbersWhoseSumIs(numbers.OrderBy(x=>x).ToList(),2020);

            Console.WriteLine($"{m1} x {m2} x {m3} = {m1*m2*m3}");
        }

        private static (int,int) Get2NumbersWhoseSumIs(List<int> numbers, int v)
        {
            for(int i=0;i<numbers.Count;i++)   
            {
                for(int j=i;j<numbers.Count;j++) 
                {
                    if (numbers[i]+numbers[j]==v)
                        return (numbers[i],numbers[j]);
                }
            }
            return (0,0);
        }

        private static (int,int,int) Get3NumbersWhoseSumIs(List<int> numbers, int v)
        {
            for(int i=0;i<numbers.Count;i++)   
            {
                for(int j=i;j<numbers.Count;j++) 
                {
                    for(int k=j;k<numbers.Count;k++) 
                    {
                        if (numbers[i]+numbers[j]+numbers[k]==v)
                            return (numbers[i],numbers[j],numbers[k]);
                    }
                }
            }
            return (0,0,0);
        }
    }
}
