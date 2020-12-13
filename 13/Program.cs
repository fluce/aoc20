using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace _13
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt").ToArray();

            long timestamp = int.Parse(lines[0]);

            long[] ids = lines[1].Split(',').Where(x => x != "x").Select(long.Parse).ToArray();

            long min = int.MaxValue;
            long min_id = 0;
            foreach (long id in ids)
            {
                long q = Math.DivRem(timestamp, id, out var remainder);
                long next = remainder == 0 ? timestamp : (q + 1) * id;
                Console.WriteLine($"id {id} => q={q} r={remainder} q*id={q * id} next={next}");
                if (next < min) { min = next; min_id = id; }
            }
            Console.WriteLine($"First departure is id {min_id} at {min} in {min - timestamp} => {(min - timestamp) * min_id}");

            var l = lines[1].Split(',');
            (int idx, long x)[] idxs = l.Zip(Enumerable.Range(0, l.Length), (x, idx) => (idx, x)).Where(x => x.x != "x").Select(x => (x.idx, x: long.Parse(x.x))).ToArray();

            foreach (var (idx, x) in idxs)
                Console.WriteLine($"{idx} {x}");
            Console.WriteLine();
            foreach (var (idx, x) in idxs)
                Console.WriteLine($"t % {x} = {(20000 * x - idx) % x}");


            var max = idxs.Maxi(x => x.x);

            var totest = idxs.Where(x => x != max).Select(i => (i.x, m: (20000 * i.x - i.idx) % i.x)).OrderByDescending(i => i.x).ToArray();

            var variable = Expression.Parameter(typeof(long));

            Expression expression = null;
            foreach (var i in totest)
            {
                var exptoadd = Expression.Equal(Expression.Modulo(variable, Expression.Constant(i.x)), Expression.Constant(i.m));
                if (expression == null)
                    expression = exptoadd;
                else
                    expression = Expression.AndAlso(expression, exptoadd);
            }
            var del = Expression.Lambda<Func<long, bool>>(expression, variable).Compile();


            CancellationTokenSource cancellationTokenSource=new CancellationTokenSource();
            
            int n=8;
            var tasks=Enumerable.Range(0,8).Select(z=>
                            Task.Factory.StartNew(()=>Calc( max.x - max.idx+z*max.x, n*max.x, del,cancellationTokenSource))).ToArray();


            Task.WaitAll(tasks);
        }

        private static void Calc(long first, long delta, Func<long, bool> del, CancellationTokenSource cts)
        {
            long count = 0;
            for (long ts = first; !cts.IsCancellationRequested; ts += delta)
            {
                if ((++count % 100000000) == 0)
                    Console.WriteLine($"{count} : {ts}");

                //                if (totest.All(x=>(ts%x.x)==x.m))
                if (del(ts))
                {
                    Console.WriteLine($"Result = {ts}");
                    cts.Cancel();
                    break;
                }
            }
        }
    }

    static class Ext
    {
        public static T Maxi<T>(this IEnumerable<T> e, Func<T,long> selector)
        {
            T maxItem=default;
            long maxValue=long.MinValue;
            foreach(var i in e) {
                var val=selector(i);
                if (val>maxValue) {
                    maxValue=val;
                    maxItem=i;
                }
            }
            return maxItem;
        }
    }
}
