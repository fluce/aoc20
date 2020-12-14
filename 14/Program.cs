using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace _14
{
    class Record {}
    class Mask:Record {
        public string MaskValue {get;set;}
    }
    class Mem:Record {
        public long Address {get;set;}
        public long Value {get;set;}
    }
    class Program
    {
        static Regex regExMask=new Regex(@"^(mask = (?<mask>[01X]{36})|mem\[(?<address>\d+)\] = (?<value>\d+))$");
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt").ToArray();

            var prog=lines.Select( x => regExMask.Match(x) )
                 .Select( x => x.Groups["mask"].Success ?
                                    new Mask{MaskValue=x.Groups["mask"].Value} as Record
                                  : new Mem{Address=long.Parse(x.Groups["address"].Value), Value=long.Parse(x.Groups["value"].Value)} as Record )
                 .ToList();
            
            string currentMask=null;
            Dictionary<long,long> memMap=new Dictionary<long, long>();
            string pad=new string('0',36);

            foreach(var rec in prog)
            {
                switch (rec)
                {
                    case Mask mask: currentMask=mask.MaskValue; break;
                    case Mem mem: {
                        
                        var bin=Convert.ToString(mem.Value,2).PadLeft(36,'0');
                        long value=0;
                        var v=String.Create(36,0,(s,t)=>{
                            for(int i=0;i<36;i++) {
                                s[i]=currentMask[i];
                                if (s[i]=='X') s[i]=bin[i];
                                if (s[i]=='1')
                                    value|=1L<<(35-i);
                            }
                        });
                        /*
                        Console.WriteLine($"# {currentMask}");
                        Console.WriteLine($"@ {bin}");
                        Console.WriteLine($"> {v} => {value}");
                        */
                        memMap[mem.Address]=value;
                        break;
                    }
                }
            }

            Console.WriteLine($"Result = {memMap.Values.Sum()}");

            currentMask=null;
            memMap.Clear();

            foreach(var rec in prog)
            {
                switch (rec)
                {
                    case Mask mask: currentMask=mask.MaskValue; break;
                    case Mem mem: {
                        
                        var bin=Convert.ToString(mem.Address,2).PadLeft(36,'0');
                        long baseaddress=0;
                        List<int> changes=new List<int>();
                        var v=String.Create(36,0,(s,t)=>{
                            for(int i=0;i<36;i++) {
                                s[i]=bin[i];
                                if (currentMask[i]!='0') s[i]=currentMask[i];
                                if (s[i]=='X')
                                    changes.Add(36-i);
                                if (s[i]=='1')
                                    baseaddress|=1L<<(35-i);
                            }
                        });

                        /*
                        Console.WriteLine($"# {currentMask}");
                        Console.WriteLine($"@ {bin}");
                        Console.WriteLine($"> {v}");
                        Console.WriteLine($"{string.Join(",",changes.Select(x=>x.ToString()))}");
                        */
                        if (changes.Count==0) {
                            memMap[baseaddress]=mem.Value;
                            //Console.WriteLine($"! {Convert.ToString(baseaddress,2).PadLeft(36,'0')} {baseaddress}=>{mem.Value}");
                        }
                        else
                            for(int i=0;i<(1<<(changes.Count));i++)
                            {
                                long address=baseaddress;
                                for(int j=0;j<changes.Count;j++) 
                                {
                                    if ((i & (1<<j))!=0)
                                        address |= 1L<<(changes[j]-1);
                                }
                                memMap[address]=mem.Value;
                                //Console.WriteLine($"! {Convert.ToString(i,2).PadLeft(changes.Count,'0')}  {Convert.ToString(address,2).PadLeft(36,'0')} {address}=>{mem.Value}");
                            }
                        
                        break;
                    }
                }
            }
            Console.WriteLine($"Result = {memMap.Values.Sum()}");


        }
    }
}
