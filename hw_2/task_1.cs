using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ParallelProgrammingTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "numbers.txt";

            CreateTestFile(filePath);

            List<int> numbers = ReadNumbersFromFile(filePath);

            Console.WriteLine($"Total numbers: {numbers.Count}\n");

            CountUniqueWithLINQ(numbers);

            CountUniqueWithPLINQ(numbers);

            Console.ReadKey();
        }

        static void CreateTestFile(string filePath)
        {
            if (File.Exists(filePath))
                return;

            Random random = new Random();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < 100000; i++)
                {
                    writer.WriteLine(random.Next(1, 1000));
                }
            }
        }

        static List<int> ReadNumbersFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            return lines.Select(line => int.Parse(line)).ToList();
        }

        static void CountUniqueWithLINQ(List<int> numbers)
        {
            Console.WriteLine("--- LINQ ---");
            Stopwatch sw = Stopwatch.StartNew();

            int uniqueCount = numbers.Distinct().Count();

            sw.Stop();
            Console.WriteLine($"Unique values: {uniqueCount}");
            Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms\n");
        }

        static void CountUniqueWithPLINQ(List<int> numbers)
        {
            Console.WriteLine("--- PLINQ ---");
            Stopwatch sw = Stopwatch.StartNew();

            int uniqueCount = numbers.AsParallel().Distinct().Count();

            sw.Stop();
            Console.WriteLine($"Unique values: {uniqueCount}");
            Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms");
        }
    }
}