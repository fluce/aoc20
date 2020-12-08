using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _08
{
    record Instruction
    {
        public string Operator;

        public string Operand;

        public int OperandAsInt=>int.Parse(Operand);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").ToArray();

            var regex=new Regex(@"^(?<token>\w+)\s+(?<value>[+-]*\d+)$");

            var program=lines.Select(x=>regex.Match(x)).Select(x=>new Instruction { Operator=x.Groups["token"].Value, Operand=x.Groups["value"].Value }).ToList();

            var (result,ip)=Run(program);
            Console.WriteLine($"Accumulator {result}");

            for(int i=0;i<program.Count;i++)            
            {
                var backup=program[i];
                if (program[i].Operator=="nop") program[i]=backup with { Operator="jmp" };
                else
                    if (program[i].Operator=="jmp") program[i]=backup with { Operator="nop" };
                    else
                        continue;

                Console.WriteLine($"Changing {i} {backup}=>{program[i]}");
                
                (result,ip)=Run(program);
                if (ip==program.Count)
                {
                    Console.WriteLine($"{i} {backup} => Accumulator {result}");
                    break;
                }

                program[i]=backup;
            }



        }

        static (int,int) Run(List<Instruction> program)
        {
            int accumulator=0;
            var counters=new int[program.Count];
            var instructionPointer=0;

            while (true)
            {
                var instruction=program[instructionPointer];
                Console.WriteLine($"IP={instructionPointer} {instruction.Operator} {instruction.OperandAsInt}");
                
                if (counters[instructionPointer]==1) break;
                
                counters[instructionPointer]++;

                switch(instruction.Operator)
                {
                    case "nop": 
                        instructionPointer++;
                        break;
                    case "acc":
                        accumulator+=instruction.OperandAsInt;
                        instructionPointer++;
                        break;
                    case "jmp":
                        instructionPointer+=instruction.OperandAsInt;
                        break;
                    default:
                        throw new Exception("Invalid operator");
                }
                if (instructionPointer>=program.Count) break;
            }
            return (accumulator,instructionPointer);
        }
    }
}
