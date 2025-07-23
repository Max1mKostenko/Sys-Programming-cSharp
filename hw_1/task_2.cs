using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

internal class Program
{
    private static int[] numbers;

    static async Task Main(string[] args)
    {
        GenerateNumbers();

        Task<int> maxTask = Task.Run(() => numbers.Max());
        Task<int> minTask = Task.Run(() => numbers.Min());
        Task<double> avgTask = Task.Run(() => numbers.Average());

        await Task.WhenAll(maxTask, minTask, avgTask);

        int max = maxTask.Result;
        int min = minTask.Result;
        double avg = avgTask.Result;

        await WriteToFile(max, min, avg);

        Console.WriteLine($"Max: {max}, Min: {min}, Avg: {avg}");
        Console.ReadKey();
    }

    static void GenerateNumbers()
    {
        numbers = new int[10000];
        Random random = new Random();

        for (int i = 0; i < 10000; i++)
        {
            numbers[i] = random.Next(1, 10001);
        }
    }

    static async Task WriteToFile(int max, int min, double avg)
    {
        using (StreamWriter writer = new StreamWriter("results.txt"))
        {
            await writer.WriteLineAsync($"Max: {max}");
            await writer.WriteLineAsync($"Min: {min}");
            await writer.WriteLineAsync($"Avg: {avg}");
        }
    }
}