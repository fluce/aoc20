using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _07
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").ToArray();

            var regex=new Regex(@"^(?<outerColor>\S+ \S+) bags contain (?:(?<none>no other bags)|(?:(?<innerCount>\d+) (?<innerColor>\S+ \S+) bags?)(?:, (?<innerCount>\d+) (?<innerColor>\S+ \S+) bags?)*).$");

            var index=new Dictionary<string,Item>();

            foreach(var line in lines) {
                var m=regex.Match(line);
                var item=new Item(  outerColor:m.Groups["outerColor"].Value, 
                                    inner:      m.Groups["innerCount"].Captures.Cast<Capture>()
                                        .Zip(   m.Groups["innerColor"].Captures.Cast<Capture>(),
                                                (a,b)=>new Child(color:b.Value,count:int.Parse(a.Value))
                                        ).ToList()
                                );
                Console.WriteLine($"outer: {item.outerColor}, inner: {string.Join("-",item.inner.Select(x=>$"{x.count} {x.color}"))}");
                index[item.outerColor]=item;
            }

            var count=index.Where(x=>x.Key!="shiny gold").Where(x=>GetAllDescendants(x.Value,index).Any(i=>i.color=="shiny gold")).Count();
            Console.WriteLine($"Count = {count}");

            int innerBagCount=CountDescendants(index["shiny gold"],index)-1;
            Console.WriteLine($"Bag Count = {innerBagCount}");
        }

        static IEnumerable<Child> GetAllDescendants(Item item, Dictionary<string,Item> index, string prefix="") 
            { 
//                Console.WriteLine($"{prefix}Item {item.outerColor}");
                foreach (var i in item.inner) {
//                    Console.WriteLine($"{prefix}Child {i.color} {i.count}");
                    yield return i;
                    foreach (var desc in GetAllDescendants(index[i.color],index,prefix+"  "))
                        yield return desc;
                }
            }

        static int CountDescendants(Item item, Dictionary<string,Item> index, string prefix="") 
            { 
//                Console.WriteLine($"{prefix}Item {item.outerColor}");
                int total=1;
                foreach (var i in item.inner) {
//                    Console.WriteLine($"{prefix}Child {i.color} {i.count}");
                    var desccount=CountDescendants(index[i.color],index,prefix+"  ");
//                    Console.WriteLine($"{prefix}Child total : {desccount*i.count}");
                    total+=desccount*i.count;
                }
//                Console.WriteLine($"{prefix}Item total {total}");
                return total;
            }

    }

    record Item(
        string outerColor,
        List<Child> inner
    );

    record Child(
        string color,
        int count
    );
}
