using System;
using System.Threading;

namespace ConsoleApp
{
    internal class Program
    {
        struct Range
        {
            public int Start;
            public int End;

            public Range(int start, int end)
            {
                Start = start;
                End = end;
            }
        }

        static void PrintRange(object obj)
        {
            Range range = (Range)obj;
            for (int i = range.Start; i <= range.End; i++)
            {
                Console.WriteLine(i);
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Enter start of range: ");
            int start = int.Parse(Console.ReadLine());

            Console.Write("Enter end of range: ");
            int end = int.Parse(Console.ReadLine());

            Console.Write("Enter number of threads: ");
            int numThreads = int.Parse(Console.ReadLine());

            int rangeSize = (end - start + 1) / numThreads;
            int remainder = (end - start + 1) % numThreads;

            Thread[] threads = new Thread[numThreads];
            int currentStart = start;

            for (int i = 0; i < numThreads; i++)
            {
                int currentEnd = currentStart + rangeSize - 1;
                if (i == numThreads - 1)
                    currentEnd += remainder;

                Range range = new Range(currentStart, currentEnd);
                threads[i] = new Thread(PrintRange);
                threads[i].Start(range);

                currentStart = currentEnd + 1;
            }
        }
    }
}
