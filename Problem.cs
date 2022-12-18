global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Numerics;
global using AdventOfCode.Tools;
using HtmlAgilityPack;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

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
            TryDownloadInputs();
            TryDownloadStatement();
            TryReadInputs();
        }

        public int Day { get; private set; }

        public string[] Inputs { get; private set; }

        public string InputsFilePath => GetFilePath(Year, Day, "data");

        public string InputsUrl => $"https://adventofcode.com/{Year}/day/{Day}/input";

        public List<string> Solutions { get; private set; } = new();

        public string StatementFilePath => GetFilePath(Year, Day, "txt");

        public string StatementUrl => $"https://adventofcode.com/{Year}/day/{Day}";

        public int Year { get; private set; }

        public static void GenerateClass(int year, int day)
        {
            string filePath = GetFilePath(year, day, "cs");

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (File.Exists(filePath))
                return;

            string data = File.ReadAllText(Path.Combine(ProjectDir, "Problem.template"));

            data = data.Replace("<YEAR>", year.ToString());
            data = data.Replace("<DAY>", day.ToString("D2"));
            File.WriteAllText(filePath, data);
        }

        public static Problem Get(int year, int day)
        {
            GenerateClass(year, day);

            var assembly = Assembly.GetExecutingAssembly();

            var type = assembly.GetTypes().First(t => t.FullName == $"AdventOfCode._{year}_{day.ToString("D2")}");
            return (Problem)Activator.CreateInstance(type);
        }

        public void AddSolution(object solution)
        {
            _sw.Stop();
            Solutions.Add($"{solution} - in {_sw.ElapsedMilliseconds} ms");
            _sw.Restart();
        }

        public abstract void Solve();

        public void SolveWithTime()
        {
            _sw.Restart();
            Solve();
        }

        public override string ToString() => $"{Year}-{Day:D2}";

        public virtual void WriteData()
        {
            foreach (string s in Inputs)
                Console.WriteLine(s);
        }

        public virtual void WriteHeader()
        {
            Console.WriteLine($"################################################################################");
            Console.WriteLine($"# YEAR:{Year}    DAY:{Day.ToString("D2")}                                                          #");
            Console.WriteLine($"################################################################################");
        }

        public virtual void WriteSolutions()
        {
            for (int i = 0; i < Solutions.Count; i++)
                Console.WriteLine($"SOLUTION N°{i + 1}: {Solutions[i]}");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            if (Solutions.Count >= 2)
                return;//

            Console.WriteLine("Download Part 2 ? (Y/N)");

            if (Console.ReadKey().Key != ConsoleKey.Y)
                return;

            TryDownloadStatement(true);
        }

        private static string GetFilePath(int year, int day, string extension) => Path.Combine(ProjectDir, $"{year}\\{year}_{day:D2}\\", $"{year}_{day:D2}.{extension}");

        private static string GetSession()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory + @"\..\..\..\", "session.key");

            if (File.Exists(filePath))
                return File.ReadAllText(filePath);

            Console.WriteLine("Missing coockie session. Type session then press enter:");
            string session = Console.ReadLine();
            File.WriteAllText(filePath, session);
            return session;
        }

        private string DownloadString(string url)
        {
            Uri baseAddress = new("https://adventofcode.com/");
            using HttpClientHandler handler = new() { UseCookies = false };
            using HttpClient client = new(handler) { BaseAddress = baseAddress };
            HttpRequestMessage message = new(HttpMethod.Post, url);
            message.Headers.Add("Cookie", $"session={Session}");
            HttpResponseMessage response = client.Send(message);
            Task<string> result = response.Content.ReadAsStringAsync();
            result.Wait();
            return result.Result;
        }

        private void TryDownloadInputs()
        {
            if (File.Exists(InputsFilePath))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(InputsFilePath));

            try
            {
                Console.WriteLine($"{this} - downloading inputs");
                File.WriteAllText(InputsFilePath, DownloadString(InputsUrl));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{this} - Error downloading data - {e}");
            }
        }

        private void TryDownloadStatement(bool force = false)
        {
            if (File.Exists(StatementFilePath) && !force)
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(StatementFilePath));

            try
            {
                Console.WriteLine($"{this} - downloading statement");
                var doc = new HtmlDocument();
                doc.LoadHtml(DownloadString(StatementUrl));
                File.WriteAllText(StatementFilePath, string.Join($"\r\n\r\n\r\n\r\n", doc.DocumentNode.SelectNodes("html/body/main/article").Select(n => n.InnerText)));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{this} - Error dowloading statement - {e}");
            }
        }

        private void TryReadInputs()
        {
            try
            {
                Inputs = File.ReadAllLines(InputsFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{this} - Error reading data - {e}");
            }
        }
    }
}