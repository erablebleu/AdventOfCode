using System.Text.RegularExpressions;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/04
/// </summary>
public class _2020_04 : Problem
{
    private List<Passport> _passports;

    public override void Parse()
    {
        _passports = new List<Passport>
            {
                new Passport()
            };

        foreach (var line in Inputs.SelectMany(i => i.Split(" ")))
        {
            if (line == string.Empty)
                _passports.Add(new Passport());
            else
            {
                var el2 = line.Split(":");
                _passports.Last().PopulatePassport(el2[0], el2[1]);
            }
        }
    }

    public override object PartOne() => _passports.Count(p => p.IsValid());

    public override object PartTwo() => _passports.Count(p => p.IsValid2());

    private record Passport
    {
        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string> {
            { "byr", null },
            { "iyr", null },
            { "eyr", null },
            { "hgt", null },
            { "hcl", null },
            { "ecl", null },
            { "pid", null },
            { "cid", null },
        };
        private static List<string> ValidEcl = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        private static Regex HclRegex = new Regex("(#[0-9a-f]{6})");

        public void PopulatePassport(string key, string value) => Fields[key] = value;
        public bool IsValid() => Fields.Where(kv => kv.Value == null).All(kv => kv.Key == "cid");
        public bool IsValid2() => IsValid() && Fields.All(kv => IsFieldValid(kv.Key, kv.Value));
        private static bool ValidateIntField(string value, int min, int max) => int.TryParse(value, out var val) && val >= min && val <= max;
        private static bool IsFieldValid(KeyValuePair<string, string> kv) => IsFieldValid(kv.Key, kv.Value);
        private static bool IsFieldValid(string key, string value) => key switch
        {
            "byr" => ValidateIntField(value, 1920, 2002),
            "iyr" => ValidateIntField(value, 2010, 2020),
            "eyr" => ValidateIntField(value, 2020, 2030),
            "hgt" => ValidateHeight(value),
            "hcl" => HclRegex.IsMatch(value),
            "ecl" => ValidEcl.Contains(value),
            "pid" => value.Length == 9 && long.TryParse(value, out var _),
            "cid" => true,
            _ => throw new NotImplementedException(),
        };
        private static bool ValidateHeight(string value)
        {
            if (value.Length < 3) return false;

            return (value[^2..]) switch
            {
                "cm" => ValidateIntField(value[0..^2], 150, 193),
                "in" => ValidateIntField(value[0..^2], 59, 76),
                _ => false,
            };
        }
    }
}