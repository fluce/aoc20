using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace _17
{
    public record Coord(int x,int y, int z, int w);

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt").ToArray();

            var space=new Dictionary<Coord,char>();
            Coord min=new Coord(0,0,0,0);
            Coord max=new Coord(lines[0].Length-1,lines.Length-1,0,0);

            for(int y=0;y<lines.Length;y++)
                for(int x=0;x<lines[y].Length;x++)
                    if (lines[y][x]=='#') space[new Coord(x,y,0,0)]=lines[y][x];

            Display(min,max,space);

            var l=new[] { -1,0,1 };
            neighbours=l.SelectMany(y=>l,(x,y)=>(x,y))
                        .SelectMany(z=>l,(a,z)=>(a.x,a.y,z))
                        .SelectMany(w=>l,(a,w)=>(a.x,a.y,a.z,w))
                        .Where(c=>!(c.x==0&&c.y==0&&c.z==0&&c.w==0)).Select(c=>new Coord(c.x,c.y,c.z,c.w)).ToArray();

            Coord curmin=new Coord(min.x,min.y,min.z,min.w);
            Coord curmax=new Coord(max.x,max.y,max.z,max.w);
            Dictionary<Coord,char> curspace=space;
            for (int i=1;i<=6;i++) {
                Console.WriteLine($"Iteration {i}");
                curspace=Iterate(ref curmin, ref curmax, curspace);
                Display(curmin,curmax,curspace);
            }
            Console.WriteLine($"Total={curspace.Count(x=>x.Value=='#')}");

            curmin=new Coord(min.x,min.y,min.z,min.w);
            curmax=new Coord(max.x,max.y,max.z,max.w);
            curspace=space;
            for (int i=1;i<=6;i++) {
                Console.WriteLine($"Iteration {i}");
                curspace=Iterate4(ref curmin, ref curmax, curspace);
                //Display(curmin,curmax,curspace);
            }
            Console.WriteLine($"Total 4={curspace.Count(x=>x.Value=='#')}");

        }

        static Coord[] neighbours;

        static Dictionary<Coord,char> Iterate(ref Coord min, ref Coord max, Dictionary<Coord,char> space)
        {
            var space2=new Dictionary<Coord,char>();
            for(int z=min.z-1;z<=max.z+1;z++) 
                for(int y=min.y-1;y<=max.y+1;y++) 
                    for(int x=min.x-1;x<=max.x+1;x++) 
                    {
                        var c=new Coord(x,y,z,0);
                        var n=neighbours.Count(n=>space.TryGetValue(new Coord(n.x+x,n.y+y,n.z+z,0),out var cc) && cc=='#');
                        var active=space.TryGetValue(c,out var cc) && cc=='#';
                        if ( (active && (n==2 || n==3)) || (!active && n==3) )
                        {
                            if (c.x<min.x) min=min with {x=c.x};
                            if (c.y<min.y) min=min with {y=c.y};
                            if (c.z<min.z) min=min with {z=c.z};
                            if (c.x>max.x) max=max with {x=c.x};
                            if (c.y>max.y) max=max with {y=c.y};
                            if (c.z>max.z) max=max with {z=c.z};
                            space2[c]='#';
                        }
                    }
            return space2;
        }

        static Dictionary<Coord,char> Iterate4(ref Coord min, ref Coord max, Dictionary<Coord,char> space)
        {
            var space2=new Dictionary<Coord,char>();
            for(int w=min.w-1;w<=max.w+1;w++) 
                for(int z=min.z-1;z<=max.z+1;z++) 
                    for(int y=min.y-1;y<=max.y+1;y++) 
                        for(int x=min.x-1;x<=max.x+1;x++) 
                        {
                            var c=new Coord(x,y,z,w);
                            var n=neighbours.Count(n=>space.TryGetValue(new Coord(n.x+x,n.y+y,n.z+z,n.w+w),out var cc) && cc=='#');
                            var active=space.TryGetValue(c,out var cc) && cc=='#';
                            if ( (active && (n==2 || n==3)) || (!active && n==3) )
                            {
                                if (c.x<min.x) min=min with {x=c.x};
                                if (c.y<min.y) min=min with {y=c.y};
                                if (c.z<min.z) min=min with {z=c.z};
                                if (c.w<min.w) min=min with {w=c.w};
                                if (c.x>max.x) max=max with {x=c.x};
                                if (c.y>max.y) max=max with {y=c.y};
                                if (c.z>max.z) max=max with {z=c.z};
                                if (c.w>max.w) max=max with {w=c.w};
                                space2[c]='#';
                            }
                        }
            return space2;
        }

        static void Display(Coord min, Coord max, Dictionary<Coord,char> space)
        {
            var buf=new StringBuilder(4096);
            for(int z=min.z;z<=max.z;z++) {

                buf.AppendFormat("z={0}",z);
                buf.AppendLine();
                for(int y=min.y;y<=max.y;y++) {
                    for(int x=min.x;x<=max.x;x++) 
                        if (space.TryGetValue(new Coord(x,y,z,0), out var c)) buf.Append(c);
                        else buf.Append('.');
                    buf.AppendLine();
                }
                buf.AppendLine();
            }
            Console.WriteLine(buf);
        }
    }
}
