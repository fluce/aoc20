using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace _11
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").Select(x => ("."+x+".").ToCharArray()).ToArray();
            var width=lines[0].Length;
            var emptyLine=new string('.',width).ToCharArray();
            lines=new char[][]{emptyLine}.Concat(lines).Concat(new char[][]{emptyLine}).ToArray();
            var height=lines.Length;

            var adj=new (int,int)[] { (-1,-1), (-1,0), (-1,1), (1,-1), (1,0), (1,1),(0,-1),(0,1) };

            while(true)
            {
                var changes=new List<(int,int,char)>();
                for(int i=1;i<width-1;i++)
                    for(int j=1;j<height-1;j++) {
                        if (lines[j][i]!='.') {
                            var adjcount=adj.Where(x=>lines[j+x.Item2][i+x.Item1]=='#').Count();
                            //Console.WriteLine($"{i},{j} => {lines[j][i]} : {adjcount}");
                            if (lines[j][i]=='L' && adjcount==0)
                                changes.Add((i,j,'#'));
                            else
                            if (lines[j][i]=='#' && adjcount>=4)
                                changes.Add((i,j,'L'));
                        }

                    }
                foreach(var (i,j,c) in changes) 
                    lines[j][i]=c;
                if (changes.Count==0) break;
            }
            var total=lines.SelectMany(x=>x).Count(x=>x=='#');
            Console.WriteLine($"Total = {total}");

            lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").Select(x => ("."+x+".").ToCharArray()).ToArray();
            lines=new char[][]{emptyLine}.Concat(lines).Concat(new char[][]{emptyLine}).ToArray();

            while(true)
            {
                Console.WriteLine("");
                foreach(var l in lines)
                    Console.WriteLine(new string(l));
                var changes=new List<(int,int,char)>();
                for(int i=1;i<width-1;i++)
                    for(int j=1;j<height-1;j++) {
                        if (lines[j][i]!='.') {
                            var adjcount=0;
                            foreach (var dir in adj) {
                                for(int step=1;
                                    i+dir.Item1*step<width && i+dir.Item1*step>=0
                                 && j+dir.Item2*step<height && j+dir.Item2*step>=0;
                                    step++)
                                {
                                    if (lines[j+dir.Item2*step][i+dir.Item1*step]=='#') {
                                        adjcount++;
                                        break;
                                    }
                                    if (lines[j+dir.Item2*step][i+dir.Item1*step]=='L')
                                        break;
                                }
                            }
                            //Console.WriteLine($"{i},{j} => {lines[j][i]} : {adjcount}");
                            if (lines[j][i]=='L' && adjcount==0)
                                changes.Add((i,j,'#'));
                            else
                            if (lines[j][i]=='#' && adjcount>=5)
                                changes.Add((i,j,'L'));
                        }

                    }
                foreach(var (i,j,c) in changes) 
                    lines[j][i]=c;
                if (changes.Count==0) break;
                //Console.ReadLine();
            }
            total=lines.SelectMany(x=>x).Count(x=>x=='#');
            Console.WriteLine($"Total2 = {total}");


        }
    }
}
