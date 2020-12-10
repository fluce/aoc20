using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").Select(int.Parse).OrderBy(x=>x).ToList();

            var highestRated=lines.Max();
            var buildInRate=highestRated+3;

            int currentRating=0;
            List<int> adapters=new List<int>();
            adapters.Add(currentRating);
            while(true)
            {
                var candidates=FindAdapters(currentRating,lines);
                Console.WriteLine($"CurrentRating : {currentRating}");
                Console.WriteLine($"Remaining : {string.Join(",",lines)}");
                Console.WriteLine($"Candidates : {string.Join(",",candidates)}");
                if (!candidates.Any()) break;
                int chosen=candidates.OrderBy(x=>x).First();
                lines.Remove(chosen);
                adapters.Add(chosen);
                currentRating=chosen;
            }
            adapters.Add(buildInRate);
            Console.WriteLine($"{string.Join(",",adapters)}");
            var repartition=new ConcurrentDictionary<int,int>();
            var seqList=new List<(int,int)>();
            int seqBeg=0;
            int seqEnd=0;
            for(int i=1;i<adapters.Count;i++) {
                var delta=adapters[i]-adapters[i-1];
                repartition.AddOrUpdate(delta,1,(key,val)=>val+1);
                if (delta==1) {
                    seqEnd=adapters[i];
                } else {
                    if (seqBeg!=seqEnd) seqList.Add((seqBeg,seqEnd));
                    seqBeg=adapters[i];
                    seqEnd=adapters[i];
                }
            }
            if (seqBeg!=seqEnd) seqList.Add((seqBeg,seqEnd));
            Console.WriteLine($"repartition[1]={repartition[1]} repartition[3]={repartition[3]} => {repartition[1]*repartition[3]}");

            var rep=seqList.GroupBy(x=>x.Item2-x.Item1+1).Select(x=>(x.Key,x.Count()));
            foreach(var r in rep) {
                Console.WriteLine($"{r.Key} => {r.Item2}");
            }

            long totalCombi=1;
            foreach(var (beg,end) in seqList) {
                if (end>beg+1) {
                    var diff=end-beg-1;
                    var combi=1;
                    if (diff<3)
                        combi=1<<diff;
                    else {
                        combi=(1<<diff) - 1;
                    }
                    Console.WriteLine($"Seq between {beg} and {end} => {combi}");
                    totalCombi*=combi;
                }
                else
                    Console.WriteLine($"Seq between {beg} and {end}");
            }
            Console.WriteLine($"Total combi = {totalCombi}");

        }

        static IEnumerable<int> FindAdapters(int input, List<int> lines) => lines.Where(x=>x>input && x<=input+3);
        
    }
}
