using System;
using System.IO;
using System.Linq;

namespace _03
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").Select(x => x.ToCharArray()).ToArray();

            (int dx, int dy) slope = (3, 1);

            long count = CountTrees(lines, slope);
            Console.WriteLine($"count={count}");

            (int dx, int dy)[] slopes = new[] { (3,1),(1, 1),(5,1),(7,1),(1,2) };

            long productofcount=1;
            foreach(var c in slopes.Select(x=>CountTrees(lines,x)))
                productofcount*=c;

            Console.WriteLine($"productofcount={productofcount}");

        }

        private static long CountTrees(char[][] lines, (int dx, int dy) slope)
        {
            var width = lines[0].Length;
            var height = lines.Length;

            char Get((int x, int y) p) => lines[p.y][p.x % width];

            (int x, int y) pos = (0, 0);

            long count = 0;
            while (pos.y < height)
            {
                if (Get(pos) == '#') count++;
                pos = (pos.x + slope.dx, pos.y + slope.dy);
            }

            return count;
        }
    }
}
