global using AdventOfCode.Tools;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Numerics;
using HtmlAgilityPack;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AdventOfCode
{
    public abstract class Problem
    {
        private static readonly string ProjectDir = Environment.CurrentDirectory + @"\..\..\..\";
        private static readonly string Session = GetSession();
        private readonly Stopwatch _sw = new();

        /// <summary>
        /// //
        /// </summary>
        public Problem()
        {
            string[] names = this.GetType().FullName.Split('.');
            Day = int.Parse(names[1].Substring(6, 2));
            Year = int.Parse(names[1].Substring(1, 4));
            Inputs = File.ReadAllLines(GetFilePath(Year, Day, "data"));
            _sw.Restart();
        }

        public int Day { get; private set; }
        public string[] Inputs { get; private set; }
        public List<string> Solutions { get; private set; } = new();

        public int Year { get; private set; }

        public static void DownloadInputs(int year, int day)
        {
            string filepath = GetFilePath(year, day, "data");

            if (File.Exists(filepath))
                return;

            File.WriteAllText(filepath, HttpHelper.DownloadString(Session, $"https://adventofcode.com/{year}/day/{day}/input"));
        }

        public static void DownloadStatement(int year, int day, bool force = false)
        {
            string filepath = GetFilePath(year, day, "txt");

            if (File.Exists(filepath) && File.ReadAllText(filepath).Contains("--- Part Two ---") && !force)
                return;

            var doc = new HtmlDocument();
            doc.LoadHtml(HttpHelper.DownloadString(Session, $"https://adventofcode.com/{year}/day/{day}"));
            File.WriteAllText(filepath, string.Join($"\r\n\r\n\r\n\r\n", doc.DocumentNode.SelectNodes("html/body/main/article").Select(n => n.InnerText)));
        }

        public static bool GenerateClass(int year, int day)
        {
            string filePath = GetFilePath(year, day, "cs");

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (File.Exists(filePath))
                return false;

            string data = File.ReadAllText(Path.Combine(ProjectDir, "Problem.template"));

            data = data.Replace("<YEAR>", year.ToString());
            data = data.Replace("<DAY>", day.ToString("D2"));
            File.WriteAllText(filePath, data);
            return true;
        }

        public static Problem Get(int year, int day)
        {
            bool generated = GenerateClass(year, day);

            DownloadInputs(year, day);
            DownloadStatement(year, day);

            if (generated)
                return null;

            return (Problem)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetTypes().First(t => t.FullName == $"AdventOfCode._{year}_{day:D2}"));
        }

        public static string GetFilePath(int year, int day, string extension) => Path.Combine(ProjectDir, $"{year}\\{year}_{day:D2}\\", $"{year}_{day:D2}.{extension}");

        public static string GetSession()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory + @"\..\..\..\", "session.key");

            if (File.Exists(filePath))
                return File.ReadAllText(filePath);

            Console.WriteLine("Missing coockie session. Type session then press enter:");
            string session = Console.ReadLine();
            File.WriteAllText(filePath, session);
            return session;
        }

        [Obsolete]
        public void AddSolution(object solution)
        {
            _sw.Stop();
            Solutions.Add($"{solution} - in {_sw.ElapsedMilliseconds} ms");
            _sw.Restart();
        }

        public virtual void Parse()
        {
        }

        public virtual object PartOne()
        {
            return null;
        }

        public virtual object PartTwo()
        {
            return null;
        }

        public virtual void Solve()
        {
        }

        public override string ToString() => $"{Year}-{Day:D2}";
    }
}