using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace TaskExercises;
class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            //when all example with shared results
            FakeApi fakeApi = new();

            ConcurrentBag<(string, int)> salaryList = new();

            Task[] tasks = new[] {
                Task.Run(() => fakeApi.GetSalaryByName("Adam", salaryList)), //task items will start running even before you await WhenAll
                Task.Run(() => fakeApi.GetSalaryByName("Eve", salaryList, 10)),
                Task.Run(() => fakeApi.GetSalaryByName("John", salaryList, 1, true))
            };
            var whenAllTask = Task.WhenAll(tasks);

            try
            {
                await whenAllTask;
            }
            catch { } //we want to eat the exception so that we can report that some of the tasks failed

            switch (whenAllTask.Status)
            {
                case TaskStatus.RanToCompletion:
                    Console.WriteLine("Found everyone, Total Salary: {0}", salaryList.Sum(c => c.Item2));
                    break;
                case TaskStatus.Faulted:
                    Console.WriteLine(
                        "Some failed, Total Salary: {0} Inner Exception(s): {1}"
                        ,salaryList.Sum(c => c.Item2), string.Join(" | "
                        ,whenAllTask.Exception?.InnerExceptions.Select(ie => ie.Message) ?? Array.Empty<string>()) //the exceptions that the task item methods throw are inner
                    );
                    break;
                default:
                    Console.WriteLine("Something else happened, Total Salary: {0}", salaryList.Sum(c => c.Item2));
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Main Error: {ex.Message}");
        }
        
        Console.ReadKey();
    }
}