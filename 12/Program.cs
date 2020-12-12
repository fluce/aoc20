using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace _12
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").Select(x=>(instr:x.Substring(0,1),param:int.Parse(x.Substring(1)))).ToArray();
            Part1(lines);
            Part2(lines);
        }

        static void Part1((string instr, int arg)[] lines)
        {

            int currentDir=90; // facing east
            (int x, int y) pos=(0,0);

            foreach(var (instr,param) in lines)
            {
                switch(instr)
                {
                    case "N": pos.y+=param; break;
                    case "S": pos.y-=param; break;
                    case "E": pos.x+=param; break;
                    case "W": pos.x-=param; break;
                    case "L": currentDir-=param; break;
                    case "R": currentDir+=param; break;
                    case "F": pos=(pos.x+(int)(Math.Sin(currentDir*Math.PI/180)*param),pos.y+(int)(Math.Cos(currentDir*Math.PI/180)*param)); break;
                }
                Console.WriteLine($"Final pos after {(instr,param)}: {pos} {currentDir}");
            }
            Console.WriteLine($"Final pos : {pos} => dist={Math.Abs(pos.x)+Math.Abs(pos.y)}");
        }

        static void Part2((string instr, int arg)[] lines)
        {
            (int x, int y) waypointPos=(10,1);
            int currentDir=90; // facing east
            (int x, int y) pos=(0,0);

            foreach(var (instr,param) in lines)
            {
                switch(instr)
                {
                    case "N": waypointPos.y+=param; break;
                    case "S": waypointPos.y-=param; break;
                    case "E": waypointPos.x+=param; break;
                    case "W": waypointPos.x-=param; break;
                    case "L": 
                            waypointPos=(
                                (int)Math.Round(waypointPos.x*Math.Cos(param*Math.PI/180)-waypointPos.y*Math.Sin(param*Math.PI/180)),
                                (int)Math.Round(waypointPos.x*Math.Sin(param*Math.PI/180)+waypointPos.y*Math.Cos(param*Math.PI/180)) 
                            )
                              ; break;
                    case "R": 
                            waypointPos=(
                                (int)Math.Round(waypointPos.x*Math.Cos(-param*Math.PI/180)-waypointPos.y*Math.Sin(-param*Math.PI/180)),
                                (int)Math.Round(waypointPos.x*Math.Sin(-param*Math.PI/180)+waypointPos.y*Math.Cos(-param*Math.PI/180)) 
                            )
                              ; break;
                    case "F": 
                            pos=(pos.x+waypointPos.x*param,pos.y+waypointPos.y*param);
                            break;
                }
                Console.WriteLine($"Final pos after {(instr,param)}: {pos} {waypointPos}");
            }
            Console.WriteLine($"Final pos : {pos} => dist={Math.Abs(pos.x)+Math.Abs(pos.y)}");
        }
    }
}
