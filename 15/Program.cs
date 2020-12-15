using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace _15
{
    class Program
    {
        static void Main(string[] args)
        {
            Run("0,3,6",10);
            Run("0,3,6",2020);
            Run("1,3,2",2020);
            Run("2,1,3",2020);
            Run("1,2,3",2020);
            Run("2,3,1",2020);
            Run("3,2,1",2020);
            Run("3,1,2",2020);
            Run("16,12,1,0,15,7,11",2020);
            Run("0,3,6",30000000);
            Run("16,12,1,0,15,7,11",30000000);
        }

        static void Run(string input, int turns=2020)
        {
            var numbers=new LinkedList<int>(input.Split(',').Select(int.Parse));
            Console.WriteLine($"Input : {string.Join(",",numbers)}");

            var index=new Dictionary<int,(int c,int idx)>();
            var current=numbers.First;
            int lastSpoken=-1;
            for(int i=1;i<=turns;i++)
            {
                //Console.WriteLine(string.Join(", ",index.Select(x=>$"{x.Key}:{x.Value}")));
                int spoken;
                if(current!=null) {
                    spoken=current.Value;
                    current=current.Next;
                } else {
                    //Console.WriteLine($"Last spoken = {lastSpoken} => {index[lastSpoken]}");
                    if (index[lastSpoken].c==1)
                        spoken=0;
                    else {
                        spoken=i-index[lastSpoken].idx-1;
                    }
                }
                if (lastSpoken!=-1)
                    index[lastSpoken]=(index[lastSpoken].c,i-1);
                if (index.TryGetValue(spoken,out var v))
                    index[spoken]=(v.c+1,v.idx);
                else
                    index[spoken]=(1,-1);
                lastSpoken=spoken;
            }
            Console.WriteLine($"Spoken number : {lastSpoken}");
        }
    }
}
