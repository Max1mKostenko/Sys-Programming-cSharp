using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp53
{
    class Program
    {
        private static int[] array;
        private static int searchValue = 25;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Array Operations with Continuation Tasks\n");

            await Task.Run(() => FillArray())
                .ContinueWith(task => DisplayArray("Original Array:"), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(task => RemoveDuplicates(), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(task => DisplayArray("After Removing Duplicates:"), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(task => SortArray(), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(task => DisplayArray("After Sorting:"), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(task => BinarySearch(searchValue), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(task =>
                {
                    if (task.Result >= 0)
                        Console.WriteLine($"Binary Search: Value {searchValue} found at index {task.Result}");
                    else
                        Console.WriteLine($"Binary Search: Value {searchValue} not found in the array");
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Console.WriteLine("\nAll operations completed successfully!");
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        private static void FillArray()
        {
            array = new int[] { 45, 12, 78, 23, 56, 12, 89, 34, 23, 67, 45, 25, 91, 78, 15 };

            Console.WriteLine($"Array filled with {array.Length} elements");
            System.Threading.Thread.Sleep(500);
        }

        private static void DisplayArray(string message)
        {
            if (array != null && array.Length > 0)
            {
                Console.WriteLine($"Array contents: [{string.Join(", ", array)}]");
                Console.WriteLine($"Array length: {array.Length}");
            }
            else
            {
                Console.WriteLine("Array is empty or null");
            }
            System.Threading.Thread.Sleep(300);
        }

        private static void RemoveDuplicates()
        {
            if (array != null && array.Length > 0)
            {
                int originalLength = array.Length;
                array = array.Distinct().ToArray();

                Console.WriteLine($"Array length changed from {originalLength} to {array.Length}");
            }
            else
            {
                Console.WriteLine("Cannot remove duplicates");
            }
            System.Threading.Thread.Sleep(400);
        }

        private static void SortArray()
        {

            if (array != null && array.Length > 0)
            {
                Array.Sort(array);
                Console.WriteLine("Array sorted in ascending order");
            }
            else
            {
                Console.WriteLine("Cannot sort empty array");
            }
            System.Threading.Thread.Sleep(300);
        }

        private static int BinarySearch(int value)
        {
            if (array != null && array.Length > 0)
            {
                int result = Array.BinarySearch(array, value);

                if (result >= 0)
                {
                    Console.WriteLine($"Binary search completed successfully");
                }
                else
                {
                    Console.WriteLine($"Binary search completed - value not found");
                }

                System.Threading.Thread.Sleep(200);
                return result;
            }
            else
            {
                Console.WriteLine("Cannot perform binary search on empty array");
                return -1;
            }
        }

        private static int ManualBinarySearch(int value)
        {
            if (array == null || array.Length == 0)
                return -1;

            int left = 0;
            int right = array.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (array[mid] == value)
                    return mid;
                else if (array[mid] < value)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return -1;
        }
    }
}