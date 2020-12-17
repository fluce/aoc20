using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace _16
{
    class Program
    {
        static Regex regex=new Regex(@"^(?<field>[^:]+): ((?<int1>\d+)-(?<int2>\d+))( or ((?<int1>\d+)-(?<int2>\d+)))*$");
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt").ToArray();

            int part=0;
            Dictionary<string,List<(int,int)>> dico=new Dictionary<string, List<(int, int)>>();

            List<int> myticket=null;
            List<List<int>> nearbyTickets=new List<List<int>>();

            foreach(var l in lines)
            {
                if (l.Length==0) part++;
                else {

                    switch (part)
                    {
                        case 0:
                            var m=regex.Match(l);
                            var field=m.Groups["field"].Value;
                            var intervals=m.Groups["int1"].Captures.OfType<Capture>().Zip(m.Groups["int2"].Captures.OfType<Capture>(),(i1,i2)=>(int.Parse(i1.Value),int.Parse(i2.Value)));

                            dico[field]=intervals.ToList();

                            break;
                        case 1:
                            if (!l.StartsWith("your")) {
                                myticket=l.Split(',').Select(int.Parse).ToList();
                            }
                            break;
                        case 2:
                            if (!l.StartsWith("nearby")) {
                                nearbyTickets.Add(l.Split(',').Select(int.Parse).ToList());
                            }
                            break;
                    }

                }
            }

            var allValidIntervals=dico.Values.SelectMany(x=>x).ToList();
            Console.WriteLine($"All valid intervals: {string.Join(",",allValidIntervals.Select(x=>x.ToString()))}");

            int sum=0;
            List<List<int>> maybeValidTickets=new List<List<int>>();
            foreach(var t in nearbyTickets)
            {
                var certainlyInvalidFields=t.Where(x=>!allValidIntervals.Any(interval=>x>=interval.Item1 && x<=interval.Item2)).ToList();
                if (certainlyInvalidFields.Count>0) {
                    Console.WriteLine($"Invalid fields : {string.Join(" - ",certainlyInvalidFields.Select(x=>x.ToString()))}");
                    sum+=certainlyInvalidFields.Sum();
                } else {
                    maybeValidTickets.Add(t);
                }
            }
            Console.WriteLine($"Ticket scanning error rate : {sum}");

            var indexes=Enumerable.Range(0,myticket.Count).ToList();
            bool foundSomething=true;
            
            long result=1;

            while (indexes.Count>0 && foundSomething) {
                foundSomething=false;
                List<int> needRemoval=new List<int>();
                foreach (var (idx,allValues) in indexes.Select(x=>(x,maybeValidTickets.Select(f=>f[x]).ToList())))
                {
                    var fields=dico.Where(x=>allValues.All(y=>x.Value.Any(z=>z.Item1<=y && z.Item2>=y))).ToList();
                    if (fields.Count==1) {
                        var fieldName=fields[0].Key;
                        Console.WriteLine($"{idx}: {fieldName}");
                        dico.Remove(fieldName);
                        needRemoval.Add(idx);
                        foundSomething=true;
                        if (fieldName.StartsWith("departure")) {
                            result*=myticket[idx];
                        }
                    } /*else {
                        Console.WriteLine($"{idx}: error : {string.Join(",",fields.Select(x=>x.Key))}");
                    }*/
                }
                indexes.RemoveAll(x=>needRemoval.Contains(x));
            }

            Console.WriteLine($"Result : {result}");

        }
    }
}
