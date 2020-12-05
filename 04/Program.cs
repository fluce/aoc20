using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _04
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args.FirstOrDefault()??"input.txt").ToArray();

            var currentRecord=new Dictionary<string,string>();
            int validCount=0;
            int validCount2=0;

            foreach(var line in lines)
            {
                if (string.IsNullOrEmpty(line)) {
                    if (CheckRecord(currentRecord)) validCount++;
                    if (CheckRecordV2(currentRecord)) validCount2++;
                    currentRecord.Clear();
                }
                foreach (var couple in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Split(':')))
                    currentRecord[couple[0]]=couple[1];
            }
            if (CheckRecord(currentRecord)) validCount++;
            if (CheckRecordV2(currentRecord)) validCount2++;
            Console.WriteLine(validCount);
            Console.WriteLine(validCount2);
        }

        static string[] fieldList=new[] { "byr","iyr","eyr","hgt","hcl","ecl","pid" }.OrderBy(x=>x).ToArray();

        static bool CheckRecord(Dictionary<string,string> record)
        {
            return record.Keys.Where(x=>x!="cid").OrderBy(x=>x).SequenceEqual(fieldList);
        }

        static bool CheckRecordV2(Dictionary<string,string> record)
        {
            if (!CheckRecord(record)) return false;

            if (!(Regex.IsMatch(record["byr"],@"^\d\d\d\d$") && int.TryParse(record["byr"],out var yr1) && yr1>=1920 && yr1<=2002)) return false; 
            if (!(Regex.IsMatch(record["iyr"],@"^\d\d\d\d$") && int.TryParse(record["iyr"],out var yr2) && yr2>=2010 && yr2<=2020)) return false; 
            if (!(Regex.IsMatch(record["eyr"],@"^\d\d\d\d$") && int.TryParse(record["eyr"],out var yr3) && yr3>=2020 && yr3<=2030)) return false; 

            if (! (
                    (Regex.IsMatch(record["hgt"],@"^\d+cm$") && int.TryParse(record["hgt"][0..^2],out var h1) && h1>=150 && h1<=193)
                 || (Regex.IsMatch(record["hgt"],@"^\d+in$") && int.TryParse(record["hgt"][0..^2],out var h2) && h2>=59 && h2<=76)
              )     
            ) return false; 
            if (!(Regex.IsMatch(record["hcl"],@"^#[0-9a-f]{6}$"))) return false; 
            if (!(Regex.IsMatch(record["ecl"],@"^(amb|blu|brn|gry|grn|hzl|oth)$"))) return false; 
            if (!(Regex.IsMatch(record["pid"],@"^\d{9}$"))) return false; 
            return true;
        }

    }
}
